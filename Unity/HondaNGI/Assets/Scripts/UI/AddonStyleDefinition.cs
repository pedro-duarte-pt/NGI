using System;

[Serializable]
public sealed class AddonStyleSheetDefinition
{
    public AddonStyleToken[] tokens;
    public AddonStyleRuleDefinition[] styles;
}

[Serializable]
public sealed class AddonStyleToken
{
    public string name;
    public string value;
}

[Serializable]
public sealed class AddonStyleRuleDefinition
{
    public string name;
    public AddonStyleProperties style;
    public AddonStyleStateDefinition[] states;
}

[Serializable]
public sealed class AddonStyleStateDefinition
{
    public string name;
    public AddonStyleProperties style;
}

[Serializable]
public sealed class AddonStyleProperties
{
    public string backgroundColor;
    public string color;

    public float fontSize;
    public string fontWeight;
    public string textAlign;

    public float width;
    public float height;
    public float minWidth;
    public float minHeight;

    public float paddingLeft;
    public float paddingRight;
    public float paddingTop;
    public float paddingBottom;

    public float marginLeft;
    public float marginRight;
    public float marginTop;
    public float marginBottom;

    public string borderColor;
    public float borderWidth;
    public float borderRadius;

    public string layoutDirection;
    public string alignItems;
    public string justifyContent;
    public float flexGrow;

    // Set opacitySet=true when opacity should be applied.
    public bool opacitySet;
    public float opacity;
}
