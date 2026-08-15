#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "timers.h"
#include "system.h"
#include "dlog.h"
#include "rs232.h"
#include "canopen/ECUCAN_App.h"

   
//watchdog for blocked ECUs
unsigned char fetch_watchdog = 0;

//current sensor being queried
unsigned char dataIndex = _SENSOR_ENTRY_MAX;
//increments every datalogging period (Default=40ms), resets after reaching 10
unsigned char timeSlice;
// Vehicle odometer.
// Public CANopen value: 0.1 km per count, stored in the low 24 bits.
// Internal remainder: accumulated meter-milliseconds, retained so 40 ms VSS
// samples do not lose fractional distance.
unsigned long odometerX100m = 0;
unsigned long odometerRemainderMeterMs = 0;

unsigned char bitsized_data[15] = {
        0x00, //ACC
        0x00, //PCS
        0x00, //ALTC
        0x00, //FANC
        0x00, //IAB
        0x00, //FLR
        0x00, //VTEC1
        0x00, //VTEC2
        0x00, //MIL
        0x00, //PWRSTEER
        0x00, //SERVCON
        0x00, //STARTER
        0x00, //VTP
        0x00, //AC
        0x00 //BRAKE
};
// ECU acquisition entry:
// command type, address MSB/command, address LSB, polling period, raw value
ECU_SENSOR_ENTRY ecuSensors[_SENSOR_ENTRY_MAX + 1] = {
        {0x02, 0x10, 0x00, 1, 0x00}, //RPM LSB
        {0x02, 0x11, 0x00, 1, 0x00}, //RPM MSB
        {0x02, 0x14, 0x00, 1, 0x00}, //MAP
        {0x02, 0x15, 0x00, 1, 0x00}, //TPS
        {0x02, 0x17, 0x00, 2, 0x00}, //Injector LSB
        {0x02, 0x18, 0x00, 2, 0x00}, //Injector MSB
        {0x02, 0x1B, 0x00, 2, 0x00}, //IAT
        {0x02, 0x1C, 0x00, 1, 0x00}, //VSS
        {0x02, 0x1D, 0x00, 2, 0x00}, //ECT
        {0x02, 0x1E, 0x00, 5, 0x00}, //PA (not available)
        {0x02, 0x1F, 0x00, 5, 0x00}, //IACV / BAT
        {0x02, 0x20, 0x00, 1, 0x00}, //O2
        {0x02, 0x21, 0x00, 1, 0x00}, //P0
        {0x02, 0x22, 0x00, 1, 0x00}, //P1
        {0x02, 0x23, 0x00, 1, 0x00}, //INPUT1
        {0x02, 0x24, 0x00, 1, 0x00}, //INPUT2
        {0x02, 0x25, 0x00, 5, 0x00}, //CEL W1 B1
        {0x02, 0x26, 0x00, 5, 0x00}, //CEL W1 B2
        {0x02, 0x27, 0x00, 5, 0x00}, //CEL W2 B1
        {0x02, 0x28, 0x00, 5, 0x00}, //CEL W2 B2
        {0x01, 0x03, 0xC6, 5, 0x00}, //ELD (not available)
        {0x01, 0x03, 0xC5, 5, 0x00}, //Battery/IACV
        {0x01, 0x02, 0x4F, 1, 0x00}, //Gear
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}, //undefined
        {0x00, 0x00, 0x00, 0, 0x00}  //undefined
};


void startDL(void) {
    startDataFetch_flag = 0;
    fetch_watchdog = 0;
    dataIndex = _SENSOR_ENTRY_MAX;
}
//validate DOC 2.0 ECU code
void checkECU(char val) {
    if (val==0xCD) {
        //verified datalagging code in ECU, so set device operacional
        deviceStatus = _DEV_ON;
        operatingMode = _MODE_DL;   //default operating mode
    }
}

//event handler for TPDO based on elaped Time (in ms)
//choose what to do based on time elapsed and operating mode
void TPDOEvent(int ElapsedMs) {
    //Redundant verification but.. just to make sure..
    if (deviceStatus==_DEV_ON) {
        if (operatingMode==_MODE_DL) {
            if (ElapsedMs==_MICROEVENT) {
                //by default every 40ms (TMR3) 
                TPDOs[0].status.isQueuedForTX = 1;
            }
            else if (ElapsedMs==_MACROEVENT) {
                //by default every 1s (TMR0)
                TPDOs[1].status.isQueuedForTX = 1;
            }
        }
    }
}

//decide what to do with the received BYTE based on the current operatiing mode
void handleByte(char byte) {
    switch(operatingMode) {
        case _MODE_DL: getDLData(byte); break;
        case _MODE_SC: break;
        case _MODE_CM: break;
    }
}

//check if sensor is elegible for this specific timeslice
int isElegible() {
    if (ecuSensors[dataIndex].type == 0x00) {
        return 0;
    }

    if (ecuSensors[dataIndex].period == 0) {
        return 0;
    }

    return ((timeSlice % ecuSensors[dataIndex].period) == 0);
}

