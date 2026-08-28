using NGI.Utilities;

/// <summary>
/// Global realtime vehicle state.
///
/// DataLogging writes raw/unpacked ECU values here; setters convert them to canonical values through Utils.
/// SensorRegistry exposes them through stable hierarchical sensor IDs.
///
/// This is intentionally a plain static data class:
/// - one vehicle
/// - one realtime state
/// - no Unity asset or singleton lifecycle required
/// </summary>
public static class VehicleData
{
    private static float ECU_speed_val = 0f;
    private static float ECU_rpm_val = 0f;
    private static float ECU_inj_val = 0f;
    private static float ECU_02nb_val = 0f;
    private static float ECU_tps_val = 0f;
    private static int ECU_gear_val = 0;
    private static int ECU_braking_val = 0;
    private static int ECU_leftVTEC_val = 0;
    private static int ECU_rightVTEC_val = 0;
    private static int ECU_VTCpressure_val = 0;
    private static float ECU_ECT_val = 0f;
    private static int ECU_fanc_val = 0;
    private static int ECU_tpdo_val = 0;
    private static int ECU_mil_val = 0;
    private static int ECU_PowerSteering_val = 0;
    private static float ECU_battery_val = 0f;
    private static double ECU_odometer_val = 0.0;

    private static float ECU_IAT_val = 0f;
    private static float ECU_IAC_val = 0f;
    private static int ECU_ACC_val = 0;
    private static int ECU_PCS_val = 0;
    private static int ECU_ALTC_val = 0;
    private static int ECU_IAB_val = 0;
    private static int ECU_FLR_val = 0;
    private static int ECU_ServiceConnector_val = 0;
    private static int ECU_Starter_val = 0;
    private static int ECU_ACSwitch_val = 0;


    /// <summary>Intake air temperature in °C.</summary>
    public static float IAT
    {
        get => ECU_IAT_val;
        set => ECU_IAT_val = Utils.EcuTemperatureRawToCelsius(value);
    }

    /// <summary>Idle air control command/position in percent.</summary>
    public static float IAC
    {
        get => ECU_IAC_val;
        set => ECU_IAC_val = Utils.EcuPercentRawToPercent(value);
    }

    /// <summary>A/C compressor request/control flag from ECU P0.</summary>
    public static int ACC
    {
        get => ECU_ACC_val;
        set => ECU_ACC_val = value;
    }

    /// <summary>PCS output flag.</summary>
    public static int PCS
    {
        get => ECU_PCS_val;
        set => ECU_PCS_val = value;
    }

    /// <summary>Alternator control flag.</summary>
    public static int ALTC
    {
        get => ECU_ALTC_val;
        set => ECU_ALTC_val = value;
    }

    /// <summary>Intake air bypass control flag.</summary>
    public static int IAB
    {
        get => ECU_IAB_val;
        set => ECU_IAB_val = value;
    }

    /// <summary>Fuel-related FLR flag exposed by the ECU.</summary>
    public static int FLR
    {
        get => ECU_FLR_val;
        set => ECU_FLR_val = value;
    }

    /// <summary>Service connector input state. Reserved until transported by CAN.</summary>
    public static int ServiceConnector
    {
        get => ECU_ServiceConnector_val;
        set => ECU_ServiceConnector_val = value;
    }

    /// <summary>Starter input state. Reserved until transported by CAN.</summary>
    public static int Starter
    {
        get => ECU_Starter_val;
        set => ECU_Starter_val = value;
    }

    /// <summary>A/C switch input state. Reserved until transported by CAN.</summary>
    public static int ACSwitch
    {
        get => ECU_ACSwitch_val;
        set => ECU_ACSwitch_val = value;
    }

    /// <summary>Engine coolant temperature in °C.</summary>
    public static float ECT
    {
        get => ECU_ECT_val;
        set => ECU_ECT_val = Utils.EcuTemperatureRawToCelsius(value);
    }

    /// <summary>
    /// Vehicle speed.
    /// Raw TPDO speed is expressed in km/h; stored internally in m/s.
    /// </summary>
    public static float Speed
    {
        get => ECU_speed_val;
        set => ECU_speed_val = Utils.EcuSpeedKphToMetersPerSecond(value);
    }

    /// <summary>
    /// Vehicle battery voltage in volts.
    /// </summary>
    public static float Battery
    {
        get => ECU_battery_val;
        set => ECU_battery_val = Utils.EcuBatteryRawToVolts(value);
    }

    /// <summary>Permanent vehicle odometer in kilometres.</summary>
    public static double Odometer
    {
        get => ECU_odometer_val;
        set => ECU_odometer_val = Utils.OdometerX100mToKilometers(value);
    }

    /// <summary>Engine speed in rpm.</summary>
    public static float Rpm
    {
        get => ECU_rpm_val;
        set => ECU_rpm_val = Utils.EcuRpmRawToRpm(value);
    }

    /// <summary>
    /// Raw/debug TPDO value. Kept for compatibility.
    /// </summary>
    public static int TPDO
    {
        get => ECU_tpdo_val;
        set => ECU_tpdo_val = value;
    }

    /// <summary>Cooling fan state: 0 = off, 1 = on.</summary>
    public static int Fan
    {
        get => ECU_fanc_val;
        set => ECU_fanc_val = value;
    }

    /// <summary>Malfunction Indicator Lamp state: 0 = off, 1 = on.</summary>
    public static int MIL
    {
        get => ECU_mil_val;
        set => ECU_mil_val = value;
    }

    /// <summary>Power steering switch/state: 0 = off, 1 = on.</summary>
    public static int PowerSteering
    {
        get => ECU_PowerSteering_val;
        set => ECU_PowerSteering_val = value;
    }

    /// <summary>
    /// Injector pulse width in milliseconds.
    /// Converted from the raw ECU injector timer value.
    /// </summary>
    public static float Injectors
    {
        get => ECU_inj_val;
        set => ECU_inj_val = Utils.EcuInjectorRawToMilliseconds(value);
    }

    /// <summary>
    /// Narrowband oxygen sensor voltage in volts.
    /// </summary>
    public static float O2nb
    {
        get => ECU_02nb_val;
        set => ECU_02nb_val = Utils.EcuO2RawToVolts(value);
    }

    /// <summary>
    /// Throttle position in percent.
    /// Converted from the raw ECU throttle value.
    /// </summary>
    public static float Tps
    {
        get => ECU_tps_val;
        set => ECU_tps_val = Utils.EcuThrottleRawToPercent(value);
    }

    /// <summary>Selected gear.</summary>
    public static int Gear
    {
        get => ECU_gear_val;
        set => ECU_gear_val = value;
    }

    /// <summary>Brake pedal state: 0 = released, 1 = pressed.</summary>
    public static int BrakePedal
    {
        get => ECU_braking_val;
        set => ECU_braking_val = value;
    }

    /// <summary>Left VTEC state.</summary>
    public static int LeftVTEC
    {
        get => ECU_leftVTEC_val;
        set => ECU_leftVTEC_val = value;
    }

    /// <summary>Right VTEC state.</summary>
    public static int RightVTEC
    {
        get => ECU_rightVTEC_val;
        set => ECU_rightVTEC_val = value;
    }

    /// <summary>VTEC pressure switch state.</summary>
    public static int VTECPressure
    {
        get => ECU_VTCpressure_val;
        set => ECU_VTCpressure_val = value;
    }
}
