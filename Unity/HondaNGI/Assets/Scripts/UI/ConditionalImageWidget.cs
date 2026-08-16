using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Generic declarative image widget. Cases are evaluated in order and the
/// first matching case selects an add-on image. If none match, the default
/// image is used.
/// </summary>
public sealed class ConditionalImageWidget : IRuntimeWidget
{
    private sealed class Condition
    {
        public SensorDefinition Sensor;
        public string Operator;
        public float Value;
    }

    private sealed class ImageCase
    {
        public Condition When;
        public string Asset;
    }

    private readonly List<ImageCase> cases = new List<ImageCase>();
    private readonly Dictionary<string, Texture2D> textures =
        new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);

    private readonly string defaultAsset;
    private readonly string addonRootPath;
    private readonly VisualElement root;
    private readonly Image image;
    private string activeAsset;

    public VisualElement Root => root;

    public ConditionalImageWidget(WidgetDefinition definition, WidgetContext context)
    {
        addonRootPath = context?.AddonRootPath;

        if (definition.Props != null &&
            definition.Props.TryGetValue("cases", out object rawCases))
        {
            List<object> array = AddonJson.Arr(rawCases);
            if (array != null)
            {
                foreach (object rawCase in array)
                {
                    ImageCase parsed = ParseCase(AddonJson.Obj(rawCase));
                    if (parsed != null)
                        cases.Add(parsed);
                }
            }
        }

        if (definition.Props != null &&
            definition.Props.TryGetValue("default", out object rawDefault))
        {
            Dictionary<string, object> obj = AddonJson.Obj(rawDefault);
            defaultAsset = obj == null ? "" : AddonJson.Str(obj, "asset");
        }
        else
        {
            defaultAsset = "";
        }

        root = new VisualElement();
        root.AddToClassList("conditional-image-widget");
        root.style.overflow = Overflow.Hidden;

        image = new Image();
        image.AddToClassList("conditional-image-widget-image");
        image.style.flexGrow = 1;
        image.style.width = Length.Percent(100f);
        image.style.height = Length.Percent(100f);

        switch (definition.PropString("scaleMode").ToLowerInvariant())
        {
            case "stretch": image.scaleMode = ScaleMode.StretchToFill; break;
            case "crop": image.scaleMode = ScaleMode.ScaleAndCrop; break;
            default: image.scaleMode = ScaleMode.ScaleToFit; break;
        }

        root.Add(image);
        Refresh();
    }

    public void Refresh()
    {
        string selected = SelectAsset();
        if (string.Equals(selected, activeAsset, StringComparison.OrdinalIgnoreCase))
            return;

        activeAsset = selected;
        image.image = LoadTexture(selected);
    }

    private string SelectAsset()
    {
        foreach (ImageCase item in cases)
        {
            if (Matches(item.When))
                return item.Asset;
        }

        return defaultAsset;
    }

    private Texture2D LoadTexture(string asset)
    {
        if (string.IsNullOrWhiteSpace(asset) || string.IsNullOrWhiteSpace(addonRootPath))
            return null;

        if (textures.TryGetValue(asset, out Texture2D cached))
            return cached;

        string path = Path.Combine(
            addonRootPath,
            asset.Replace('/', Path.DirectorySeparatorChar));

        if (!File.Exists(path))
        {
            Debug.LogWarning("Conditional image asset not found: " + path);
            return null;
        }

        byte[] bytes = File.ReadAllBytes(path);
        var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!texture.LoadImage(bytes))
        {
            Debug.LogWarning("Could not decode conditional image asset: " + path);
            UnityEngine.Object.Destroy(texture);
            return null;
        }

        texture.name = Path.GetFileNameWithoutExtension(path);
        textures[asset] = texture;
        return texture;
    }

    private static ImageCase ParseCase(Dictionary<string, object> obj)
    {
        if (obj == null || !obj.TryGetValue("when", out object rawWhen))
            return null;

        Condition condition = ParseCondition(AddonJson.Obj(rawWhen));
        string asset = AddonJson.Str(obj, "asset");

        if (condition == null || string.IsNullOrWhiteSpace(asset))
            return null;

        return new ImageCase { When = condition, Asset = asset };
    }

    private static Condition ParseCondition(Dictionary<string, object> obj)
    {
        if (obj == null)
            return null;

        SensorDefinition sensor = SensorRegistry.Get(AddonJson.Str(obj, "sensor"));
        if (sensor == null)
            return null;

        float value;
        try
        {
            value = obj.TryGetValue("value", out object raw)
                ? Convert.ToSingle(raw, CultureInfo.InvariantCulture)
                : 0f;
        }
        catch
        {
            value = 0f;
        }

        return new Condition
        {
            Sensor = sensor,
            Operator = AddonJson.Str(obj, "operator", "=="),
            Value = value
        };
    }

    private static bool Matches(Condition condition)
    {
        if (condition == null || condition.Sensor == null)
            return false;

        float actual = condition.Sensor.Value;
        float expected = condition.Value;

        switch (condition.Operator)
        {
            case "<": return actual < expected;
            case "<=": return actual <= expected;
            case ">": return actual > expected;
            case ">=": return actual >= expected;
            case "==": return Mathf.Approximately(actual, expected);
            case "!=": return !Mathf.Approximately(actual, expected);
            default: return false;
        }
    }
}
