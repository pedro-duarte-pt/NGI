using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Generic declarative value widget. Cases are evaluated in order; the first
/// matching case selects either a sensor or a static message. If none match,
/// the default result is used.
/// </summary>
public sealed class ConditionalSensorWidget : IRuntimeWidget
{
    private sealed class Condition
    {
        public SensorDefinition Sensor;
        public string Operator;
        public float Value;
    }

    private sealed class Result
    {
        public SensorDefinition Sensor;
        public string Message;
        public string Unit;
    }

    private sealed class Case
    {
        public Condition When;
        public Result Result;
    }

    private readonly List<Case> cases = new List<Case>();
    private readonly Result defaultResult;
    private readonly string fixedLabel;
    private readonly AddonStyleRuntime styles;
    private readonly VisualElement root;
    private readonly Label nameLabel;
    private readonly Label valueLabel;
    private readonly Label unitLabel;
    private readonly VisualElement selectionDash;
    private SensorDefinition activeSensor;

    public VisualElement Root => root;

    public ConditionalSensorWidget(WidgetDefinition definition, AddonStyleRuntime styles = null)
    {
        this.styles = styles;
        fixedLabel = definition.PropString("label");

        if (definition.Props != null && definition.Props.TryGetValue("cases", out object rawCases))
        {
            List<object> array = AddonJson.Arr(rawCases);
            if (array != null)
            {
                foreach (object rawCase in array)
                {
                    Dictionary<string, object> caseObj = AddonJson.Obj(rawCase);
                    Case parsed = ParseCase(caseObj);
                    if (parsed != null)
                        cases.Add(parsed);
                }
            }
        }

        defaultResult = definition.Props != null && definition.Props.TryGetValue("default", out object rawDefault)
            ? ParseResult(AddonJson.Obj(rawDefault))
            : null;

        root = new VisualElement();
        root.AddToClassList("sensor-widget");

        var header = new VisualElement();
        header.AddToClassList("sensor-widget-header");

        nameLabel = new Label(string.IsNullOrWhiteSpace(fixedLabel) ? "VALUE" : fixedLabel);
        nameLabel.AddToClassList("sensor-widget-name");

        selectionDash = new VisualElement();
        selectionDash.AddToClassList("sensor-widget-selection-dash");
        string traceColor = definition.PropString("traceColor");
        if (!string.IsNullOrWhiteSpace(traceColor) && ColorUtility.TryParseHtmlString(traceColor, out Color color))
            selectionDash.style.backgroundColor = color;

        header.Add(nameLabel);
        header.Add(selectionDash);

        var area = new VisualElement();
        area.AddToClassList("sensor-widget-value-area");
        valueLabel = new Label("--");
        valueLabel.AddToClassList("sensor-widget-value");
        unitLabel = new Label("");
        unitLabel.AddToClassList("sensor-widget-unit");
        area.Add(valueLabel);
        area.Add(unitLabel);

        root.Add(header);
        root.Add(area);

        root.RegisterCallback<ClickEvent>(_ =>
        {
            if (activeSensor != null)
                TraceSelection.Toggle(activeSensor.Id);
        });

        TraceSelection.Changed += UpdateSelectionAppearance;
        Refresh();
    }

    public void Refresh()
    {
        Result result = SelectResult();
        activeSensor = result?.Sensor;

        if (string.IsNullOrWhiteSpace(fixedLabel))
            nameLabel.text = activeSensor != null ? activeSensor.ShortName : "VALUE";

        if (activeSensor != null)
        {
            if (activeSensor.Kind == SensorKind.Boolean)
            {
                valueLabel.text = activeSensor.Value >= 0.5f ? "ON" : "OFF";
                unitLabel.text = "";
            }
            else
            {
                valueLabel.text = activeSensor.Value.ToString("F" + activeSensor.Decimals, CultureInfo.InvariantCulture);
                unitLabel.text = activeSensor.Unit;
            }
        }
        else if (result != null)
        {
            valueLabel.text = result.Message ?? "--";
            unitLabel.text = result.Unit ?? "";
        }
        else
        {
            valueLabel.text = "--";
            unitLabel.text = "";
        }

        UpdateSelectionAppearance();
    }

    private Result SelectResult()
    {
        foreach (Case item in cases)
        {
            if (Matches(item.When))
                return item.Result;
        }
        return defaultResult;
    }

    private static bool Matches(Condition condition)
    {
        if (condition == null || condition.Sensor == null)
            return false;

        float actual = condition.Sensor.Value;
        float expected = condition.Value;

        switch (condition.Operator)
        {
            case "<": return actual < expected;
            case "<=": return actual <= expected;
            case ">": return actual > expected;
            case ">=": return actual >= expected;
            case "==": return Mathf.Approximately(actual, expected);
            case "!=": return !Mathf.Approximately(actual, expected);
            default: return false;
        }
    }

    private static Case ParseCase(Dictionary<string, object> obj)
    {
        if (obj == null || !obj.TryGetValue("when", out object rawWhen))
            return null;

        Condition condition = ParseCondition(AddonJson.Obj(rawWhen));
        Result result = ParseResult(obj);
        return condition == null || result == null ? null : new Case { When = condition, Result = result };
    }

    private static Condition ParseCondition(Dictionary<string, object> obj)
    {
        if (obj == null)
            return null;

        SensorDefinition sensor = SensorRegistry.Get(AddonJson.Str(obj, "sensor"));
        if (sensor == null)
            return null;

        float value;
        try
        {
            value = obj.TryGetValue("value", out object raw)
                ? Convert.ToSingle(raw, CultureInfo.InvariantCulture)
                : 0f;
        }
        catch
        {
            value = 0f;
        }

        return new Condition
        {
            Sensor = sensor,
            Operator = AddonJson.Str(obj, "operator", "=="),
            Value = value
        };
    }

    private static Result ParseResult(Dictionary<string, object> obj)
    {
        if (obj == null)
            return null;

        string sensorId = AddonJson.Str(obj, "sensor");
        if (!string.IsNullOrWhiteSpace(sensorId))
        {
            SensorDefinition sensor = SensorRegistry.Get(sensorId);
            if (sensor != null)
                return new Result { Sensor = sensor };
        }

        if (obj.TryGetValue("message", out object rawMessage))
        {
            return new Result
            {
                Message = Convert.ToString(rawMessage, CultureInfo.InvariantCulture),
                Unit = AddonJson.Str(obj, "unit")
            };
        }

        return null;
    }

    private void UpdateSelectionAppearance()
    {
        bool selected = activeSensor != null && TraceSelection.IsSelected(activeSensor.Id);
        styles?.SetState(root, "selected", selected);
        styles?.SetState(selectionDash, "selected", selected);
    }
}
