using System;
using System.Collections.Generic;
using System.Linq;
 
/// <summary>
/// Describes one externally visible vehicle sensor.
///
/// VehicleData remains the real-time state store.
/// SensorRegistry is the stable, hierarchical API consumed by UI layouts,
/// traces and, later, external/add-on scripting.
/// </summary>
public sealed class SensorDefinition
{
    public string Id { get; }
    public string Name { get; }
    public string ShortName { get; }
    public string Unit { get; }
    public SensorKind Kind { get; }
    public float Min { get; }
    public float Max { get; }
    public int Decimals { get; }
    public IReadOnlyList<string> Tags { get; }

    private readonly Func<float> valueGetter;

    public float Value => valueGetter();

    public SensorDefinition(
        string id,
        string name,
        string shortName,
        string unit,
        SensorKind kind,
        float min,
        float max,
        int decimals,
        Func<float> valueGetter,
        params string[] tags)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Unit = unit;
        Kind = kind;
        Min = min;
        Max = max;
        Decimals = decimals;
        this.valueGetter = valueGetter ?? throw new ArgumentNullException(nameof(valueGetter));
        Tags = tags ?? Array.Empty<string>();
    }
}

public enum SensorKind
{
    Continuous,
    Integer,
    Boolean
}

/// <summary>
/// Public sensor catalogue.
///
/// IMPORTANT:
/// - This class does not store a second copy of the live data.
/// - Values are read directly from VehicleData.
/// - IDs describe vehicle meaning, not CAN/TPDO transport details.
/// </summary>
public static class SensorRegistry
{
    private static readonly Dictionary<string, SensorDefinition> sensors =
        new Dictionary<string, SensorDefinition>(StringComparer.OrdinalIgnoreCase);

