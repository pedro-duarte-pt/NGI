using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class SensorTraceWidget : IRuntimeWidget
{
    private sealed class Sample
    {
        public float Time;
        public float Value;
    }

    private sealed class TraceData
    {
        public SensorDefinition Sensor;
        public readonly List<Sample> Samples = new List<Sample>();
    }

    private readonly VisualElement root;
    private readonly VisualElement graph;
    private readonly VisualElement legendContainer;
    private readonly VisualElement windowButtons;
    private readonly VisualElement axisOverlay;
    private readonly AddonStyleRuntime styles;

    private readonly Dictionary<string, TraceData> traces =
        new Dictionary<string, TraceData>(StringComparer.OrdinalIgnoreCase);

    private readonly float[] allowedWindows = { 5f, 10f, 30f, 60f, 120f };
    private readonly List<Color> palette = new List<Color>();

    private float windowSeconds;
    private readonly int maxTraces;

    public VisualElement Root => root;

    public SensorTraceWidget(
        WidgetDefinition definition,
        AddonStyleRuntime styles = null)
    {
        this.styles = styles;

        windowSeconds = definition.PropFloat("windowSeconds") > 0f
            ? definition.PropFloat("windowSeconds")
            : 10f;

        maxTraces = definition.PropInt("maxTraces") > 0
            ? definition.PropInt("maxTraces")
            : 5;

        BuildPalette(definition.PropStringArray("traceColors"));

        TraceSelection.MaxTraces = maxTraces;
        TraceSelection.SetInitial(definition.PropStringArray("sensors"));

        root = new VisualElement();
        root.AddToClassList("sensor-trace-widget");

        legendContainer = new VisualElement();
        legendContainer.AddToClassList("sensor-trace-legend");

        graph = new VisualElement();
        graph.AddToClassList("sensor-trace-graph");
        graph.generateVisualContent += DrawGraph;

        // Text cannot reliably be created from generateVisualContent.
        // Keep axis labels as real UI Toolkit children over the graph.
        axisOverlay = new VisualElement();
        axisOverlay.name = "sensor-trace-axis-overlay";
        axisOverlay.style.position = Position.Absolute;
        axisOverlay.style.left = 0;
        axisOverlay.style.top = 0;
        axisOverlay.style.right = 0;
        axisOverlay.style.bottom = 0;
        axisOverlay.pickingMode = PickingMode.Ignore;
        graph.Add(axisOverlay);

        var bottom = new VisualElement();
        bottom.AddToClassList("sensor-trace-bottom-controls");

        windowButtons = new VisualElement();
        windowButtons.AddToClassList("sensor-trace-window-buttons");

        foreach (float seconds in allowedWindows)
        {
            float captured = seconds;

            var button = new Button(() => SetWindow(captured))
            {
                text = seconds.ToString("0") + " s"
            };

            button.name = "window-" + seconds.ToString("0");
            button.AddToClassList("sensor-trace-window-button");
            windowButtons.Add(button);
        }

        var reset = new Button(ResetTrace) { text = "RESET" };
        reset.AddToClassList("sensor-trace-window-button");
        reset.AddToClassList("sensor-trace-reset-button");

        bottom.Add(windowButtons);
        bottom.Add(reset);

        root.Add(legendContainer);
        root.Add(graph);
        root.Add(bottom);

        TraceSelection.Changed += SyncSelectedSensors;

        UpdateWindowButtons();
        SyncSelectedSensors();
        graph.MarkDirtyRepaint();
    }

    public void Refresh()
    {
        SyncSelectedSensors();

        float now = Time.unscaledTime;

        foreach (TraceData trace in traces.Values)
        {
            trace.Samples.Add(new Sample
            {
                Time = now,
                Value = trace.Sensor.Value
            });

            float oldest = now - windowSeconds;

            while (trace.Samples.Count > 0 &&
                   trace.Samples[0].Time < oldest)
            {
                trace.Samples.RemoveAt(0);
            }
        }

        UpdateLegend();
        UpdateAxisLabels();
        graph.MarkDirtyRepaint();
    }

    private void BuildPalette(string[] configuredColors)
    {
        if (configuredColors != null)
        {
            foreach (string html in configuredColors)
            {
                if (!string.IsNullOrWhiteSpace(html) &&
                    ColorUtility.TryParseHtmlString(html.Trim(), out Color color))
                {
                    palette.Add(color);
                }
            }
        }

        if (palette.Count == 0)
        {
            palette.Add(new Color(1f, 0.12f, 0.12f));
            palette.Add(new Color(0.55f, 0.20f, 0.95f));
            palette.Add(new Color(0.20f, 0.75f, 0.25f));
            palette.Add(new Color(0.95f, 0.55f, 0.05f));
            palette.Add(new Color(0.10f, 0.70f, 0.72f));
        }
    }

    private void SyncSelectedSensors()
    {
        var wanted = new HashSet<string>(
            TraceSelection.SelectedSensors,
            StringComparer.OrdinalIgnoreCase
        );

        var remove = new List<string>();

        foreach (string id in traces.Keys)
        {
            if (!wanted.Contains(id))
                remove.Add(id);
        }

        foreach (string id in remove)
            traces.Remove(id);

        foreach (string id in wanted)
        {
            if (traces.ContainsKey(id))
                continue;

            SensorDefinition sensor = SensorRegistry.Get(id);

            if (sensor != null)
            {
                traces.Add(id, new TraceData
                {
                    Sensor = sensor
                });
            }
        }

        UpdateLegend();
        UpdateAxisLabels();
        graph.MarkDirtyRepaint();
    }

    private void SetWindow(float seconds)
    {
        windowSeconds = seconds;
        UpdateWindowButtons();
        UpdateAxisLabels();
        graph.MarkDirtyRepaint();
    }

private void UpdateWindowButtons()
{
    foreach (VisualElement child in windowButtons.Children())
        styles?.SetState(child, "active", false);

    VisualElement active =
        windowButtons.Q<VisualElement>(
            "window-" + windowSeconds.ToString("0")
        );

    if (active != null)
        styles?.SetState(active, "active", true);
}

    private void ResetTrace()
    {
        foreach (TraceData trace in traces.Values)
            trace.Samples.Clear();

        graph.MarkDirtyRepaint();
    }

    private void UpdateLegend()
    {
        legendContainer.Clear();

        if (TraceSelection.SelectedSensors.Count == 0)
        {
            var empty = new Label("SELECT A SENSOR");
            empty.AddToClassList("sensor-trace-legend-empty");
            legendContainer.Add(empty);
            styles?.ApplyElement(empty);
            return;
        }

        int index = 0;

        foreach (string id in TraceSelection.SelectedSensors)
        {
            SensorDefinition sensor = SensorRegistry.Get(id);

            if (sensor == null)
                continue;

            var item = new VisualElement();
            item.AddToClassList("sensor-trace-legend-item");

            var dash = new VisualElement();
            dash.AddToClassList("sensor-trace-legend-dash");

            var text = new Label(
                sensor.ShortName +
                (string.IsNullOrWhiteSpace(sensor.Unit)
                    ? ""
                    : " (" + sensor.Unit + ")")
            );

            text.AddToClassList("sensor-trace-legend-text");

            item.Add(dash);
            item.Add(text);
            legendContainer.Add(item);

            styles?.ApplyElement(item);
            styles?.ApplyElement(dash);
            styles?.ApplyElement(text);

            dash.style.backgroundColor = palette[index % palette.Count];

            index++;
        }

        styles?.ApplyElement(legendContainer);
    }

    private void DrawGraph(MeshGenerationContext context)
    {
        Rect r = graph.contentRect;

        if (r.width <= 1f || r.height <= 1f)
            return;

        Painter2D painter = context.painter2D;

        // Space reserved for Y labels on the left and the shared X scale below.
        AddonStyleProperties plotStyle = Style("sensor-trace-plot");
        float yAxisWidth = Number(plotStyle.width, 48f);
        float xAxisHeight = Number(plotStyle.height, 24f);
        float topPadding = Number(plotStyle.paddingTop, 8f);
        float rightPadding = Number(plotStyle.paddingRight, 8f);

        float plotLeft = r.xMin + yAxisWidth;
        float plotRight = r.xMax - rightPadding;
        float plotTop = r.yMin + topPadding;
        float plotBottom = r.yMax - xAxisHeight;

        int selectedCount = TraceSelection.SelectedSensors.Count;

        if (selectedCount <= 0 ||
            plotRight <= plotLeft ||
            plotBottom <= plotTop)
        {
            return;
        }

        float laneHeight =
            (plotBottom - plotTop) / selectedCount;

        // Shared vertical time grid.
        const int xDivisions = 6;

        AddonStyleProperties timeGridStyle = Style("sensor-trace-time-grid");
        painter.lineWidth = Number(timeGridStyle.lineWidth, 1f);
        painter.strokeColor = StyleColor(
            timeGridStyle.color,
            new Color(0.12f, 0.14f, 0.16f, 1f));

        for (int i = 0; i <= xDivisions; i++)
        {
            float x01 = (float)i / xDivisions;
            float x = Mathf.Lerp(plotLeft, plotRight, x01);

            painter.BeginPath();
            painter.MoveTo(new Vector2(x, plotTop));
            painter.LineTo(new Vector2(x, plotBottom));
            painter.Stroke();
        }

        float now = Time.unscaledTime;
        int laneIndex = 0;

        foreach (string id in TraceSelection.SelectedSensors)
        {
            SensorDefinition sensor = SensorRegistry.Get(id);

            if (sensor == null)
                continue;

            float laneTop = plotTop + laneIndex * laneHeight;
            float laneBottom = laneTop + laneHeight;

            // A little breathing room inside each lane.
            AddonStyleProperties laneStyle = Style("sensor-trace-lane");
            float lanePadding = Number(laneStyle.paddingTop, 6f);
            float innerTop = laneTop + lanePadding;
            float innerBottom = laneBottom - lanePadding;

            Color traceColor =
                palette[laneIndex % palette.Count];

            DrawLaneGridAndScale(
                painter,
                sensor,
                traceColor,
                plotLeft,
                plotRight,
                laneTop,
                laneBottom,
                innerTop,
                innerBottom
            );

            if (traces.TryGetValue(id, out TraceData trace) &&
                trace.Samples.Count >= 2)
            {
                DrawTrace(
                    painter,
                    trace,
                    traceColor,
                    now,
                    plotLeft,
                    plotRight,
                    innerTop,
                    innerBottom
                );
            }

            laneIndex++;
        }

        DrawTimeAxis(
            context,
            painter,
            plotLeft,
            plotRight,
            plotBottom,
            xDivisions
        );
    }

    private void DrawLaneGridAndScale(
        Painter2D painter,
        SensorDefinition sensor,
        Color traceColor,
        float plotLeft,
        float plotRight,
        float laneTop,
        float laneBottom,
        float innerTop,
        float innerBottom)
    {
        const int yDivisions = 2;

        AddonStyleProperties separatorStyle = Style("sensor-trace-separator");
        painter.lineWidth = Number(separatorStyle.lineWidth, 1f);
        painter.strokeColor = StyleColor(
            separatorStyle.color,
            new Color(0.18f, 0.20f, 0.22f, 1f));

        painter.BeginPath();
        painter.MoveTo(new Vector2(plotLeft, laneBottom));
        painter.LineTo(new Vector2(plotRight, laneBottom));
        painter.Stroke();

        AddonStyleProperties laneGridStyle = Style("sensor-trace-lane-grid");
        painter.lineWidth = Number(laneGridStyle.lineWidth, 1f);
        painter.strokeColor = StyleColor(
            laneGridStyle.color,
            new Color(0.11f, 0.13f, 0.15f, 1f));

        for (int i = 0; i <= yDivisions; i++)
        {
            float y01 = (float)i / yDivisions;
            float y = Mathf.Lerp(innerBottom, innerTop, y01);

            painter.BeginPath();
            painter.MoveTo(new Vector2(plotLeft, y));
            painter.LineTo(new Vector2(plotRight, y));
            painter.Stroke();
        }
    }

    private void DrawTrace(
        Painter2D painter,
        TraceData trace,
        Color traceColor,
        float now,
        float plotLeft,
        float plotRight,
        float innerTop,
        float innerBottom)
    {
        float min = trace.Sensor.Min;
        float max = trace.Sensor.Max;

        if (Mathf.Approximately(min, max))
            max = min + 1f;

        AddonStyleProperties traceStyle = Style("sensor-trace-line");
        painter.lineWidth = Number(traceStyle.lineWidth, 2f);
        painter.strokeColor = traceColor;
        painter.BeginPath();

        bool first = true;

        foreach (Sample sample in trace.Samples)
        {
            float age = now - sample.Time;

            float x01 =
                1f - Mathf.Clamp01(age / windowSeconds);

            float y01 =
                Mathf.InverseLerp(min, max, sample.Value);

            Vector2 p = new Vector2(
                Mathf.Lerp(plotLeft, plotRight, x01),
                Mathf.Lerp(innerBottom, innerTop, y01)
            );

            if (first)
            {
                painter.MoveTo(p);
                first = false;
            }
            else
            {
                painter.LineTo(p);
            }
        }

        painter.Stroke();
    }

    private void DrawTimeAxis(
        MeshGenerationContext context,
        Painter2D painter,
        float plotLeft,
        float plotRight,
        float plotBottom,
        int divisions)
    {
        AddonStyleProperties axisStyle = Style("sensor-trace-axis");
        painter.lineWidth = Number(axisStyle.lineWidth, 1f);
        painter.strokeColor = StyleColor(
            axisStyle.color,
            new Color(0.30f, 0.32f, 0.34f, 1f));

        painter.BeginPath();
        painter.MoveTo(new Vector2(plotLeft, plotBottom));
        painter.LineTo(new Vector2(plotRight, plotBottom));
        painter.Stroke();
    }

    private void UpdateAxisLabels()
    {
        if (axisOverlay == null)
            return;

        axisOverlay.Clear();

        Rect r = graph.contentRect;

        if (r.width <= 1f || r.height <= 1f)
            return;

        AddonStyleProperties plotStyle = Style("sensor-trace-plot");
        float yAxisWidth = Number(plotStyle.width, 48f);
        float xAxisHeight = Number(plotStyle.height, 24f);
        float topPadding = Number(plotStyle.paddingTop, 8f);
        float rightPadding = Number(plotStyle.paddingRight, 8f);
        const int yDivisions = 2;
        const int xDivisions = 6;

        float plotLeft = r.xMin + yAxisWidth;
        float plotRight = r.xMax - rightPadding;
        float plotTop = r.yMin + topPadding;
        float plotBottom = r.yMax - xAxisHeight;

        int selectedCount = TraceSelection.SelectedSensors.Count;

        if (selectedCount <= 0 ||
            plotRight <= plotLeft ||
            plotBottom <= plotTop)
        {
            return;
        }

        float laneHeight = (plotBottom - plotTop) / selectedCount;
        int laneIndex = 0;

        foreach (string id in TraceSelection.SelectedSensors)
        {
            SensorDefinition sensor = SensorRegistry.Get(id);

            if (sensor == null)
                continue;

            float laneTop = plotTop + laneIndex * laneHeight;
            float laneBottom = laneTop + laneHeight;
            AddonStyleProperties laneStyle = Style("sensor-trace-lane");
            float lanePadding = Number(laneStyle.paddingTop, 6f);
            float innerTop = laneTop + lanePadding;
            float innerBottom = laneBottom - lanePadding;

            Color traceColor = palette[laneIndex % palette.Count];

            for (int i = 0; i <= yDivisions; i++)
            {
                float value01 = (float)i / yDivisions;
                float value = Mathf.Lerp(sensor.Min, sensor.Max, value01);
                float y = Mathf.Lerp(innerBottom, innerTop, value01);

                var label = new Label(FormatAxisValue(sensor, value));
                label.pickingMode = PickingMode.Ignore;
                label.style.position = Position.Absolute;
                AddonStyleProperties yLabelStyle = Style("sensor-trace-y-label");
                label.style.left = Number(yLabelStyle.marginLeft, 2f);
                label.style.top = y - Number(yLabelStyle.marginTop, 9f);
                label.style.width = Number(yLabelStyle.width, 40f);
                label.style.height = Number(yLabelStyle.height, 18f);
                label.style.fontSize = Number(yLabelStyle.fontSize, 15f);
                label.style.unityTextAlign = TextAnchor.MiddleRight;
                label.style.color = traceColor;

                axisOverlay.Add(label);
            }

            laneIndex++;
        }

        for (int i = 0; i <= xDivisions; i++)
        {
            float x01 = (float)i / xDivisions;
            float x = Mathf.Lerp(plotLeft, plotRight, x01);
            float secondsAgo = windowSeconds * (1f - x01);

            string labelText =
                i == xDivisions
                    ? "NOW"
                    : "-" + FormatSeconds(secondsAgo) + "s";

            var label = new Label(labelText);
            label.pickingMode = PickingMode.Ignore;
            label.style.position = Position.Absolute;
            AddonStyleProperties xLabelStyle = Style("sensor-trace-x-label");
            float xLabelWidth = Number(xLabelStyle.width, 52f);
            label.style.left = x - (xLabelWidth * 0.5f);
            label.style.top = plotBottom + Number(xLabelStyle.marginTop, 2f);
            label.style.width = xLabelWidth;
            label.style.height = Number(xLabelStyle.height, 20f);
            label.style.fontSize = Number(xLabelStyle.fontSize, 15f);
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.color = StyleColor(
                xLabelStyle.color,
                new Color(0.90f, 0.91f, 0.92f, 1f));

            axisOverlay.Add(label);
        }
    }


    private AddonStyleProperties Style(string name)
    {
        return styles != null
            ? styles.Resolve(name)
            : new AddonStyleProperties();
    }

    private static float Number(OptionalFloat value, float fallback)
    {
        return value != null && value.HasValue ? value.value : fallback;
    }

    private static Color StyleColor(string value, Color fallback)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               ColorUtility.TryParseHtmlString(value, out Color parsed)
            ? parsed
            : fallback;
    }

    private static string FormatAxisValue(
        SensorDefinition sensor,
        float value)
    {
        if (sensor.Kind == SensorKind.Boolean)
            return value >= 0.5f ? "1" : "0";

        // Avoid excessive decimals on the compact Y scale.
        int decimals = Mathf.Clamp(sensor.Decimals, 0, 1);
        return value.ToString("F" + decimals);
    }

    private static string FormatSeconds(float seconds)
    {
        if (seconds >= 10f)
            return Mathf.RoundToInt(seconds).ToString();

        return seconds.ToString("0.#");
    }
}