void load_bits(unsigned char sensor, char answer) {
    switch (sensor) {
            case _ECU_SENSOR_P0: 
                bitsized_data[_DL_BIT_ACC] = (answer >> _DL_BIT_ACC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_PCS] = (answer >> _DL_BIT_PCS_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_ALTC] = (answer >> _DL_BIT_ALTC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_FANC] = (answer >> _DL_BIT_FANC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_IAB] = (answer >> _DL_BIT_IAB_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_FLR] = (answer >> _DL_BIT_FLR_OFFSET) & 0x01;
                break;
            case _ECU_SENSOR_P1: 
                bitsized_data[_DL_BIT_VTEC1] = (answer >> _DL_BIT_VTEC1_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_VTEC2] = (answer >> _DL_BIT_VTEC2_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_MIL] = (answer >> _DL_BIT_MIL_OFFSET) & 0x01;
                break;
            case _ECU_SENSOR_INPUT1: 
                bitsized_data[_DL_BIT_PWRSTEER] = (answer >> _DL_BIT_PWRSTEER_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_SERVCON] = (answer >> _DL_BIT_SERVCON_OFFSET) & 0x01;
                break;
            case _ECU_SENSOR_INPUT2: 
                bitsized_data[_DL_BIT_STARTER] = (answer >> _DL_BIT_STARTER_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_VTP] = (answer >> _DL_BIT_VTP_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_AC] = (answer >> _DL_BIT_AC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_BRAKE] = (answer >> _DL_BIT_BRAKE_OFFSET) & 0x01;
                break;
    };
}

//store sensor information received from ECU
int getDLData(char answer) {    
    unsigned char sensor = dataIndex;
    ecuSensors[sensor].value = (unsigned char)answer;
        
    //handle special sensors
    switch (sensor) {
        case _ECU_SENSOR_VSS:
            Odometer_Update((unsigned char)answer); break;
        case _ECU_SENSOR_P0: 
        case _ECU_SENSOR_P1: 
        case _ECU_SENSOR_INPUT1: 
        case _ECU_SENSOR_INPUT2: 
            load_bits(sensor,answer); break;
        case _ECU_SENSOR_CEL1: 
        case _ECU_SENSOR_CEL2: 
        case _ECU_SENSOR_CEL3: 
        case _ECU_SENSOR_CEL4: registerCEL(answer); break;
    }
    
    //TODO: for multi-byte answers, get next byte
    //sendRS232Data(0x08);
    
    //else if dataFetch is not over yet, get Next Sensor in Line 
    if (dataIndex<_SENSOR_ENTRY_MAX) { 
        dataIndex++; 
        askDLData();
    }
   
    return 0;
}

void Odometer_Update(unsigned char speedKmh) {
    /*
     * VSS is sampled every _MICROEVENT milliseconds (currently 40 ms).
     *
     * distance [m] = speed [km/h] * time [ms] / 3600
     *
     * Accumulate the numerator in meter-milliseconds. One odometer count is
     * 100 m, therefore one count corresponds to 360000 meter-milliseconds.
     * This uses integer arithmetic only and preserves fractional distance
     * between samples.
     */
    const unsigned long ODOMETER_COUNT_THRESHOLD = 360000UL;

    odometerRemainderMeterMs +=
        (unsigned long)speedKmh * (unsigned long)_MICROEVENT;

    while (odometerRemainderMeterMs >= ODOMETER_COUNT_THRESHOLD) {
        odometerRemainderMeterMs -= ODOMETER_COUNT_THRESHOLD;

        if (odometerX100m < 0xFFFFFFUL) {
            odometerX100m++;
        }
    }
}


//TODO:
void registerCEL(char data) {
    if (data>0) {
        
    }
}

//select next eligible sensor and query the ECU
int askDLData() {
    for (; dataIndex < _SENSOR_ENTRY_MAX; dataIndex++) {
        if (isElegible()) {
            break;
        }
    }

    if (dataIndex == _SENSOR_ENTRY_MAX) {
        return 0;
    }

    if (ecuSensors[dataIndex].type == _SENSOR_ENTRY_TYPE_PRESET) {
        sendRS232Data(ecuSensors[dataIndex].address1);
    }
    else if (ecuSensors[dataIndex].type == _SENSOR_ENTRY_TYPE_SPECIFIC_RAM) {
        sendRS232Data(ecuSensors[dataIndex].type);
        sendRS232Data(ecuSensors[dataIndex].address1);
        sendRS232Data(ecuSensors[dataIndex].address2);
    }
    else {
        return 1;
    }

    return 0;
}

//to be called every timeslice (start of new data batch).
//Timeslice size to be defined base on datalogging frequency)
int startDataFetch() {
    //update the fraction of the 1-second frame that we are currently on
    if (timeSlice>=10) { timeSlice = 1;}
    else { timeSlice++; }    


    //TO REMOVE : BUGGY CODE
    //if message is pending to Transmit, reset EUSART
    //if(!TXSTA1bits.TRMT) {
    //    setEUSART();  
    //    dataIndex = _SENSOR_ENTRY_MAX;
    //}
    
    //if previous fetch didn't arrive yet, dont start new one. will be corrupted
    if (dataIndex<_SENSOR_ENTRY_MAX) {
        //if message is pending to be received, wait 3 times then purge ECU
        if (fetch_watchdog<WATCHDOG_LIMIT) { fetch_watchdog++; }
        else { dataIndex=_SENSOR_ENTRY_MAX; }
        
        return 0;
    }
    //Reset watchdog
    fetch_watchdog = 0;

    //reset sensor list. Note: no requests are supposed to be pending
    dataIndex=0;
    
    //iterate thru all possible positions
    askDLData();
    
    return 0;
}