using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class AddonStyleApplier
{
    public static void Apply(VisualElement element, AddonStyleProperties style)
    {
        if (element == null || style == null)
            return;

        Reset(element);

        if (TryColor(style.backgroundColor, out Color background))
            element.style.backgroundColor = background;

        if (TryColor(style.color, out Color foreground))
            element.style.color = foreground;

        if (style.fontSize > 0)
            element.style.fontSize = style.fontSize;

        if (!string.IsNullOrWhiteSpace(style.fontWeight))
        {
            element.style.unityFontStyleAndWeight =
                style.fontWeight.Equals("bold", StringComparison.OrdinalIgnoreCase)
                    ? FontStyle.Bold
                    : FontStyle.Normal;
        }

        if (!string.IsNullOrWhiteSpace(style.textAlign))
            element.style.unityTextAlign = ParseTextAnchor(style.textAlign);

        if (style.width > 0) element.style.width = style.width;
        if (style.height > 0) element.style.height = style.height;
        if (style.minWidth > 0) element.style.minWidth = style.minWidth;
        if (style.minHeight > 0) element.style.minHeight = style.minHeight;

        if (style.paddingLeft != 0) element.style.paddingLeft = style.paddingLeft;
        if (style.paddingRight != 0) element.style.paddingRight = style.paddingRight;
        if (style.paddingTop != 0) element.style.paddingTop = style.paddingTop;
        if (style.paddingBottom != 0) element.style.paddingBottom = style.paddingBottom;

        if (style.marginLeft != 0) element.style.marginLeft = style.marginLeft;
        if (style.marginRight != 0) element.style.marginRight = style.marginRight;
        if (style.marginTop != 0) element.style.marginTop = style.marginTop;
        if (style.marginBottom != 0) element.style.marginBottom = style.marginBottom;

        if (TryColor(style.borderColor, out Color border))
        {
            element.style.borderLeftColor = border;
            element.style.borderRightColor = border;
            element.style.borderTopColor = border;
            element.style.borderBottomColor = border;
        }

        if (style.borderWidth > 0)
        {
            element.style.borderLeftWidth = style.borderWidth;
            element.style.borderRightWidth = style.borderWidth;
            element.style.borderTopWidth = style.borderWidth;
            element.style.borderBottomWidth = style.borderWidth;
        }

        if (style.borderRadius > 0)
        {
            element.style.borderTopLeftRadius = style.borderRadius;
            element.style.borderTopRightRadius = style.borderRadius;
            element.style.borderBottomLeftRadius = style.borderRadius;
            element.style.borderBottomRightRadius = style.borderRadius;
        }

        if (!string.IsNullOrWhiteSpace(style.layoutDirection))
        {
            element.style.flexDirection =
                style.layoutDirection.Equals("row", StringComparison.OrdinalIgnoreCase)
                    ? FlexDirection.Row
                    : FlexDirection.Column;
        }

        if (!string.IsNullOrWhiteSpace(style.alignItems))
            element.style.alignItems = ParseAlign(style.alignItems);

        if (!string.IsNullOrWhiteSpace(style.justifyContent))
            element.style.justifyContent = ParseJustify(style.justifyContent);

        if (style.flexGrow != 0)
            element.style.flexGrow = style.flexGrow;

        if (style.opacitySet)
            element.style.opacity = style.opacity;
    }

    private static void Reset(VisualElement element)
    {
        element.style.backgroundColor = StyleKeyword.Null;
        element.style.color = StyleKeyword.Null;
        element.style.fontSize = StyleKeyword.Null;
        element.style.unityFontStyleAndWeight = StyleKeyword.Null;
        element.style.unityTextAlign = StyleKeyword.Null;

        element.style.paddingLeft = StyleKeyword.Null;
        element.style.paddingRight = StyleKeyword.Null;
        element.style.paddingTop = StyleKeyword.Null;
        element.style.paddingBottom = StyleKeyword.Null;

        element.style.marginLeft = StyleKeyword.Null;
        element.style.marginRight = StyleKeyword.Null;
        element.style.marginTop = StyleKeyword.Null;
        element.style.marginBottom = StyleKeyword.Null;

        element.style.borderLeftColor = StyleKeyword.Null;
        element.style.borderRightColor = StyleKeyword.Null;
        element.style.borderTopColor = StyleKeyword.Null;
        element.style.borderBottomColor = StyleKeyword.Null;

        element.style.borderLeftWidth = StyleKeyword.Null;
        element.style.borderRightWidth = StyleKeyword.Null;
        element.style.borderTopWidth = StyleKeyword.Null;
        element.style.borderBottomWidth = StyleKeyword.Null;

        element.style.borderTopLeftRadius = StyleKeyword.Null;
        element.style.borderTopRightRadius = StyleKeyword.Null;
        element.style.borderBottomLeftRadius = StyleKeyword.Null;
        element.style.borderBottomRightRadius = StyleKeyword.Null;

        element.style.flexDirection = StyleKeyword.Null;
        element.style.alignItems = StyleKeyword.Null;
        element.style.justifyContent = StyleKeyword.Null;

        element.style.opacity = StyleKeyword.Null;
    }

    private static bool TryColor(string value, out Color color)
    {
        color = default;
        return !string.IsNullOrWhiteSpace(value) &&
               ColorUtility.TryParseHtmlString(value.Trim(), out color);
    }

    private static Align ParseAlign(string value)
    {
        switch (value.Trim().ToLowerInvariant())
        {
            case "center": return Align.Center;
            case "flex-end": return Align.FlexEnd;
            case "stretch": return Align.Stretch;
            default: return Align.FlexStart;
        }
    }

    private static Justify ParseJustify(string value)
    {
        switch (value.Trim().ToLowerInvariant())
        {
            case "center": return Justify.Center;
            case "flex-end": return Justify.FlexEnd;
            case "space-between": return Justify.SpaceBetween;
            default: return Justify.FlexStart;
        }
    }

    private static TextAnchor ParseTextAnchor(string value)
    {
        switch (value.Trim().ToLowerInvariant())
        {
            case "upper-center": return TextAnchor.UpperCenter;
            case "upper-right": return TextAnchor.UpperRight;
            case "middle-left": return TextAnchor.MiddleLeft;
            case "middle-center": return TextAnchor.MiddleCenter;
            case "middle-right": return TextAnchor.MiddleRight;
            case "lower-left": return TextAnchor.LowerLeft;
            case "lower-center": return TextAnchor.LowerCenter;
            case "lower-right": return TextAnchor.LowerRight;
            default: return TextAnchor.UpperLeft;
        }
    }
}
