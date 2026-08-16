using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Generic declarative selector bound to one protected PreferenceRegistry entry.
/// </summary>
public sealed class PreferenceSelectorWidget : IRuntimeWidget
{
    public VisualElement Root { get; }
    private readonly PreferenceRegistry.Definition preference;
    private readonly DropdownField dropdown;

    public PreferenceSelectorWidget(WidgetDefinition definition)
    {
        string preferenceId = definition.PropString("preference");
        if (!PreferenceRegistry.TryGet(preferenceId, out preference))
        {
            Debug.LogError("Unknown preference: " + preferenceId);
            Root = new Label("Unknown preference: " + preferenceId);
            return;
        }

        var root = new VisualElement();
        root.AddToClassList("preference-selector");
        root.style.flexDirection = FlexDirection.Column;
        root.style.justifyContent = Justify.Center;
        root.style.paddingLeft = 12;
        root.style.paddingRight = 12;

        var label = new Label(preference.Label);
        label.AddToClassList("preference-label");
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.marginBottom = 6;
        root.Add(label);

        var choices = new List<string>(preference.Options);
        int index = choices.IndexOf(preference.GetValue());
        if (index < 0) index = 0;

        dropdown = new DropdownField(choices, index);
        dropdown.AddToClassList("preference-dropdown");
        dropdown.RegisterValueChangedCallback(evt =>
        {
            if (!preference.TrySetValue(evt.newValue))
                Refresh();
        });
        root.Add(dropdown);
        Root = root;
    }

    public void Refresh()
    {
        if (preference == null || dropdown == null)
            return;

        string value = preference.GetValue();
        if (dropdown.value != value)
            dropdown.SetValueWithoutNotify(value);
    }
}
