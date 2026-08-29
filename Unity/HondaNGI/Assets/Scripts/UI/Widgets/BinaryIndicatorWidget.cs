using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class BinaryIndicatorWidget : IRuntimeWidget
{
    private readonly SensorDefinition sensor;
    private readonly Image image;
    private readonly Texture2D onTexture;
    private readonly Texture2D offTexture;

    public VisualElement Root { get; }

    public BinaryIndicatorWidget(WidgetDefinition definition, WidgetContext context)
    {
        sensor = SensorRegistry.Get(definition.PropString("sensor"));

        Root = new VisualElement();
        Root.AddToClassList("sensor-widget");

        var label = new Label(definition.PropString(
            "label",
            sensor != null ? sensor.ShortName : "STATE"));
        label.AddToClassList("sensor-widget-name");
        Root.Add(label);

        image = new Image();
        image.AddToClassList("binary-indicator-image");
        image.scaleMode = ScaleMode.ScaleToFit;
        image.style.flexGrow = 1;
        Root.Add(image);

        onTexture = LoadTexture(context, definition.PropString("onAsset"));
        offTexture = LoadTexture(context, definition.PropString("offAsset"));

        Refresh();
    }

    public void Refresh()
    {
        image.image = sensor != null && sensor.Value >= 0.5f
            ? onTexture
            : offTexture;
    }

    private static Texture2D LoadTexture(WidgetContext context, string asset)
    {
        if (context == null ||
            string.IsNullOrWhiteSpace(context.AddonRootPath) ||
            string.IsNullOrWhiteSpace(asset))
            return null;

        string path = Path.Combine(
            context.AddonRootPath,
            asset.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(path))
        {
            Debug.LogWarning("Binary indicator asset not found: " + path);
            return null;
        }

        byte[] bytes = File.ReadAllBytes(path);
        var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (!texture.LoadImage(bytes))
        {
            Object.Destroy(texture);
            return null;
        }

        texture.name = Path.GetFileNameWithoutExtension(path);
        return texture;
    }
}
