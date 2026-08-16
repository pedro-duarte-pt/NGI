using UnityEngine.UIElements;

public sealed class TextWidget : IRuntimeWidget
{
    private readonly VisualElement root;
    public VisualElement Root => root;

    public TextWidget(string text)
    {
        root = new VisualElement();
        root.AddToClassList("text-widget");

        var label = new Label(text ?? "");
        label.AddToClassList("text-widget-label");
        root.Add(label);
    }

    public void Refresh() { }
}
