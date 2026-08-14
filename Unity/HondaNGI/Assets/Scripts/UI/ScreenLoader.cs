using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private float refreshRateHz = 20f;

    public ScreenDefinition CurrentScreen { get; private set; }

    private readonly List<IRuntimeWidget> runtimeWidgets =
        new List<IRuntimeWidget>();

    private readonly AddonStyleRuntime styleRuntime =
        new AddonStyleRuntime();

    private VisualElement documentRoot;
    private VisualElement currentScreenContainer;
    private float nextRefreshTime;

    private void Awake()
    {
        if (uiDocument == null)
        {
            Debug.LogError("ScreenLoader requires a UIDocument reference.");
            enabled = false;
            return;
        }

        documentRoot = uiDocument.rootVisualElement;
    }

    private void Update()
    {
        if (refreshRateHz <= 0f || Time.unscaledTime < nextRefreshTime)
            return;

        nextRefreshTime = Time.unscaledTime + (1f / refreshRateHz);

        foreach (IRuntimeWidget widget in runtimeWidgets)
            widget.Refresh();
    }

    public void LoadAddonScreen(string addonId, string screenId)
    {
        if (!AddonRegistry.TryGetScreen(
                addonId, screenId,
                out LoadedAddon addon,
                out AddonScreenEntry entry))
        {
            Debug.LogError("Addon screen not found: " + addonId + " / " + screenId);
            return;
        }

        if (string.IsNullOrWhiteSpace(entry.path))
        {
            Debug.LogError("Screen path is empty for: " + addonId + " / " + screenId);
            return;
        }

        LoadScreenFromFolder(
            addonId,
            screenId,
            addon.RootPath,
            Path.Combine(addon.RootPath, entry.path));
    }

    private void LoadScreenFromFolder(
        string addonId,
        string screenId,
        string addonRoot,
        string folder)
    {
        string screenPath = Path.Combine(folder, "screen.json");

        if (!File.Exists(screenPath))
        {
            Debug.LogError("Screen file not found: " + screenPath);
            return;
        }

        try
        {
            CurrentScreen =
                ScreenDefinition.FromJson(File.ReadAllText(screenPath));
        }
        catch (System.Exception ex)
        {
            Debug.LogError(
                "Invalid screen definition: " +
                screenPath + " | " + ex.Message);
            return;
        }

        if (CurrentScreen == null || CurrentScreen.Widgets == null)
        {
            Debug.LogError("Invalid screen definition: " + screenPath);
            return;
        }

        string stylePath =
            string.IsNullOrWhiteSpace(CurrentScreen.Style)
                ? null
                : Path.Combine(folder, CurrentScreen.Style);

        if (!styleRuntime.Load(stylePath))
            Debug.LogWarning("Screen will continue without external styles.");

        BuildScreen(addonId, addonRoot);

        Debug.Log(
            "[SCREEN OK] " + addonId + " / " + screenId +
            " -> " + CurrentScreen.Title +
            " (" + CurrentScreen.Widgets.Count + " widgets)");
    }

    private void BuildScreen(string addonId, string addonRoot)
    {
        runtimeWidgets.Clear();

        if (currentScreenContainer != null)
            currentScreenContainer.RemoveFromHierarchy();

        currentScreenContainer = new VisualElement();
        currentScreenContainer.name = "screen-" + CurrentScreen.Id;
        currentScreenContainer.AddToClassList("screen-root");
        currentScreenContainer.AddToClassList(
            "addon-" + Sanitize(addonId));

        currentScreenContainer.style.position = Position.Absolute;
        currentScreenContainer.style.left = 0;
        currentScreenContainer.style.top = 0;
        currentScreenContainer.style.right = 0;
        currentScreenContainer.style.bottom = 0;

        documentRoot.Add(currentScreenContainer);

        var context = new WidgetContext
        {
            AddonRootPath = addonRoot,
            Styles = styleRuntime
        };

        foreach (WidgetDefinition definition in CurrentScreen.Widgets)
            BuildTree(
                definition,
                context,
                currentScreenContainer,
                true);

        styleRuntime.ApplyRecursive(currentScreenContainer);

        // Layout must use the real resolved size of the UI container.
        currentScreenContainer.RegisterCallback<GeometryChangedEvent>(
            OnScreenGeometryChanged);

        ApplyScreenLayout();
    }

    private void BuildTree(
        WidgetDefinition definition,
        WidgetContext context,
        VisualElement parent,
        bool topLevel)
    {
        IRuntimeWidget widget =
            WidgetFactory.Create(definition, context);

        if (widget == null)
            return;

        VisualElement element = widget.Root;

        if (!string.IsNullOrWhiteSpace(definition.Id))
            element.name = definition.Id;

        if (!string.IsNullOrWhiteSpace(definition.StyleClass))
            element.AddToClassList(definition.StyleClass);

        // Store top-level grid placement; actual pixels are calculated later.
        if (topLevel && definition.Layout != null)
        {
            element.userData = definition.Layout;
            element.style.position = Position.Absolute;
        }

        parent.Add(element);
        runtimeWidgets.Add(widget);

        if (definition.Children == null)
            return;

        foreach (WidgetDefinition child in definition.Children)
            BuildTree(child, context, element, false);
    }

    private void OnScreenGeometryChanged(GeometryChangedEvent evt)
    {
        ApplyScreenLayout();
    }

    private void ApplyScreenLayout()
    {
        if (currentScreenContainer == null || CurrentScreen == null)
            return;

        ScreenLayoutDefinition layout =
            CurrentScreen.Layout ?? new ScreenLayoutDefinition();

        if (!layout.Type.Equals(
                "grid",
                System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogWarning(
                "Unsupported screen layout type: " + layout.Type);
            return;
        }

        Rect rect = currentScreenContainer.contentRect;

        if (rect.width <= 1f || rect.height <= 1f)
            return;

        float padding = Mathf.Max(0f, layout.Padding);
        float gap = Mathf.Max(0f, layout.Gap);

        float usableWidth =
            rect.width -
            (padding * 2f) -
            (gap * (layout.Columns - 1));

        float usableHeight =
            rect.height -
            (padding * 2f) -
            (gap * (layout.Rows - 1));

        if (usableWidth <= 0f || usableHeight <= 0f)
            return;

        float cellWidth = usableWidth / layout.Columns;
        float cellHeight = usableHeight / layout.Rows;

        foreach (VisualElement element in currentScreenContainer.Children())
        {
            if (!(element.userData is WidgetLayout widgetLayout))
                continue;

            ApplyGridCell(
                element,
                widgetLayout,
                layout,
                cellWidth,
                cellHeight);
        }
    }

    private static void ApplyGridCell(
        VisualElement element,
        WidgetLayout widget,
        ScreenLayoutDefinition screen,
        float cellWidth,
        float cellHeight)
    {
        int x = Mathf.Clamp(widget.X, 0, screen.Columns - 1);
        int y = Mathf.Clamp(widget.Y, 0, screen.Rows - 1);

        int width = Mathf.Clamp(
            widget.Width,
            1,
            screen.Columns - x);

        int height = Mathf.Clamp(
            widget.Height,
            1,
            screen.Rows - y);

        element.style.left =
            screen.Padding + x * (cellWidth + screen.Gap);

        element.style.top =
            screen.Padding + y * (cellHeight + screen.Gap);

        element.style.width =
            width * cellWidth + (width - 1) * screen.Gap;

        element.style.height =
            height * cellHeight + (height - 1) * screen.Gap;
    }

    private static string Sanitize(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? "unknown"
            : value.Replace(".", "-")
                   .Replace("_", "-")
                   .Replace(" ", "-")
                   .ToLowerInvariant();
}
