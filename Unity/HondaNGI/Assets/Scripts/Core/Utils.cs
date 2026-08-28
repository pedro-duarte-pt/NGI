using UnityEngine;

namespace NGI.Utilities
{
    public static class Utils
    {
        public enum UnitType
        {
            Mph = 0,
            Gallons = 1,
            Fahrenheit = 2,
            Miles = 3,
            KilometersPerHour = 4,
            Celsius = 5,
            Kilometers = 6,
            LitersPer100Km = 7,
            Mpg = 8,
            Liters = 9
        }

        public static string displayValueInUnits(UnitType unitType, float val)
        {
            //Method assumes International Standard System for source values (e.g.: metric)
            float converted;
            string unit;

            switch (unitType)
            {
                case UnitType.Mph: // from m/s
                    converted = val * 2.23694f;
                    unit = " mph";
                    break;
                case UnitType.Gallons: // from liters
                    converted = val * 0.264172f;
                    unit = " gallons";
                    break;
                case UnitType.Fahrenheit: // from Celsius
                    converted = (val * 9f / 5f) + 32f;
                    unit = " ºF";
                    break;
                case UnitType.Miles: // from meters
                    converted = val / 1609.34f;
                    unit = " miles";
                    break;
                case UnitType.KilometersPerHour: // from m/s
                    converted = val * 3.6f;
                    unit = " km/h";
                    break;
                case UnitType.Celsius:
                    converted = val;
                    unit = " °C";
                    break;
                case UnitType.Kilometers: // from meters
                    converted = val / 1000f;
                    unit = " km";
                    break;
                case UnitType.LitersPer100Km:
                    converted = val;
                    unit = " l/100km";
                    break;
                case UnitType.Mpg: // from l/100km
                    converted = 235.215f / val;
                    unit = " mpg";
                    break;
                case UnitType.Liters:
                    converted = val;
                    unit = " liters";
                    break;
                default:
                    return val + " (unknown unit)";
            }

            return converted.ToString("0.##") + unit;
        }


        // ECU raw-value conversions. DataLogging only transports/unpacks raw values;
        // VehicleData calls these helpers to expose meaningful canonical values.

        public static float EcuSpeedKphToMetersPerSecond(float speedKph)
        {
            return speedKph / 3.6f;
        }

        public static float EcuRpmRawToRpm(float rpmRaw)
        {
            return rpmRaw > 0f ? 1875000f / rpmRaw : 0f;
        }

        public static float EcuInjectorRawToMilliseconds(float injectorRaw)
        {
            return injectorRaw * 3.20000004768372f / 1000.0f;
        }

        public static float EcuO2RawToVolts(float o2Raw)
        {
            return o2Raw / 51.0f;
        }

        public static float EcuBatteryRawToVolts(float batteryRaw)
        {
            return (26.0f * batteryRaw) / 270.0f;
        }

        public static float EcuPercentRawToPercent(float raw)
        {
            return (raw / 255f) * 100f;
        }

        public static float EcuThrottleRawToPercent(float tpsRaw)
        {
            return (float)System.Math.Round(tpsRaw * 0.472637 - 11.46119);
        }

        public static float EcuTemperatureRawToCelsius(float temperatureRaw)
        {
            double value = temperatureRaw / 51.0;

            value =
                (0.1423 * System.Math.Pow(value, 6)) -
                (2.4938 * System.Math.Pow(value, 5)) +
                (17.837 * System.Math.Pow(value, 4)) -
                (68.698 * System.Math.Pow(value, 3)) +
                (154.69 * System.Math.Pow(value, 2)) -
                (232.75 * value) +
                284.24;

            value = ((value - 32.0) * 5.0) / 9.0;
            return (float)value;
        }

        public static double OdometerX100mToKilometers(double odometerX100m)
        {
            return odometerX100m / 10.0;
        }

    }
}
