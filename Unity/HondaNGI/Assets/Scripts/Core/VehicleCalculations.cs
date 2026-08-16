/// <summary>
/// Derived vehicle values calculated from VehicleData and VehicleConfiguration.
/// </summary>
public static class VehicleCalculations
{
    /// <summary>
    /// Speed enhanced using current tire circumference relative to the OEM tire reference.
    /// Canonical unit remains metres per second, matching VehicleData.Speed.
    /// Falls back to the ECU/VSS speed if tire configuration is invalid.
    /// </summary>
    public static float AdjustedSpeed
    {
        get
        {
            float referenceCircumference =
                VehicleConfiguration.OemTires.NominalCircumferenceMm;

            float currentCircumference =
                VehicleConfiguration.CurrentTires.EffectiveCircumferenceMm;

            if (referenceCircumference <= 0f || currentCircumference <= 0f)
                return VehicleData.Speed;

            return VehicleData.Speed *
                   currentCircumference /
                   referenceCircumference;
        }
    }

    public static float TireSpeedAdjustmentFactor
    {
        get
        {
            float referenceCircumference =
                VehicleConfiguration.OemTires.NominalCircumferenceMm;
            float currentCircumference =
                VehicleConfiguration.CurrentTires.EffectiveCircumferenceMm;

            if (referenceCircumference <= 0f || currentCircumference <= 0f)
                return 1f;

            return currentCircumference / referenceCircumference;
        }
    }

    public static float InjectorDutyCycle
    {
        get
        {
            if (VehicleData.Rpm <= 0f || VehicleData.Injectors <= 0f)
                return 0f;

            return VehicleData.Rpm * VehicleData.Injectors / 1200f;
        }
    }

    public static float FuelRateLitresPerHour
    {
        get
        {
            if (VehicleData.Rpm <= 0f ||
                VehicleData.Injectors <= 0f ||
                VehicleConfiguration.InjectorCount <= 0 ||
                VehicleConfiguration.InjectorFlowCcPerMin <= 0f)
                return 0f;

            return VehicleData.Rpm
                 * VehicleData.Injectors
                 * VehicleConfiguration.InjectorCount
                 * VehicleConfiguration.InjectorFlowCcPerMin
                 / 2000000f;
        }
    }

    /// <summary>
    /// Instantaneous fuel consumption in litres per 100 km using AdjustedSpeed.
    /// </summary>
    public static float FuelConsumptionLitresPer100Km
    {
        get
        {
            float speedKmh = AdjustedSpeed * 3.6f;

            if (speedKmh <= 0f)
                return 0f;

            return FuelRateLitresPerHour * 100f / speedKmh;
        }
    }
}
