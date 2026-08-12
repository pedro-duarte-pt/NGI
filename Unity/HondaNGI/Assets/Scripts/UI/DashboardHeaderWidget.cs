using UnityEngine.UIElements;

public sealed class DashboardHeaderWidget : IRuntimeWidget
{
    private readonly VisualElement root;
    public VisualElement Root => root;

    public DashboardHeaderWidget(WidgetContext context)
    {
        root = new VisualElement();
        root.AddToClassList("dashboard-header");

        var brand = new VisualElement();
        brand.AddToClassList("dashboard-brand");

        var imageDefinition = new WidgetDefinition
        {
            asset = "assets/honda-logo.png",
            scaleMode = "fit"
        };

        var logo = new ImageWidget(imageDefinition, context);
        logo.Root.AddToClassList("dashboard-logo");

        var titleBlock = new VisualElement();
        titleBlock.AddToClassList("dashboard-title-block");

        var title = new Label("SENSOR DIAGNOSTICS");
        title.AddToClassList("dashboard-title");

        var subtitle = new Label("HONDA DEL SOL VTi");
        subtitle.AddToClassList("dashboard-subtitle");

        titleBlock.Add(title);
        titleBlock.Add(subtitle);

        brand.Add(logo.Root);
        brand.Add(titleBlock);

        root.Add(brand);
    }

    public void Refresh() { }
}
