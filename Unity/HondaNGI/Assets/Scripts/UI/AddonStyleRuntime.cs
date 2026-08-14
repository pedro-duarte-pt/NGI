using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class AddonStyleRuntime
{
    private AddonStyleResolver resolver;

    private readonly Dictionary<VisualElement, HashSet<string>> states =
        new Dictionary<VisualElement, HashSet<string>>();

    public bool Load(string path)
    {
        states.Clear();

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Debug.LogError("Addon style file not found: " + path);
            return false;
        }

        AddonStyleSheetDefinition sheet =
            JsonUtility.FromJson<AddonStyleSheetDefinition>(File.ReadAllText(path));

        if (sheet == null || sheet.styles == null)
        {
            Debug.LogError("Could not deserialize addon style file: " + path);
            return false;
        }

        resolver = new AddonStyleResolver(sheet);
        return true;
    }

    public AddonStyleProperties Resolve(params string[] styleNames)
    {
        if (resolver == null)
            return new AddonStyleProperties();

        return resolver.Resolve(styleNames, null);
    }

    public AddonStyleProperties Resolve(
        IEnumerable<string> styleNames,
        IEnumerable<string> activeStates)
    {
        if (resolver == null)
            return new AddonStyleProperties();

        return resolver.Resolve(styleNames, activeStates);
    }

    public void ApplyRecursive(VisualElement root)
    {
        if (root == null) return;
        ApplyElement(root);
        foreach (VisualElement child in root.Children())
            ApplyRecursive(child);
    }

    public void ApplyElement(VisualElement element)
    {
        if (element == null || resolver == null) return;

        states.TryGetValue(element, out HashSet<string> activeStates);
        AddonStyleApplier.Apply(
            element,
            resolver.Resolve(element.GetClasses(), activeStates));
    }

    public void SetState(VisualElement element, string state, bool enabled)
    {
        if (element == null || string.IsNullOrWhiteSpace(state)) return;

        if (!states.TryGetValue(element, out HashSet<string> elementStates))
        {
            elementStates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            states[element] = elementStates;
        }

        if (enabled) elementStates.Add(state);
        else elementStates.Remove(state);

        ApplyElement(element);
    }
}
