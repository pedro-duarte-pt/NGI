using System;
[Serializable] public sealed class AddonStyleSheetDefinition { public AddonStyleToken[] tokens; public AddonStyleRuleDefinition[] styles; }
[Serializable] public sealed class AddonStyleToken { public string name; public string value; }
[Serializable] public sealed class AddonStyleRuleDefinition { public string name; public AddonStyleProperties style; public AddonStyleStateDefinition[] states; }
[Serializable] public sealed class AddonStyleStateDefinition { public string name; public AddonStyleProperties style; }
[Serializable]
public sealed class AddonStyleProperties
{
    public string backgroundColor, color, fontWeight, textAlign, borderColor, layoutDirection, alignItems, justifyContent;
    public OptionalFloat fontSize, width, height, minWidth, minHeight;
    public OptionalFloat paddingLeft, paddingRight, paddingTop, paddingBottom;
    public OptionalFloat marginLeft, marginRight, marginTop, marginBottom;
    public OptionalFloat borderWidth, borderRadius, flexGrow, opacity;
}
