using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class ExternalStyleLoader
{
    private readonly Dictionary<string, StyleRuleDefinition> rules =
        new Dictionary<string, StyleRuleDefinition>(StringComparer.OrdinalIgnoreCase);

    public bool Load(string path)
    {
        rules.Clear();

        if (string.IsNullOrWhiteSpace(path))
            return true;

        if (!File.Exists(path))
        {
            Debug.LogError("External style file not found: " + path);
            return false;
        }

        ExternalStyleSheetDefinition sheet =
            JsonUtility.FromJson<ExternalStyleSheetDefinition>(File.ReadAllText(path));

        if (sheet == null || sheet.rules == null)
        {
            Debug.LogError("Could not deserialize external style file: " + path);
            return false;
        }

        foreach (StyleRuleDefinition rule in sheet.rules)
        {
            if (rule != null && !string.IsNullOrWhiteSpace(rule.selector))
                rules[rule.selector] = rule;
        }

        return true;
    }

    public void ApplyRecursive(VisualElement root)
    {
        if (root == null) return;

        ApplyToElement(root);

        foreach (VisualElement child in root.Children())
            ApplyRecursive(child);
    }

    public void RefreshElement(VisualElement element)
    {
        if (element == null) return;

        ResetSupportedStyles(element);
        ApplyToElement(element);
    }

    public void ApplyToElement(VisualElement element)
    {
        if (element == null) return;

        foreach (string className in element.GetClasses())
        {
            if (rules.TryGetValue(className, out StyleRuleDefinition rule))
                ApplyRule(element, rule);
        }
    }

    private static void ResetSupportedStyles(VisualElement element)
    {
        element.style.backgroundColor = StyleKeyword.Null;
        element.style.color = StyleKeyword.Null;
        element.style.fontSize = StyleKeyword.Null;
        element.style.unityFontStyleAndWeight = StyleKeyword.Null;

        element.style.paddingLeft = StyleKeyword.Null;
        element.style.paddingRight = StyleKeyword.Null;
        element.style.paddingTop = StyleKeyword.Null;
        element.style.paddingBottom = StyleKeyword.Null;

        element.style.marginLeft = StyleKeyword.Null;
        element.style.marginRight = StyleKeyword.Null;
        element.style.marginTop = StyleKeyword.Null;
        element.style.marginBottom = StyleKeyword.Null;

        element.style.borderTopLeftRadius = StyleKeyword.Null;
        element.style.borderTopRightRadius = StyleKeyword.Null;
        element.style.borderBottomLeftRadius = StyleKeyword.Null;
        element.style.borderBottomRightRadius = StyleKeyword.Null;

        element.style.borderLeftWidth = StyleKeyword.Null;
        element.style.borderRightWidth = StyleKeyword.Null;
        element.style.borderTopWidth = StyleKeyword.Null;
        element.style.borderBottomWidth = StyleKeyword.Null;

        element.style.borderLeftColor = StyleKeyword.Null;
        element.style.borderRightColor = StyleKeyword.Null;
        element.style.borderTopColor = StyleKeyword.Null;
        element.style.borderBottomColor = StyleKeyword.Null;

        element.style.flexDirection = StyleKeyword.Null;
        element.style.alignItems = StyleKeyword.Null;
        element.style.justifyContent = StyleKeyword.Null;
        element.style.unityTextAlign = StyleKeyword.Null;
    }

    private static void ApplyRule(VisualElement element, StyleRuleDefinition rule)
    {
        if (TryParseColor(rule.backgroundColor, out Color background))
            element.style.backgroundColor = background;

        if (TryParseColor(rule.color, out Color foreground))
            element.style.color = foreground;

        if (rule.fontSize > 0)
            element.style.fontSize = rule.fontSize;

        if (rule.bold)
            element.style.unityFontStyleAndWeight = FontStyle.Bold;

        if (rule.paddingLeft != 0) element.style.paddingLeft = rule.paddingLeft;
        if (rule.paddingRight != 0) element.style.paddingRight = rule.paddingRight;
        if (rule.paddingTop != 0) element.style.paddingTop = rule.paddingTop;
        if (rule.paddingBottom != 0) element.style.paddingBottom = rule.paddingBottom;

        if (rule.marginLeft != 0) element.style.marginLeft = rule.marginLeft;
        if (rule.marginRight != 0) element.style.marginRight = rule.marginRight;
        if (rule.marginTop != 0) element.style.marginTop = rule.marginTop;
        if (rule.marginBottom != 0) element.style.marginBottom = rule.marginBottom;

        if (rule.borderRadius > 0)
        {
            element.style.borderTopLeftRadius = rule.borderRadius;
            element.style.borderTopRightRadius = rule.borderRadius;
            element.style.borderBottomLeftRadius = rule.borderRadius;
            element.style.borderBottomRightRadius = rule.borderRadius;
        }

        if (rule.borderWidth > 0)
        {
            element.style.borderLeftWidth = rule.borderWidth;
            element.style.borderRightWidth = rule.borderWidth;
            element.style.borderTopWidth = rule.borderWidth;
            element.style.borderBottomWidth = rule.borderWidth;
        }

        if (TryParseColor(rule.borderColor, out Color border))
        {
            element.style.borderLeftColor = border;
            element.style.borderRightColor = border;
            element.style.borderTopColor = border;
            element.style.borderBottomColor = border;
        }

        if (!string.IsNullOrWhiteSpace(rule.flexDirection))
            element.style.flexDirection =
                rule.flexDirection.Equals("row", StringComparison.OrdinalIgnoreCase)
                ? FlexDirection.Row : FlexDirection.Column;

        if (!string.IsNullOrWhiteSpace(rule.alignItems))
            element.style.alignItems = ParseAlign(rule.alignItems);

        if (!string.IsNullOrWhiteSpace(rule.justifyContent))
            element.style.justifyContent = ParseJustify(rule.justifyContent);

        if (!string.IsNullOrWhiteSpace(rule.textAlign))
            element.style.unityTextAlign = ParseTextAnchor(rule.textAlign);
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

    private static bool TryParseColor(string value, out Color color)
    {
        color = default;
        return !string.IsNullOrWhiteSpace(value) &&
               ColorUtility.TryParseHtmlString(value.Trim(), out color);
    }
}
