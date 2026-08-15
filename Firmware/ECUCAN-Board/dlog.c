#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "eeprom.h"
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
//increments by 1 with every 200 microseconds
unsigned int INJ_sampling_interval = 0;
unsigned int VSS_sampling_interval = 0;
//trip and total distance counter in multiples of 10m
unsigned long tripAx10m = 0;
unsigned long tripBx10m = 0;
unsigned long tripCx10m = 0;
unsigned long totalx10m = 0;

unsigned long tripAx1ml = 0;
unsigned long tripBx1ml = 0;
unsigned long tripCx1ml = 0;

//incremental distance counter in cm
unsigned int currentDistancex1cm = 0;

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
//datalogger entry:
// DL command (0= disabled, 1=read RAM, 2=write RAM, 3=read ROM, 4=write ROM), ADDmsB, ADDlsB, every X polling period(s) (i.e. 1 = always, 2 = every 2 periods, 5 = every 5 periods, up to 10), sensor_value
unsigned char datalogger[150] = {
        0x02, 0x10, 0x00, 1, 0x00, //RPM LSB
        0x02, 0x11, 0x00, 1, 0x00, //RPM MSB
        0x02, 0x14, 0x00, 1, 0x00, //MAP
        0x02, 0x15, 0x00, 1, 0x00, //TPS
        0x02, 0x17, 0x00, 2, 0x00, //Injector LSB
        0x02, 0x18, 0x00, 2, 0x00, //injector LSB
        0x02, 0x1B, 0x00, 2, 0x00, //IAT
        0x02, 0x1C, 0x00, 1, 0x00, //VSS
        0x02, 0x1D, 0x00, 2, 0x00, //ECT
        0x02, 0x1E, 0x00, 5, 0x00, //PA (not available)
        0x02, 0x1F, 0x00, 5, 0x00, //IACV / BAT
        0x02, 0x20, 0x00, 1, 0x00, //O2
        0x02, 0x21, 0x00, 1, 0x00, //P0
        0x02, 0x22, 0x00, 1, 0x00, //P1
        0x02, 0x23, 0x00, 1, 0x00, //INPUT1
        0x02, 0x24, 0x00, 1, 0x00, //INPUT2
        0x02, 0x25, 0x00, 5, 0x00, //CEL W1 B1
        0x02, 0x26, 0x00, 5, 0x00, //CEL W1 B2
        0x02, 0x27, 0x00, 5, 0x00, //CEL W2 B1
        0x02, 0x28, 0x00, 5, 0x00, //CEL W2 B2
        0x01, 0x03, 0xC6, 5, 0x00, //ELD (not available)
        0x01, 0x03, 0xC5, 5, 0x00, //Battery/IACV
        0x01, 0x02, 0x4F, 1, 0x00, //Gear
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
        0x00, 0x00, 0x00, 0, 0x00, //undefined
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
    //if normal sensor
    if (datalogger[dataIndex* (unsigned)_SENSOR_ENTRY_SIZE]!=0x00) {
        //if sensor sampling period is according to this timeslice 
        if ((timeSlice)%datalogger[dataIndex*(unsigned)_SENSOR_ENTRY_SIZE+(unsigned)_SENSOR_ENTRY_PERIOD_OFFSET] == 0) { return 1; }
        else { return 0; }
    }
    else { return 0; }
}

void load_bits(unsigned char sensor, char answer) {
    switch (sensor) {
            case _DL_P0: 
                bitsized_data[_DL_BIT_ACC] = (answer >> _DL_BIT_ACC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_PCS] = (answer >> _DL_BIT_PCS_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_ALTC] = (answer >> _DL_BIT_ALTC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_FANC] = (answer >> _DL_BIT_FANC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_IAB] = (answer >> _DL_BIT_IAB_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_FLR] = (answer >> _DL_BIT_FLR_OFFSET) & 0x01;
                break;
            case _DL_P1: 
                bitsized_data[_DL_BIT_VTEC1] = (answer >> _DL_BIT_VTEC1_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_VTEC2] = (answer >> _DL_BIT_VTEC2_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_MIL] = (answer >> _DL_BIT_MIL_OFFSET) & 0x01;
                break;
            case _DL_INPUT1: 
                bitsized_data[_DL_BIT_PWRSTEER] = (answer >> _DL_BIT_PWRSTEER_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_SERVCON] = (answer >> _DL_BIT_SERVCON_OFFSET) & 0x01;
                break;
            case _DL_INPUT2: 
                bitsized_data[_DL_BIT_STARTER] = (answer >> _DL_BIT_STARTER_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_VTP] = (answer >> _DL_BIT_VTP_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_AC] = (answer >> _DL_BIT_AC_OFFSET) & 0x01;
                bitsized_data[_DL_BIT_BRAKE] = (answer >> _DL_BIT_BRAKE_OFFSET) & 0x01;
                break;
    };
}

