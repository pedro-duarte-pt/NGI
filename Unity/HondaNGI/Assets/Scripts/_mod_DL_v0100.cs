using UnityEngine;
using integrationBoard;
using System.Timers;
using System;

public static class DataLogging
{

    public static decimal get_Volt(byte binaryData)
    {
        decimal decimalData = Convert.ToDecimal(binaryData);
        return (decimalData / 255) * 5;
    }

    public static void loadData(byte[] USBdata)
    {
        if (USBdata[01]==0x30) { processTPDO1(USBdata); } //if tpdo1
        else if (USBdata[01] == 0x50) { processTPDO2(USBdata); } //if tpdo2
        else if (USBdata[01] == 0x70) { } //if tpdo3
        else if (USBdata[01] == 0x90) { }  //if tpdo4
        else { Debug.Log("unknown message: "+ USBdata[00]); }
        
    }



    static private void processTPDO2(byte[] USBdata)
    {
        // USBdata[6..8] = trip/distance bytes. Kept reserved for the board firmware.

        VehicleData.IAT = (float)get_Temp(USBdata[9]);
        VehicleData.ECT = (float)get_Temp(USBdata[10]);
        VehicleData.Battery = (float)get_Volt(USBdata[11]);
        VehicleData.IAC = ((float)USBdata[12] / 255f) * 100f;

        byte flags = USBdata[13];

        VehicleData.ACC = (flags >> 0) & 0x01;
        VehicleData.PCS = (flags >> 1) & 0x01;
        VehicleData.ALTC = (flags >> 2) & 0x01;
        VehicleData.Fan = (flags >> 3) & 0x01;
        VehicleData.IAB = (flags >> 4) & 0x01;
        VehicleData.FLR = (flags >> 5) & 0x01;
        VehicleData.MIL = (flags >> 6) & 0x01;
        VehicleData.PowerSteering = (flags >> 7) & 0x01;

        // Starter, ServiceConnector and ACSwitch are prepared in VehicleData
        // and SensorRegistry but are intentionally not decoded here yet,
        // because the current CAN TPDO mapping does not transport them.
    }


    static private void processTPDO1(byte[] USBdata)
    {
        VehicleData.Speed = USBdata[6];     //handle Speed
        if ((USBdata[11] + USBdata[12]) > 0)                //handle RPM
        {
            VehicleData.Rpm = 1875000 / (USBdata[11] + USBdata[12] * 256);
        }
        VehicleData.Injectors = (USBdata[7] + USBdata[8] * 256);
        VehicleData.O2nb = (float) get_Volt(USBdata[9]);
        VehicleData.Tps = USBdata[10];
        VehicleData.Gear = USBdata[13]&0x0F;
        VehicleData.BrakePedal = (USBdata[13] & 0x40) >> 6;
        VehicleData.LeftVTEC = (USBdata[13] & 0x10) >> 4;
        VehicleData.RightVTEC = (USBdata[13] & 0x20) >> 5;
        VehicleData.VTECPressure = (USBdata[13] & 0x80)>>7;

    }

    static private decimal get_Temp(int input)
    {
     
        decimal x;
        x = Decimal.Divide((decimal)(-6807 * Math.Pow(input, 3)) , 3303219920);
        x = x + Decimal.Divide((decimal)(7171 * Math.Pow(input, 2)) , 3343340);
        x = x - Decimal.Divide(((decimal)147926159 * (decimal)input) , 173853680);
        x = x + (decimal) 101.67;

        //=-6807*POWER(B1;3)/3303219920+7171*POWER(B1;2)/3343340-147926159*B1/173853680+763274357/7507318

        //convert to celcius
        return Math.Round(x, 0);
    }

    static private decimal get_Temp_f(int input)
    {
        double V = ((double)input / 255) * 5;

        decimal x;
        x = 0.1423M * (decimal)Math.Pow(V, 6)
            - 2.4938M * (decimal)Math.Pow(V, 5)
            + 17.837M * (decimal)Math.Pow(V, 4)
            - 68.698M * (decimal)Math.Pow(V, 3)
            + 154.69M * (decimal)Math.Pow(V, 2)
            - 232.75M * (decimal)V
            + 284.24M;

        //convert to celcius
        x = (x - 32) * ((decimal)5 / 9);

        return Math.Round(x, 0);
    }

}
