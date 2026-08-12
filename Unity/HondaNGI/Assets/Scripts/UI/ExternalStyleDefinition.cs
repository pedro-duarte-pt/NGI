using System;

[Serializable]
public class ExternalStyleSheetDefinition
{
    public StyleRuleDefinition[] rules;
}

[Serializable]
public class StyleRuleDefinition
{
    // Class selector without the dot.
    // Example: "sensor-widget"
    public string selector;

    public string backgroundColor;
    public string color;

    public int fontSize;
    public bool bold;

    public float paddingLeft;
    public float paddingRight;
    public float paddingTop;
    public float paddingBottom;

    public float marginLeft;
    public float marginRight;
    public float marginTop;
    public float marginBottom;

    public float borderRadius;

    public float borderWidth;
    public string borderColor;

    // Optional layout properties.
    public string flexDirection;   // row / column
    public string alignItems;      // flex-start / center / flex-end / stretch
    public string justifyContent;  // flex-start / center / flex-end / space-between
    public string textAlign;       // upper-left / middle-center / lower-right etc.
}