//store sensor information received from ECU
int getDLData(char answer) {    
    unsigned char sensor = (unsigned char) (dataIndex*_SENSOR_ENTRY_SIZE+_SENSOR_ENTRY_DATA_OFFSET); 
    //Test
    datalogger[sensor] = answer;
        
    //handle special sensors
    switch (sensor) {
        case _DL_VSS:  
            calcDistance(answer); break;
        case _DL_INJ_HB: 
            calcConsumption(answer); break;
        case _DL_P0: 
        case _DL_P1: 
        case _DL_INPUT1: 
        case _DL_INPUT2: 
            load_bits(sensor,answer); break;
        case _DL_CEL1: 
        case _DL_CEL2: 
        case _DL_CEL3: 
        case _DL_CEL4: registerCEL(answer); break;
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

void calcDistance(char data) {
    unsigned char * ta;
    unsigned char * tb;
    unsigned char * tc;
    
    if (data==0x00) { VSS_sampling_interval=0; }
    else {
        if (VSS_sampling_interval>1) {
            float time = 0.2*(VSS_sampling_interval-1);  //calc time elapsed (in ms) since last VSS sample. 0.2ms ISR
            float distance = (time*data)/36;  //calc distance in cm based on speed sampled. /36 (cm/ms) in 1km/h
            //ADD distance to trip and total counters

            currentDistancex1cm = currentDistancex1cm + (unsigned int) distance;
            
            if (currentDistancex1cm>1000) {
                currentDistancex1cm = currentDistancex1cm - 1000;
                tripAx10m = tripAx10m + 1;
                tripBx10m = tripBx10m + 1;
                tripCx10m = tripCx10m + 1;
                totalx10m = totalx10m + 1;

                //TO_DO: remove this when you fix trip calculation
                //storeDistance();
            }
        }
        VSS_sampling_interval = 1;
    }
}

void storeDistance(void) {
    //aux vars
    unsigned char a;
    unsigned char b;
    unsigned char c;
    unsigned char d;

    a = (unsigned char)(totalx10m & 0xFFUL);
    b = (unsigned char)((totalx10m >> 8) & 0xFFUL);
    c = (unsigned char)((totalx10m >> 16) & 0xFFUL);
    d = (unsigned char)((totalx10m >> 24) & 0xFFUL);
    
    write_octet_eep(_DL_EEPROM_ADD_DIST1, a);
    write_octet_eep(_DL_EEPROM_ADD_DIST2, b);
    write_octet_eep(_DL_EEPROM_ADD_DIST3, c);    
    write_octet_eep(_DL_EEPROM_ADD_DIST4, d);             
}

void loadDistance(void) { 
    //aux vars
    char a;
    char b;
    char c;
    char d;

    d = read_octet_eep(_DL_EEPROM_ADD_DIST4);
    a = read_octet_eep(_DL_EEPROM_ADD_DIST3);
    b = read_octet_eep(_DL_EEPROM_ADD_DIST2);
    c = read_octet_eep(_DL_EEPROM_ADD_DIST1);
    
    totalx10m = d;
    totalx10m = totalx10m<<8;
    totalx10m = totalx10m + a;
    totalx10m = totalx10m<<8;
    totalx10m = totalx10m + b;
    totalx10m = totalx10m<<8;
    totalx10m = totalx10m + c;

    return;
}

void resetDistance(void) {
    write_octet_eep(_DL_EEPROM_ADD_DIST1, 0x00);
    write_octet_eep(_DL_EEPROM_ADD_DIST2, 0x00);
    write_octet_eep(_DL_EEPROM_ADD_DIST3, 0x00);
    write_octet_eep(_DL_EEPROM_ADD_DIST4, 0x00);
    return;
}


void calcConsumption(char data) {
    int RPM_tmp = (datalogger[_DL_RPM_HB]<<8)+datalogger[_DL_RPM_LB];
    int inj_ms = (datalogger[_DL_INJ_HB]<<8)+datalogger[_DL_INJ_LB];
    if (RPM_tmp>0) {
        int RPM = (int) 1875000/RPM_tmp;
        if (INJ_sampling_interval>0) {
            float time = 0.2*INJ_sampling_interval;  //calc time elapsed (in ms) since last VSS sample. 0.2ms ISR
            INJ_sampling_interval = 0;

            //ml/ms of open injector:X = 240cc/min injectors / 60000 (ms/min))
            //ml/min @ current RPM and throtle opening: X = X * (RPM) * 0.5squirts per rotation * 4 injectors   * ms per squirt (inj_ms) 
            //ml in current sampling period: X = X/60000 * sampling period in ms (time)
            float fuelx1ml = RPM*inj_ms*time*0.008/60000; 
                
            tripAx1ml = tripAx1ml + fuelx1ml;
            tripBx1ml = tripBx1ml + fuelx1ml;
            tripCx1ml = tripCx1ml + fuelx1ml;
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
    char sensor_pos;
    for (;dataIndex<_SENSOR_ENTRY_MAX;dataIndex++) { if (isElegible()) { 
        sensor_pos = dataIndex*(unsigned)_SENSOR_ENTRY_SIZE;
        break; 
    } }
    
    if (dataIndex==_SENSOR_ENTRY_MAX) { return 0; } 
    
    //if eligible and standard command
      if (datalogger[(unsigned char)(sensor_pos+_SENSOR_ENTRY_TYPE_OFFSET)]==_SENSOR_ENTRY_TYPE_PRESET) {
        //get Sensor Data
        sendRS232Data(datalogger[sensor_pos+(unsigned)_SENSOR_ENTRY_ADD1_OFFSET]);
    }
    else if (datalogger[(unsigned char)(sensor_pos+_SENSOR_ENTRY_TYPE_OFFSET)]==_SENSOR_ENTRY_TYPE_SPECIFIC_RAM) 
    {
        //get Sensor Data, specify RAM ADDRESS FETCH MODE
        sendRS232Data(datalogger[sensor_pos+(unsigned)_SENSOR_ENTRY_TYPE_OFFSET]);
        //get Sensor Data, specify RAM ADDRESS MSB
        sendRS232Data(datalogger[sensor_pos+(unsigned)_SENSOR_ENTRY_ADD1_OFFSET]);
        //get Sensor Data, specify RAM ADDRESS LSB
        sendRS232Data(datalogger[sensor_pos+(unsigned)_SENSOR_ENTRY_ADD2_OFFSET]); 
    }
    else { return 1; }
    
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