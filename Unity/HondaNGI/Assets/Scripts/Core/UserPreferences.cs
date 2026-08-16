using System;
using UnityEngine;

public enum TemperatureUnit { Celsius = 0, Fahrenheit = 1 }
public enum SpeedUnit { KilometersPerHour = 0, MilesPerHour = 1 }
public enum PressureUnit { Bar = 0, Psi = 1, Kilopascal = 2 }
public enum DistanceUnit { Kilometer = 0, Mile = 1 }
public enum FuelConsumptionUnit { LitersPer100Km = 0, MilesPerGallonUS = 1, MilesPerGallonUK = 2 }

/// <summary>
/// Driver presentation preferences. VehicleData and calculations remain in
/// their canonical units; these settings only affect presentation.
/// </summary>
public static class UserPreferences
{
    private const string TemperatureKey = "prefs.units.temperature";
    private const string SpeedKey = "prefs.units.speed";
    private const string PressureKey = "prefs.units.pressure";
    private const string DistanceKey = "prefs.units.distance";
    private const string FuelConsumptionKey = "prefs.units.fuelConsumption";

    public static event Action Changed;

    public static TemperatureUnit Temperature
    {
        get => Read(TemperatureKey, TemperatureUnit.Celsius);
        set => Write(TemperatureKey, value);
    }

    public static SpeedUnit Speed
    {
        get => Read(SpeedKey, SpeedUnit.KilometersPerHour);
        set => Write(SpeedKey, value);
    }

    public static PressureUnit Pressure
    {
        get => Read(PressureKey, PressureUnit.Bar);
        set => Write(PressureKey, value);
    }

    public static DistanceUnit Distance
    {
        get => Read(DistanceKey, DistanceUnit.Kilometer);
        set => Write(DistanceKey, value);
    }

    public static FuelConsumptionUnit FuelConsumption
    {
        get => Read(FuelConsumptionKey, FuelConsumptionUnit.LitersPer100Km);
        set => Write(FuelConsumptionKey, value);
    }

    public static void ResetToMetricDefaults()
    {
        PlayerPrefs.DeleteKey(TemperatureKey);
        PlayerPrefs.DeleteKey(SpeedKey);
        PlayerPrefs.DeleteKey(PressureKey);
        PlayerPrefs.DeleteKey(DistanceKey);
        PlayerPrefs.DeleteKey(FuelConsumptionKey);
        PlayerPrefs.Save();
        Changed?.Invoke();
    }

    private static T Read<T>(string key, T fallback) where T : struct
    {
        int raw = PlayerPrefs.GetInt(key, Convert.ToInt32(fallback));
        return Enum.IsDefined(typeof(T), raw) ? (T)Enum.ToObject(typeof(T), raw) : fallback;
    }

    private static void Write<T>(string key, T value) where T : struct
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
        PlayerPrefs.Save();
        Changed?.Invoke();
    }
}