    static SensorRegistry()
    {
        // -----------------------------------------------------------------
        // ENGINE
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "engine.rpm",
            name: "Engine Speed",
            shortName: "RPM",
            unit: "rpm",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 9000f,
            decimals: 0,
            valueGetter: () => VehicleData.Rpm,
            "engine", "rpm", "rotational-speed", "rotation"));

        Register(new SensorDefinition(
            id: "engine.coolant_temperature",
            name: "Engine Coolant Temperature",
            shortName: "ECT",
            unit: "°C",
            kind: SensorKind.Continuous,
            min: -20f,
            max: 130f,
            decimals: 0,
            valueGetter: () => VehicleData.ECT,
            "engine", "temperature", "cooling"));

        Register(new SensorDefinition(
            id: "engine.injector.pulse_width",
            name: "Injector Pulse Width",
            shortName: "INJ",
            unit: "ms",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 30f,
            decimals: 2,
            valueGetter: () => VehicleData.Injectors,
            "engine", "fuel", "injector"));

        Register(new SensorDefinition(
            id: "engine.injector.duty_cycle",
            name: "Injector Duty Cycle",
            shortName: "INJ DUTY",
            unit: "%",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 100f,
            decimals: 1,
            valueGetter: () => VehicleCalculations.InjectorDutyCycle,
            "engine", "fuel", "injector", "derived"));

        Register(new SensorDefinition(
            id: "engine.fuel.rate",
            name: "Fuel Rate",
            shortName: "FUEL",
            unit: "L/h",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 100f,
            decimals: 2,
            valueGetter: () => VehicleCalculations.FuelRateLitresPerHour,
            "engine", "fuel", "consumption", "derived"));

        Register(new SensorDefinition(
            id: "engine.fuel.consumption",
            name: "Fuel Consumption",
            shortName: "CONS",
            unit: "L/100 km",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 100f,
            decimals: 1,
            valueGetter: () => VehicleCalculations.FuelConsumptionLitresPer100Km,
            "engine", "fuel", "consumption", "derived"));

        Register(new SensorDefinition(
            id: "engine.cooling.fan",
            name: "Cooling Fan",
            shortName: "FAN",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.Fan,
            "engine", "cooling", "switch"));

        Register(new SensorDefinition(
            id: "engine.vtec.left",
            name: "Left VTEC",
            shortName: "VTEC-L",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.LeftVTEC,
            "engine", "vtec", "switch"));

        Register(new SensorDefinition(
            id: "engine.vtec.right",
            name: "Right VTEC",
            shortName: "VTEC-R",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.RightVTEC,
            "engine", "vtec", "switch"));

        Register(new SensorDefinition(
            id: "engine.vtec.pressure",
            name: "VTEC Pressure Switch",
            shortName: "VTEC-P",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.VTECPressure,
            "engine", "vtec", "pressure", "switch"));


        Register(new SensorDefinition(
            id: "engine.idle_air_control",
            name: "Idle Air Control",
            shortName: "IAC",
            unit: "%",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 100f,
            decimals: 1,
            valueGetter: () => VehicleData.IAC,
            "engine", "idle", "air", "control"));

        // -----------------------------------------------------------------
        // INTAKE
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "intake.throttle.position",
            name: "Throttle Position",
            shortName: "TPS",
            unit: "%",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 100f,
            decimals: 0,
            valueGetter: () => VehicleData.Tps,
            "intake", "throttle", "position"));


        Register(new SensorDefinition(
            id: "intake.air_temperature",
            name: "Intake Air Temperature",
            shortName: "IAT",
            unit: "°C",
            kind: SensorKind.Continuous,
            min: -20f,
            max: 100f,
            decimals: 0,
            valueGetter: () => VehicleData.IAT,
            "intake", "temperature", "air"));

        Register(new SensorDefinition(
            id: "intake.air_bypass",
            name: "Intake Air Bypass",
            shortName: "IAB",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.IAB,
            "intake", "air", "bypass", "switch"));

        // -----------------------------------------------------------------
        // EXHAUST
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "exhaust.oxygen.narrowband",
            name: "Narrowband Oxygen",
            shortName: "O2",
            unit: "",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 20f,
            decimals: 2,
            valueGetter: () => VehicleData.O2nb,
            "exhaust", "oxygen", "mixture"));

        // -----------------------------------------------------------------
        // DRIVETRAIN
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "drivetrain.vehicle_speed",
            name: "Vehicle Speed",
            shortName: "VSS",
            unit: "m/s",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 80f,
            decimals: 1,
            valueGetter: () => VehicleData.Speed,
            "drivetrain", "vehicle-speed", "linear-speed", "vehicle"));

        Register(new SensorDefinition(
            id: "drivetrain.adjusted_speed",
            name: "Adjusted Vehicle Speed",
            shortName: "ADJ",
            unit: "m/s",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 80f,
            decimals: 1,
            valueGetter: () => VehicleCalculations.AdjustedSpeed,
            "drivetrain", "vehicle-speed", "linear-speed", "adjusted", "tires"));

        Register(new SensorDefinition(
            id: "drivetrain.odometer",
            name: "Vehicle Odometer",
            shortName: "ODO",
            unit: "km",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 1677721.5f,
            decimals: 1,
            valueGetter: () => (float)VehicleData.Odometer,
            "drivetrain", "distance", "odometer", "vehicle"));

        Register(new SensorDefinition(
            id: "drivetrain.transmission.gear",
            name: "Selected Gear",
            shortName: "GEAR",
            unit: "",
            kind: SensorKind.Integer,
            min: 0f,
            max: 6f,
            decimals: 0,
            valueGetter: () => VehicleData.Gear,
            "drivetrain", "transmission", "gear"));

        // -----------------------------------------------------------------
        // CHASSIS
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "chassis.brake.pedal",
            name: "Brake Pedal",
            shortName: "BRAKE",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.BrakePedal,
            "chassis", "brake", "switch"));

        Register(new SensorDefinition(
            id: "chassis.steering.power_assist",
            name: "Power Steering",
            shortName: "P/S",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.PowerSteering,
            "chassis", "steering", "switch"));

        // -----------------------------------------------------------------
        // ELECTRICAL
        // -----------------------------------------------------------------

        Register(new SensorDefinition(
            id: "electrical.battery.voltage",
            name: "Battery Sense Voltage",
            shortName: "BAT",
            unit: "V",
            kind: SensorKind.Continuous,
            min: 0f,
            max: 5f,
            decimals: 2,
            valueGetter: () => VehicleData.Battery,
            "electrical", "battery", "voltage"));


        Register(new SensorDefinition(
            id: "electrical.alternator.control",
            name: "Alternator Control",
            shortName: "ALTC",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.ALTC,
            "electrical", "alternator", "control", "switch"));

        Register(new SensorDefinition(
            id: "electrical.accessory.acc",
            name: "ACC Output",
            shortName: "ACC",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.ACC,
            "electrical", "accessory", "switch"));

        // -----------------------------------------------------------------
        // DIAGNOSTICS
        // -----------------------------------------------------------------


        Register(new SensorDefinition(
            id: "engine.control.pcs",
            name: "PCS Output",
            shortName: "PCS",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.PCS,
            "engine", "control", "switch"));

        Register(new SensorDefinition(
            id: "engine.fuel.flr",
            name: "FLR Output",
            shortName: "FLR",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.FLR,
            "engine", "fuel", "switch"));

        Register(new SensorDefinition(
            id: "electrical.starter",
            name: "Starter",
            shortName: "STARTER",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.Starter,
            "electrical", "starter", "input", "switch"));

        Register(new SensorDefinition(
            id: "electrical.service_connector",
            name: "Service Connector",
            shortName: "SRVCON",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.ServiceConnector,
            "electrical", "service", "input", "switch"));

        Register(new SensorDefinition(
            id: "climate.ac_switch",
            name: "A/C Switch",
            shortName: "A/C",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.ACSwitch,
            "climate", "air-conditioning", "input", "switch"));

        Register(new SensorDefinition(
            id: "diagnostics.mil",
            name: "Malfunction Indicator Lamp",
            shortName: "MIL",
            unit: "",
            kind: SensorKind.Boolean,
            min: 0f,
            max: 1f,
            decimals: 0,
            valueGetter: () => VehicleData.MIL,
            "diagnostics", "warning", "switch"));
    }

    private static void Register(SensorDefinition sensor)
    {
        if (sensor == null)
            throw new ArgumentNullException(nameof(sensor));

        if (sensors.ContainsKey(sensor.Id))
            throw new InvalidOperationException("Duplicate sensor ID: " + sensor.Id);

        sensors.Add(sensor.Id, sensor);
    }

    /// <summary>
    /// Returns a sensor definition, or null when the ID does not exist.
    /// </summary>
    public static SensorDefinition Get(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        sensors.TryGetValue(id, out SensorDefinition sensor);
        return sensor;
    }

    /// <summary>
    /// Convenience method for UI/add-on consumers.
    /// </summary>
    public static bool TryGetValue(string id, out float value)
    {
        SensorDefinition sensor = Get(id);

        if (sensor == null)
        {
            value = 0f;
            return false;
        }

        value = sensor.Value;
        return true;
    }

    /// <summary>
    /// All sensors in the public catalogue.
    /// </summary>
    public static IEnumerable<SensorDefinition> All => sensors.Values;

    /// <summary>
    /// Returns all sensors below a hierarchical path.
    /// Example: Under("engine") returns engine.rpm, engine.vtec.*, etc.
    /// </summary>
    public static IEnumerable<SensorDefinition> Under(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return All;

        string prefix = path.EndsWith(".") ? path : path + ".";

        return sensors.Values.Where(
            sensor => sensor.Id.Equals(path, StringComparison.OrdinalIgnoreCase) ||
                      sensor.Id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns all sensors carrying a given metadata tag.
    /// Example: WithTag("temperature").
    /// </summary>
    public static IEnumerable<SensorDefinition> WithTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return Enumerable.Empty<SensorDefinition>();

        return sensors.Values.Where(
            sensor => sensor.Tags.Any(
                sensorTag => sensorTag.Equals(tag, StringComparison.OrdinalIgnoreCase)));
    }
}
