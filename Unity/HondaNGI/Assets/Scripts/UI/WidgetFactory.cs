using UnityEngine;
public static class WidgetFactory {
 public static IRuntimeWidget Create(WidgetDefinition d,WidgetContext c){if(d==null)return null;switch(d.Type){
 case"container":return new ContainerWidget();
 case"sensor":{string id=d.PropString("sensor");var s=SensorRegistry.Get(id);if(s==null){Debug.LogError("Unknown sensor: "+id);return null;}return new SensorWidget(s,d.PropString("traceColor"),c?.Styles);}
 case"text":return new TextWidget(d.PropString("text"));
 case"sensorTrace":return new SensorTraceWidget(d,c?.Styles);
 case"conditionalSensor":return new ConditionalSensorWidget(d,c?.Styles);
 case"conditionalImage":return new ConditionalImageWidget(d,c);
 case"image":return new ImageWidget(d,c);
 default:Debug.LogWarning("Unsupported widget type: "+d.Type);return null;}}}
