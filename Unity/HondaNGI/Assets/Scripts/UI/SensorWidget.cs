using UnityEngine;
using UnityEngine.UIElements;

public sealed class SensorWidget : IRuntimeWidget
{
    private readonly SensorDefinition sensor;
    private readonly AddonStyleRuntime styles;
    private readonly VisualElement root;
    private readonly Label valueLabel;
    private readonly Label unitLabel;
    private readonly VisualElement selectionDash;
    private readonly Color sensorColor;

    public VisualElement Root => root;
    public string SensorId => sensor.Id;

    public SensorWidget(
        SensorDefinition sensor,
        string traceColor,
        AddonStyleRuntime styles = null)
    {
        this.sensor = sensor;
        this.styles = styles;

        sensorColor = new Color(0.95f, 0.12f, 0.12f);

        if (!string.IsNullOrWhiteSpace(traceColor) &&
            ColorUtility.TryParseHtmlString(traceColor, out Color parsed))
        {
            sensorColor = parsed;
        }

        root = new VisualElement();
        root.AddToClassList("sensor-widget");
        root.style.flexDirection = FlexDirection.Column;

        var header = new VisualElement();
        header.AddToClassList("sensor-widget-header");

        var nameLabel = new Label(sensor.ShortName);
        nameLabel.AddToClassList("sensor-widget-name");

        selectionDash = new VisualElement();
        selectionDash.AddToClassList("sensor-widget-selection-dash");
        selectionDash.style.backgroundColor = sensorColor;
        selectionDash.style.width = 28;
        selectionDash.style.height = 4;

        header.Add(nameLabel);
        header.Add(selectionDash);

        var valueArea = new VisualElement();
        valueArea.AddToClassList("sensor-widget-value-area");
        valueArea.style.flexGrow = 1;

        valueLabel = new Label("--");
        valueLabel.AddToClassList("sensor-widget-value");

        unitLabel = new Label(sensor.Unit);
        unitLabel.AddToClassList("sensor-widget-unit");

        valueArea.Add(valueLabel);
        valueArea.Add(unitLabel);

        root.Add(header);
        root.Add(valueArea);

        root.RegisterCallback<ClickEvent>(_ => TraceSelection.Toggle(sensor.Id));

        TraceSelection.Changed += UpdateSelectionAppearance;
        UpdateSelectionAppearance();
    }

    public void Refresh()
    {
        if (sensor.Kind == SensorKind.Boolean)
        {
            valueLabel.text = sensor.Value >= 0.5f ? "ON" : "OFF";
            unitLabel.text = "";
        }
        else
        {
            valueLabel.text = sensor.Value.ToString("F" + sensor.Decimals);
            unitLabel.text = sensor.Unit;
        }
    }

    private void UpdateSelectionAppearance()
    {
        bool selected = TraceSelection.IsSelected(sensor.Id);

        styles?.SetState(root, "selected", selected);
        styles?.SetState(selectionDash, "selected", selected);
    }
}
