/// <summary>
/// Derived vehicle values calculated from VehicleData and VehicleConfiguration.
///
/// Fuel calculations currently use nominal injector flow. Fuel-pressure/MAP
/// correction can be added here later when reliable live/configured inputs exist.
/// </summary>
public static class VehicleCalculations
{
    /// <summary>
    /// Injector duty cycle in percent for a four-stroke engine.
    /// VehicleData.Injectors is injector pulse width in milliseconds.
    /// </summary>
    public static float InjectorDutyCycle
    {
        get
        {
            if (VehicleData.Rpm <= 0f || VehicleData.Injectors <= 0f)
                return 0f;

            return VehicleData.Rpm * VehicleData.Injectors / 1200f;
        }
    }

    /// <summary>Instantaneous nominal fuel flow in litres per hour.</summary>
    public static float FuelRateLitresPerHour
    {
        get
        {
            if (VehicleData.Rpm <= 0f ||
                VehicleData.Injectors <= 0f ||
                VehicleConfiguration.InjectorCount <= 0 ||
                VehicleConfiguration.InjectorFlowCcPerMin <= 0f)
            {
                return 0f;
            }

            return VehicleData.Rpm
                 * VehicleData.Injectors
                 * VehicleConfiguration.InjectorCount
                 * VehicleConfiguration.InjectorFlowCcPerMin
                 / 2000000f;
        }
    }

    /// <summary>
    /// Instantaneous fuel consumption in litres per 100 km.
    /// Returns zero while stationary; presentation logic will select L/h at low speed.
    /// </summary>
    public static float FuelConsumptionLitresPer100Km
    {
        get
        {
            float speedKmh = VehicleData.Speed * 3.6f;

            if (speedKmh <= 0f)
                return 0f;

            return FuelRateLitresPerHour * 100f / speedKmh;
        }
    }
}
