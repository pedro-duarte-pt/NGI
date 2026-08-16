using System;

/// <summary>
/// Physical vehicle configuration used by application-level calculations.
/// </summary>
public static class VehicleConfiguration
{
    public static int CylinderCount = 4;
    public static int InjectorCount = 4;
    public static float InjectorFlowCcPerMin = 240f;

    // OEM VSS reference tire: 185/65 R14.
    public static TireConfiguration OemTires = new TireConfiguration
    {
        Make = "",
        Model = "",
        WidthMm = 185,
        AspectRatio = 65,
        WheelDiameterInches = 14f,
        LoadIndex = 0,
        SpeedRating = "",
        RollingCircumferenceMm = 0f
    };

    // Currently installed tires: 195/50 R15.
    public static TireConfiguration CurrentTires = new TireConfiguration
    {
        Make = "",
        Model = "",
        WidthMm = 195,
        AspectRatio = 50,
        WheelDiameterInches = 15f,
        LoadIndex = 0,
        SpeedRating = "",
        RollingCircumferenceMm = 0f
    };
}

/// <summary>One tire specification, currently assumed identical on all four wheels.</summary>
public class TireConfiguration
{
    public string Make = "";
    public string Model = "";
    public int WidthMm;
    public int AspectRatio;
    public float WheelDiameterInches;
    public int LoadIndex;
    public string SpeedRating = "";

    /// <summary>Optional measured rolling circumference; zero uses nominal geometry.</summary>
    public float RollingCircumferenceMm;

    public float SidewallHeightMm =>
        WidthMm > 0 && AspectRatio > 0 ? WidthMm * (AspectRatio / 100f) : 0f;

    public float NominalDiameterMm =>
        WheelDiameterInches > 0f
            ? WheelDiameterInches * 25.4f + 2f * SidewallHeightMm
            : 0f;

    public float NominalCircumferenceMm =>
        NominalDiameterMm > 0f ? NominalDiameterMm * (float)Math.PI : 0f;

    public float EffectiveCircumferenceMm =>
        RollingCircumferenceMm > 0f ? RollingCircumferenceMm : NominalCircumferenceMm;
}
