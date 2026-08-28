using UnityEngine;

public static class DataLogging
{
    public static void loadData(byte[] USBdata)
    {
        if (USBdata[1] == 0x30) { processTPDO1(USBdata); }
        else if (USBdata[1] == 0x50) { processTPDO2(USBdata); }
        else if (USBdata[1] == 0x70) { } // TPDO3
        else if (USBdata[1] == 0x90) { } // TPDO4
        else { Debug.Log("unknown message: " + USBdata[0]); }
    }

    private static void processTPDO2(byte[] USBdata)
    {
        uint odometerX100m = (uint)USBdata[6]
                            | ((uint)USBdata[7] << 8)
                            | ((uint)USBdata[8] << 16);

        VehicleData.Odometer = odometerX100m;

        VehicleData.IAT = USBdata[9];
        VehicleData.ECT = USBdata[10];
        VehicleData.Battery = USBdata[11];
        VehicleData.IAC = USBdata[12];

        byte flags = USBdata[13];

        VehicleData.ACC = (flags >> 0) & 0x01;
        VehicleData.PCS = (flags >> 1) & 0x01;
        VehicleData.ALTC = (flags >> 2) & 0x01;
        VehicleData.Fan = (flags >> 3) & 0x01;
        VehicleData.IAB = (flags >> 4) & 0x01;
        VehicleData.FLR = (flags >> 5) & 0x01;
        VehicleData.MIL = (flags >> 6) & 0x01;
        VehicleData.PowerSteering = (flags >> 7) & 0x01;
    }

    private static void processTPDO1(byte[] USBdata)
    {
        ushort injectorRaw = (ushort)(USBdata[7] | (USBdata[8] << 8));
        ushort rpmRaw = (ushort)(USBdata[11] | (USBdata[12] << 8));

        VehicleData.Speed = USBdata[6];
        VehicleData.Injectors = injectorRaw;
        VehicleData.O2nb = USBdata[9];
        VehicleData.Tps = USBdata[10];
        VehicleData.Rpm = rpmRaw;

        byte flags = USBdata[13];
        VehicleData.Gear = flags & 0x0F;
        VehicleData.LeftVTEC = (flags >> 4) & 0x01;
        VehicleData.RightVTEC = (flags >> 5) & 0x01;
        VehicleData.BrakePedal = (flags >> 6) & 0x01;
        VehicleData.VTECPressure = (flags >> 7) & 0x01;
    }
}
