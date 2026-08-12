using UnityEngine.UIElements;

public sealed class PanelWidget : IRuntimeWidget
{
    private readonly VisualElement root;
    public VisualElement Root => root;

    public PanelWidget(string title, string subtitle)
    {
        root = new VisualElement();
        root.AddToClassList("panel-widget");

        var titleLabel = new Label(title ?? "");
        titleLabel.AddToClassList("panel-widget-title");
        root.Add(titleLabel);

        if (!string.IsNullOrWhiteSpace(subtitle))
        {
            var subtitleLabel = new Label(subtitle);
            subtitleLabel.AddToClassList("panel-widget-subtitle");
            root.Add(subtitleLabel);
        }
    }

    public void Refresh() { }
}
