using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenLoader : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private float refreshRateHz = 20f;

    public ScreenDefinition CurrentScreen { get; private set; }

    private readonly List<IRuntimeWidget> runtimeWidgets = new List<IRuntimeWidget>();
    private readonly ExternalStyleLoader styleLoader = new ExternalStyleLoader();

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
                out AddonScreenEntry screenEntry))
        {
            Debug.LogError("Addon screen not found: " + addonId + " / " + screenId);
            return;
        }

        if (string.IsNullOrWhiteSpace(screenEntry.path))
        {
            Debug.LogError("Screen path is empty for: " + addonId + " / " + screenId);
            return;
        }

        LoadScreenFromFolder(
            addonId,
            screenId,
            addon.RootPath,
            Path.Combine(addon.RootPath, screenEntry.path)
        );
    }

    private void LoadScreenFromFolder(
        string addonId,
        string screenId,
        string addonRoot,
        string screenFolder)
    {
        string screenPath = Path.Combine(screenFolder, "screen.json");

        if (!File.Exists(screenPath))
        {
            Debug.LogError("Screen file not found: " + screenPath);
            return;
        }

        CurrentScreen =
            JsonUtility.FromJson<ScreenDefinition>(File.ReadAllText(screenPath));

        if (CurrentScreen == null || CurrentScreen.widgets == null)
        {
            Debug.LogError("Invalid screen definition: " + screenPath);
            return;
        }

        string stylePath = string.IsNullOrWhiteSpace(CurrentScreen.style)
            ? null
            : Path.Combine(screenFolder, CurrentScreen.style);

        if (!styleLoader.Load(stylePath))
            Debug.LogWarning("Screen will continue without external styles.");

        BuildScreen(addonId, addonRoot);

        Debug.Log(
            "[SCREEN OK] " + addonId + " / " + screenId +
            " -> " + CurrentScreen.title +
            " (" + CurrentScreen.widgets.Length + " widgets)"
        );
    }

    private void BuildScreen(string addonId, string addonRoot)
    {
        runtimeWidgets.Clear();

        if (currentScreenContainer != null)
            currentScreenContainer.RemoveFromHierarchy();

        currentScreenContainer = new VisualElement();
        currentScreenContainer.name = "screen-" + CurrentScreen.id;
        currentScreenContainer.AddToClassList("screen-root");
        currentScreenContainer.AddToClassList("addon-" + SanitizeClassName(addonId));

        currentScreenContainer.style.position = Position.Absolute;
        currentScreenContainer.style.left = 0;
        currentScreenContainer.style.top = 0;
        currentScreenContainer.style.right = 0;
        currentScreenContainer.style.bottom = 0;

        documentRoot.Add(currentScreenContainer);

        var context = new WidgetContext
        {
            AddonRootPath = addonRoot,
            RefreshStyle = styleLoader.RefreshElement
        };

        foreach (WidgetDefinition definition in CurrentScreen.widgets)
        {
            IRuntimeWidget widget = WidgetFactory.Create(definition, context);

            if (widget == null)
                continue;

            VisualElement element = widget.Root;

            if (!string.IsNullOrWhiteSpace(definition.styleClass))
                element.AddToClassList(definition.styleClass);

            ApplyLayout(element, definition);
            currentScreenContainer.Add(element);
            runtimeWidgets.Add(widget);
        }

        styleLoader.ApplyRecursive(currentScreenContainer);
    }

    private static void ApplyLayout(VisualElement element, WidgetDefinition definition)
    {
        const float cellWidth = 180f;
        const float cellHeight = 110f;
        const float gap = 12f;

        element.style.position = Position.Absolute;
        element.style.left = definition.x * (cellWidth + gap);
        element.style.top = definition.y * (cellHeight + gap);
        element.style.width =
            definition.width * cellWidth + (definition.width - 1) * gap;
        element.style.height =
            definition.height * cellHeight + (definition.height - 1) * gap;
    }

    private static string SanitizeClassName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "unknown";

        return value.Replace(".", "-")
                    .Replace("_", "-")
                    .Replace(" ", "-")
                    .ToLowerInvariant();
    }
}
