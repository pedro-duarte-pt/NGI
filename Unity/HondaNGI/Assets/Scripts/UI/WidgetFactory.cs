using UnityEngine;

public static class WidgetFactory
{
    public static IRuntimeWidget Create(
        WidgetDefinition definition,
        WidgetContext context)
    {
        if (definition == null)
            return null;

        switch (definition.type)
        {
            case "sensor":
            {
                SensorDefinition sensor = SensorRegistry.Get(definition.sensor);

                if (sensor == null)
                {
                    Debug.LogError("Unknown sensor: " + definition.sensor);
                    return null;
                }

                return new SensorWidget(
                    sensor,
                    definition.traceColor,
                    context?.RefreshStyle
                );
            }

            case "text":
                return new TextWidget(definition.text);

            case "panel":
                return new PanelWidget(definition.text, definition.subtitle);

            case "sensorTrace":
                return new SensorTraceWidget(
                    definition,
                    context?.RefreshStyle
                );

            case "image":
                return new ImageWidget(definition, context);

            case "dashboardHeader":
                return new DashboardHeaderWidget(context);

            default:
                Debug.LogWarning("Unsupported widget type: " + definition.type);
                return null;
        }
    }
}
