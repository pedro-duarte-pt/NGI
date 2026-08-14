using System;
using System.Collections.Generic;
using System.Globalization;

public sealed class ScreenDefinition
{
    public string Id, Title, Style;
    public ScreenLayoutDefinition Layout;
    public IReadOnlyList<WidgetDefinition> Widgets;

    public static ScreenDefinition FromJson(string json)
    {
        var root = AddonJson.Obj(AddonJson.Parse(json));
        if (root == null) return null;

        return new ScreenDefinition
        {
            Id = AddonJson.Str(root, "id"),
            Title = AddonJson.Str(root, "title"),
            Style = AddonJson.Str(root, "style"),
            Layout = ScreenLayoutDefinition.From(
                root.TryGetValue("layout", out object l) ? AddonJson.Obj(l) : null),
            Widgets = WidgetDefinition.List(
                root.TryGetValue("widgets", out object w) ? w : null)
        };
    }
}

public sealed class ScreenLayoutDefinition
{
    public string Type = "grid";
    public int Columns = 10;
    public int Rows = 7;
    public float Gap = 12f;
    public float Padding = 0f;

    public static ScreenLayoutDefinition From(Dictionary<string, object> obj)
    {
        var result = new ScreenLayoutDefinition();
        if (obj == null) return result;

        result.Type = AddonJson.Str(obj, "type", "grid");
        result.Columns = Math.Max(1, AddonJson.Int(obj, "columns", 10));
        result.Rows = Math.Max(1, AddonJson.Int(obj, "rows", 7));
        result.Gap = Float(obj, "gap", 12f);
        result.Padding = Float(obj, "padding", 0f);
        return result;
    }

    private static float Float(Dictionary<string, object> obj, string key, float fallback)
    {
        if (!obj.TryGetValue(key, out object value) || value == null) return fallback;
        try { return Convert.ToSingle(value, CultureInfo.InvariantCulture); }
        catch { return fallback; }
    }
}

public sealed class WidgetDefinition
{
    public string Type, Id, StyleClass;
    public WidgetLayout Layout;
    public IReadOnlyDictionary<string, object> Props;
    public IReadOnlyList<WidgetDefinition> Children;

    public static WidgetDefinition From(Dictionary<string, object> obj)
    {
        if (obj == null) return null;

        return new WidgetDefinition
        {
            Type = AddonJson.Str(obj, "type"),
            Id = AddonJson.Str(obj, "id"),
            StyleClass = AddonJson.Str(obj, "styleClass"),
            Layout = WidgetLayout.From(
                obj.TryGetValue("layout", out object l) ? AddonJson.Obj(l) : null),
            Props = obj.TryGetValue("props", out object p)
                ? AddonJson.Obj(p) ?? new Dictionary<string, object>()
                : new Dictionary<string, object>(),
            Children = List(
                obj.TryGetValue("children", out object c) ? c : null)
        };
    }

    public static IReadOnlyList<WidgetDefinition> List(object raw)
    {
        var array = AddonJson.Arr(raw);
        var result = new List<WidgetDefinition>();
        if (array == null) return result;

        foreach (object item in array)
        {
            WidgetDefinition node = From(AddonJson.Obj(item));
            if (node != null) result.Add(node);
        }
        return result;
    }

    public string PropString(string key, string fallback = "") =>
        Props != null && Props.TryGetValue(key, out object v) && v != null
            ? Convert.ToString(v, CultureInfo.InvariantCulture) : fallback;

    public float PropFloat(string key, float fallback = 0f)
    {
        try { return Props != null && Props.TryGetValue(key, out object v) ? Convert.ToSingle(v, CultureInfo.InvariantCulture) : fallback; }
        catch { return fallback; }
    }

    public int PropInt(string key, int fallback = 0)
    {
        try { return Props != null && Props.TryGetValue(key, out object v) ? Convert.ToInt32(v, CultureInfo.InvariantCulture) : fallback; }
        catch { return fallback; }
    }

    public string[] PropStringArray(string key)
    {
        if (Props == null || !Props.TryGetValue(key, out object v)) return null;
        var array = AddonJson.Arr(v);
        if (array == null) return null;

        var result = new string[array.Count];
        for (int i = 0; i < array.Count; i++)
            result[i] = Convert.ToString(array[i], CultureInfo.InvariantCulture);
        return result;
    }
}

public sealed class WidgetLayout
{
    public int X, Y, Width = 1, Height = 1;

    public static WidgetLayout From(Dictionary<string, object> obj) =>
        obj == null ? null : new WidgetLayout
        {
            X = AddonJson.Int(obj, "x"),
            Y = AddonJson.Int(obj, "y"),
            Width = Math.Max(1, AddonJson.Int(obj, "width", 1)),
            Height = Math.Max(1, AddonJson.Int(obj, "height", 1))
        };
}
