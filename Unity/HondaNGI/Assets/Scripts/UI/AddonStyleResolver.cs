using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AddonStyleResolver
{
    private readonly Dictionary<string, AddonStyleRuleDefinition> rules =
        new Dictionary<string, AddonStyleRuleDefinition>(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, string> tokens =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public AddonStyleResolver(AddonStyleSheetDefinition sheet)
    {
        if (sheet == null)
            return;

        if (sheet.tokens != null)
        {
            foreach (AddonStyleToken token in sheet.tokens)
            {
                if (token != null && !string.IsNullOrWhiteSpace(token.name))
                    tokens[token.name] = token.value;
            }
        }

        if (sheet.styles != null)
        {
            foreach (AddonStyleRuleDefinition rule in sheet.styles)
            {
                if (rule != null && !string.IsNullOrWhiteSpace(rule.name))
                    rules[rule.name] = rule;
            }
        }
    }

    public AddonStyleProperties Resolve(
        IEnumerable<string> styleNames,
        IEnumerable<string> activeStates)
    {
        var result = new AddonStyleProperties();

        if (styleNames == null)
            return result;

        var states = activeStates == null
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(activeStates, StringComparer.OrdinalIgnoreCase);

        foreach (string styleName in styleNames)
        {
            if (!rules.TryGetValue(styleName, out AddonStyleRuleDefinition rule))
                continue;

            Merge(result, rule.style);

            if (rule.states == null)
                continue;

            foreach (AddonStyleStateDefinition state in rule.states)
            {
                if (state != null &&
                    !string.IsNullOrWhiteSpace(state.name) &&
                    states.Contains(state.name))
                {
                    Merge(result, state.style);
                }
            }
        }

        ResolveTokens(result);
        return result;
    }

    private void ResolveTokens(AddonStyleProperties style)
    {
        style.backgroundColor = ResolveToken(style.backgroundColor);
        style.color = ResolveToken(style.color);
        style.borderColor = ResolveToken(style.borderColor);
    }

    private string ResolveToken(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value[0] != '$')
            return value;

        string key = value.Substring(1);
        return tokens.TryGetValue(key, out string resolved) ? resolved : value;
    }

    private static void Merge(AddonStyleProperties target, AddonStyleProperties source)
    {
        if (source == null)
            return;

        if (!string.IsNullOrWhiteSpace(source.backgroundColor)) target.backgroundColor = source.backgroundColor;
        if (!string.IsNullOrWhiteSpace(source.color)) target.color = source.color;
        if (source.fontSize != 0) target.fontSize = source.fontSize;
        if (!string.IsNullOrWhiteSpace(source.fontWeight)) target.fontWeight = source.fontWeight;
        if (!string.IsNullOrWhiteSpace(source.textAlign)) target.textAlign = source.textAlign;

        if (source.width != 0) target.width = source.width;
        if (source.height != 0) target.height = source.height;
        if (source.minWidth != 0) target.minWidth = source.minWidth;
        if (source.minHeight != 0) target.minHeight = source.minHeight;

        if (source.paddingLeft != 0) target.paddingLeft = source.paddingLeft;
        if (source.paddingRight != 0) target.paddingRight = source.paddingRight;
        if (source.paddingTop != 0) target.paddingTop = source.paddingTop;
        if (source.paddingBottom != 0) target.paddingBottom = source.paddingBottom;

        if (source.marginLeft != 0) target.marginLeft = source.marginLeft;
        if (source.marginRight != 0) target.marginRight = source.marginRight;
        if (source.marginTop != 0) target.marginTop = source.marginTop;
        if (source.marginBottom != 0) target.marginBottom = source.marginBottom;

        if (!string.IsNullOrWhiteSpace(source.borderColor)) target.borderColor = source.borderColor;
        if (source.borderWidth != 0) target.borderWidth = source.borderWidth;
        if (source.borderRadius != 0) target.borderRadius = source.borderRadius;

        if (!string.IsNullOrWhiteSpace(source.layoutDirection)) target.layoutDirection = source.layoutDirection;
        if (!string.IsNullOrWhiteSpace(source.alignItems)) target.alignItems = source.alignItems;
        if (!string.IsNullOrWhiteSpace(source.justifyContent)) target.justifyContent = source.justifyContent;
        if (source.flexGrow != 0) target.flexGrow = source.flexGrow;

        if (source.opacitySet)
        {
            target.opacitySet = true;
            target.opacity = source.opacity;
        }
    }
}
