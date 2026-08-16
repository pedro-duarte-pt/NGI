using System;

/// <summary>
/// Converts canonical SensorRegistry values to driver-selected display units.
/// SensorDefinition.Value, Min and Max remain canonical and are therefore safe
/// for calculations, conditions and add-on logic.
/// </summary>
public static class UnitPresentation
{
    public static float Value(SensorDefinition sensor, float canonicalValue)
    {
        if (sensor == null) return canonicalValue;

        if (IsTemperature(sensor))
            return UserPreferences.Temperature == TemperatureUnit.Fahrenheit
                ? canonicalValue * 9f / 5f + 32f
                : canonicalValue;

        if (sensor.Id.Equals("drivetrain.vehicle_speed", StringComparison.OrdinalIgnoreCase))
        {
            // Canonical VehicleData speed is m/s.
            return UserPreferences.Speed == SpeedUnit.MilesPerHour
                ? canonicalValue * 2.23693629f
                : canonicalValue * 3.6f;
        }

        if (sensor.Id.Equals("drivetrain.odometer", StringComparison.OrdinalIgnoreCase))
            return UserPreferences.Distance == DistanceUnit.Mile
                ? canonicalValue * 0.621371192f
                : canonicalValue;

        if (sensor.Id.Equals("engine.fuel.consumption", StringComparison.OrdinalIgnoreCase))
        {
            if (canonicalValue <= 0f) return 0f;
            switch (UserPreferences.FuelConsumption)
            {
                case FuelConsumptionUnit.MilesPerGallonUS: return 235.214583f / canonicalValue;
                case FuelConsumptionUnit.MilesPerGallonUK: return 282.480936f / canonicalValue;
                default: return canonicalValue;
            }
        }

        if (IsPressure(sensor))
        {
            // Pressure sensors are expected to use bar as the canonical registry unit.
            switch (UserPreferences.Pressure)
            {
                case PressureUnit.Psi: return canonicalValue * 14.5037738f;
                case PressureUnit.Kilopascal: return canonicalValue * 100f;
                default: return canonicalValue;
            }
        }

        return canonicalValue;
    }


    public static float Min(SensorDefinition sensor)
    {
        if (sensor != null && sensor.Id.Equals("engine.fuel.consumption", StringComparison.OrdinalIgnoreCase) &&
            UserPreferences.FuelConsumption != FuelConsumptionUnit.LitersPer100Km)
            return Value(sensor, sensor.Max);
        return Value(sensor, sensor.Min);
    }

    public static float Max(SensorDefinition sensor)
    {
        if (sensor != null && sensor.Id.Equals("engine.fuel.consumption", StringComparison.OrdinalIgnoreCase) &&
            UserPreferences.FuelConsumption != FuelConsumptionUnit.LitersPer100Km)
            return Value(sensor, Math.Max(sensor.Min, 0.5f));
        return Value(sensor, sensor.Max);
    }

    public static string Unit(SensorDefinition sensor)
    {
        if (sensor == null) return "";

        if (IsTemperature(sensor))
            return UserPreferences.Temperature == TemperatureUnit.Fahrenheit ? "°F" : "°C";

        if (sensor.Id.Equals("drivetrain.vehicle_speed", StringComparison.OrdinalIgnoreCase))
            return UserPreferences.Speed == SpeedUnit.MilesPerHour ? "mph" : "km/h";

        if (sensor.Id.Equals("drivetrain.odometer", StringComparison.OrdinalIgnoreCase))
            return UserPreferences.Distance == DistanceUnit.Mile ? "mi" : "km";

        if (sensor.Id.Equals("engine.fuel.consumption", StringComparison.OrdinalIgnoreCase))
        {
            switch (UserPreferences.FuelConsumption)
            {
                case FuelConsumptionUnit.MilesPerGallonUS: return "mpg US";
                case FuelConsumptionUnit.MilesPerGallonUK: return "mpg UK";
                default: return "L/100 km";
            }
        }

        if (IsPressure(sensor))
        {
            switch (UserPreferences.Pressure)
            {
                case PressureUnit.Psi: return "psi";
                case PressureUnit.Kilopascal: return "kPa";
                default: return "bar";
            }
        }

        return sensor.Unit;
    }

    private static bool IsTemperature(SensorDefinition sensor) =>
        sensor.Unit == "°C" || HasTag(sensor, "temperature");

    private static bool IsPressure(SensorDefinition sensor) =>
        sensor.Unit.Equals("bar", StringComparison.OrdinalIgnoreCase) || HasTag(sensor, "pressure-value");

    private static bool HasTag(SensorDefinition sensor, string tag)
    {
        foreach (string item in sensor.Tags)
            if (item.Equals(tag, StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }
}
