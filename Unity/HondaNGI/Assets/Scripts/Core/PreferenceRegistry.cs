using System;
using System.Collections.Generic;

/// <summary>
/// Controlled bridge between declarative add-on UI and core-owned user preferences.
/// Add-ons can only read/set preference IDs and values explicitly exposed here.
/// </summary>
public static class PreferenceRegistry
{
    public sealed class Definition
    {
        public string Id { get; }
        public string Label { get; }
        public IReadOnlyList<string> Options { get; }
        private readonly Func<string> getter;
        private readonly Action<string> setter;

        public Definition(string id, string label, IReadOnlyList<string> options,
            Func<string> getter, Action<string> setter)
        {
            Id = id;
            Label = label;
            Options = options;
            this.getter = getter;
            this.setter = setter;
        }

        public string GetValue() => getter();

        public bool TrySetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            bool valid = false;

            for (int i = 0; i < Options.Count; i++)
            {
                if (string.Equals(
                        Options[i],
                        value,
                        StringComparison.Ordinal))
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
                return false;

            setter(value);
            return true;
        }
    }

    private static readonly Dictionary<string, Definition> definitions =
        new Dictionary<string, Definition>(StringComparer.OrdinalIgnoreCase)
        {
            ["units.temperature"] = new Definition(
                "units.temperature", "TEMPERATURE",
                new[] { "Celsius", "Fahrenheit" },
                () => UserPreferences.Temperature.ToString(),
                v => UserPreferences.Temperature =
                    (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), v)),

            ["units.speed"] = new Definition(
                "units.speed", "SPEED",
                new[] { "KilometersPerHour", "MilesPerHour" },
                () => UserPreferences.Speed.ToString(),
                v => UserPreferences.Speed =
                    (SpeedUnit)Enum.Parse(typeof(SpeedUnit), v)),

            ["units.pressure"] = new Definition(
                "units.pressure", "PRESSURE",
                new[] { "Bar", "Psi", "Kilopascal" },
                () => UserPreferences.Pressure.ToString(),
                v => UserPreferences.Pressure =
                    (PressureUnit)Enum.Parse(typeof(PressureUnit), v)),

            ["units.distance"] = new Definition(
                "units.distance", "DISTANCE",
                new[] { "Kilometer", "Mile" },
                () => UserPreferences.Distance.ToString(),
                v => UserPreferences.Distance =
                    (DistanceUnit)Enum.Parse(typeof(DistanceUnit), v)),

            ["units.fuelConsumption"] = new Definition(
                "units.fuelConsumption", "FUEL CONSUMPTION",
                new[] { "LitersPer100Km", "MilesPerGallonUS", "MilesPerGallonUK" },
                () => UserPreferences.FuelConsumption.ToString(),
                v => UserPreferences.FuelConsumption =
                    (FuelConsumptionUnit)Enum.Parse(typeof(FuelConsumptionUnit), v))
        };

    public static bool TryGet(string id, out Definition definition) =>
        definitions.TryGetValue(id ?? string.Empty, out definition);
}
