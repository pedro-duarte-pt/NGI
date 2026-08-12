using System;

[Serializable]
public class ScreenDefinition
{
    public string id;
    public string title;
    public string style;
    public WidgetDefinition[] widgets;
}

[Serializable]
public class WidgetDefinition
{
    public string type;

    public string sensor;
    public string[] sensors;
    public string traceColor;

    public string text;
    public string subtitle;

    // Generic addon image asset path, relative to addon root.
    public string asset;
    public string scaleMode;

    public int x;
    public int y;
    public int width;
    public int height;

    public string styleClass;

    public float windowSeconds;
    public int maxTraces;
    public string[] traceColors;
}
