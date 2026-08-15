/// <summary>
/// Persistent vehicle-specific configuration.
///
/// These defaults describe the current B16A2/P30 setup. A future Settings
/// screen can edit/persist these values without changing the calculation API.
/// </summary>
public static class VehicleConfiguration
{
    /// <summary>Number of engine cylinders.</summary>
    public static int CylinderCount { get; set; } = 4;

    /// <summary>Number of fuel injectors used by the fuel-rate calculation.</summary>
    public static int InjectorCount { get; set; } = 4;

    /// <summary>Nominal injector flow at the injector's rated differential pressure, in cc/min.</summary>
    public static float InjectorFlowCcPerMin { get; set; } = 240f;
}
