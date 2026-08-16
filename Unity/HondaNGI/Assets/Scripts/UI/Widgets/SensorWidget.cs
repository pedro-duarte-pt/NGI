using UnityEngine;
using UnityEngine.UIElements;
public sealed class SensorWidget : IRuntimeWidget
{
    readonly SensorDefinition sensor; readonly AddonStyleRuntime styles; readonly VisualElement root; readonly Label valueLabel,unitLabel; readonly VisualElement selectionDash;
    public VisualElement Root=>root; public string SensorId=>sensor.Id;
    public SensorWidget(SensorDefinition sensor,string traceColor,AddonStyleRuntime styles=null) {
        this.sensor=sensor;this.styles=styles;
        root=new VisualElement();root.AddToClassList("sensor-widget");
        var header=new VisualElement();header.AddToClassList("sensor-widget-header");
        var name=new Label(sensor.ShortName);name.AddToClassList("sensor-widget-name");
        selectionDash=new VisualElement();selectionDash.AddToClassList("sensor-widget-selection-dash");
        if(!string.IsNullOrWhiteSpace(traceColor)&&ColorUtility.TryParseHtmlString(traceColor,out Color c)) selectionDash.style.backgroundColor=c;
        header.Add(name);header.Add(selectionDash);
        var area=new VisualElement();area.AddToClassList("sensor-widget-value-area");
        valueLabel=new Label("--");valueLabel.AddToClassList("sensor-widget-value");
        unitLabel=new Label(UnitPresentation.Unit(sensor));unitLabel.AddToClassList("sensor-widget-unit");
        area.Add(valueLabel);area.Add(unitLabel);root.Add(header);root.Add(area);
        root.RegisterCallback<ClickEvent>(_=>TraceSelection.Toggle(sensor.Id));
        TraceSelection.Changed+=UpdateSelectionAppearance;UpdateSelectionAppearance();
    }
    public void Refresh(){
        if(sensor.Kind==SensorKind.Boolean){valueLabel.text=sensor.Value>=.5f?"ON":"OFF";unitLabel.text="";}
        else {valueLabel.text=UnitPresentation.Value(sensor,sensor.Value).ToString("F"+sensor.Decimals);unitLabel.text=UnitPresentation.Unit(sensor);}
    }
    void UpdateSelectionAppearance(){bool selected=TraceSelection.IsSelected(sensor.Id);styles?.SetState(root,"selected",selected);styles?.SetState(selectionDash,"selected",selected);}
}
