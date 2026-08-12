
#ifndef DATALOGGER_H
#define	DATALOGGER_H

#define _SENSOR_ENTRY_SIZE 5
#define _SENSOR_ENTRY_TYPE_OFFSET 0
#define _SENSOR_ENTRY_ADD1_OFFSET 1
#define _SENSOR_ENTRY_ADD2_OFFSET 2
#define _SENSOR_ENTRY_PERIOD_OFFSET 3
#define _SENSOR_ENTRY_DATA_OFFSET 4

#define _SENSOR_ENTRY_MAX 29

#define _SENSOR_ENTRY_TYPE_PRESET (unsigned char) 0x02
#define _SENSOR_ENTRY_TYPE_SPECIFIC_RAM 0x01

#define _SAMPLING_PERIOD 0.2		//sampling interval in ms

#define _DL_RPM_LB 4
#define _DL_RPM_HB 9
#define _DL_MAP 14
#define _DL_TPS 19
#define _DL_INJ_LB 24
#define _DL_INJ_HB 29
#define _DL_IAT 34
#define _DL_VSS 39
#define _DL_ECT 44
#define _DL_PA 49
#define _DL_IAC 54
#define _DL_O2 59
#define _DL_P0 64
#define _DL_P1 69
#define _DL_INPUT1 74
#define _DL_INPUT2 79
#define _DL_CEL1 84
#define _DL_CEL2 89
#define _DL_CEL3 94
#define _DL_CEL4 99
#define _DL_ELD 104
#define _DL_BAT 109
#define _DL_GEAR 114

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
extern unsigned char datalogger[];
extern unsigned char bitsized_data[];
extern unsigned int INJ_sampling_interval;
extern unsigned int VSS_sampling_interval;
extern unsigned long totalx10m;

#endif	/* DATALOGGER_H */