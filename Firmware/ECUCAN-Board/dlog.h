
#ifndef DATALOGGER_H
#define	DATALOGGER_H

typedef struct
{
    unsigned char type;
    unsigned char address1;
    unsigned char address2;
    unsigned char period;
    unsigned char value;
} ECU_SENSOR_ENTRY;

#define _SENSOR_ENTRY_SIZE 5

#define _SENSOR_ENTRY_MAX 29

#define _SENSOR_ENTRY_TYPE_PRESET (unsigned char) 0x02
#define _SENSOR_ENTRY_TYPE_SPECIFIC_RAM 0x01

#define _SAMPLING_PERIOD 0.2		//sampling interval in ms

#define _ECU_SENSOR_RPM_LB   0
#define _ECU_SENSOR_RPM_HB   1
#define _ECU_SENSOR_MAP      2
#define _ECU_SENSOR_TPS      3
#define _ECU_SENSOR_INJ_LB   4
#define _ECU_SENSOR_INJ_HB   5
#define _ECU_SENSOR_IAT      6
#define _ECU_SENSOR_VSS      7
#define _ECU_SENSOR_ECT      8
#define _ECU_SENSOR_PA       9
#define _ECU_SENSOR_IAC      10
#define _ECU_SENSOR_O2       11
#define _ECU_SENSOR_P0       12
#define _ECU_SENSOR_P1       13
#define _ECU_SENSOR_INPUT1   14
#define _ECU_SENSOR_INPUT2   15
#define _ECU_SENSOR_CEL1     16
#define _ECU_SENSOR_CEL2     17
#define _ECU_SENSOR_CEL3     18
#define _ECU_SENSOR_CEL4     19
#define _ECU_SENSOR_ELD      20
#define _ECU_SENSOR_BAT      21
#define _ECU_SENSOR_GEAR     22

#define _DL_BIT_ACC 0
#define _DL_BIT_PCS 1
#define _DL_BIT_ALTC 2
#define _DL_BIT_FANC 3
#define _DL_BIT_IAB 4
#define _DL_BIT_FLR 5
#define _DL_BIT_VTEC1 6
#define _DL_BIT_VTEC2 7
#define _DL_BIT_MIL 8
#define _DL_BIT_PWRSTEER 9
#define _DL_BIT_SERVCON 10
#define _DL_BIT_STARTER 11
#define _DL_BIT_VTP 12
#define _DL_BIT_AC 13
#define _DL_BIT_BRAKE 14

#define _DL_BIT_ACC_OFFSET 0
#define _DL_BIT_PCS_OFFSET 1
#define _DL_BIT_ALTC_OFFSET 2
#define _DL_BIT_FANC_OFFSET 3
#define _DL_BIT_IAB_OFFSET 5
#define _DL_BIT_FLR_OFFSET 7
#define _DL_BIT_VTEC1_OFFSET 0
#define _DL_BIT_VTEC2_OFFSET 1
#define _DL_BIT_MIL_OFFSET 4
#define _DL_BIT_PWRSTEER_OFFSET 3
#define _DL_BIT_SERVCON_OFFSET 7
#define _DL_BIT_STARTER_OFFSET 0
#define _DL_BIT_VTP_OFFSET 1
#define _DL_BIT_AC_OFFSET 2
#define _DL_BIT_BRAKE_OFFSET 4

#define _DL_EEPROM_ADD_DIST1 0
#define _DL_EEPROM_ADD_DIST2 1
#define _DL_EEPROM_ADD_DIST3 2
#define _DL_EEPROM_ADD_DIST4 3


#define WATCHDOG_LIMIT 10

void storeDistance(void);
void resetDistance(void);
void loadDistance(void);
void startDL(void);
void checkECU(char);
void handleByte(char);
void TPDOEvent(int); 
int startDataFetch(void);
int askDLData(void);
int isElegible(void);
int getDLData(char);

void calcDistance(char);
void calcConsumption(char);
void registerCEL(char);
void load_bits(unsigned char, char);

extern unsigned char timeSlice;
extern ECU_SENSOR_ENTRY ecuSensors[];
extern unsigned char bitsized_data[];
extern unsigned int INJ_sampling_interval;
extern unsigned int VSS_sampling_interval;
extern unsigned long totalx10m;

#endif	/* DATALOGGER_H */