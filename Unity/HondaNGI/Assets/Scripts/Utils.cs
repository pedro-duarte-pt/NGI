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
    }
}