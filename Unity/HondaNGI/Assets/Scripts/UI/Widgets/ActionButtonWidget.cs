using UnityEngine;
using UnityEngine.UIElements;

/// <summary>Generic declarative navigation/back button.</summary>
public sealed class ActionButtonWidget : IRuntimeWidget
{
    public VisualElement Root { get; }

    public ActionButtonWidget(WidgetDefinition definition)
    {
        string text = definition.PropString("text", "ACTION");
        string action = definition.PropString("action");
        string screen = definition.PropString("screen");
        string addon = definition.PropString("addon");
        string fallbackScreen = definition.PropString("fallbackScreen");
        string fallbackAddon = definition.PropString("fallbackAddon");

        var button = new Button(() =>
        {
            if (action == "navigate")
                ScreenNavigation.Navigate(screen, addon);
            else if (action == "back")
                ScreenNavigation.Back(fallbackScreen, fallbackAddon);
            else
                Debug.LogWarning("Unsupported actionButton action: " + action);
        });

        button.text = text;
        button.AddToClassList("action-button");
        button.style.width = Length.Percent(100);
        button.style.height = Length.Percent(100);
        button.style.unityFontStyleAndWeight = FontStyle.Bold;
        Root = button;
    }

    public void Refresh() { }
}
