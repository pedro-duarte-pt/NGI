#ifndef SYSTEM_H
#define	SYSTEM_H

//HW settings
#define _XTAL_FREQ 12000000L		//XTAL frequency 

//HW IO
#define _LED_GREEN LATCbits.LATC2
#define _LED_YELLOW LATCbits.LATC3
#define _LED_ON 1
#define _LED_OFF 0

//DEVICE STATE MACHINE
 typedef enum {
   _DEV_SETUP, 
   _DEV_READY, 
   _DEV_ON,
   _DEV_SLEEP,
   _DEV_ERROR,
   _DEV_CANERR
 } _DEV_STATUS;

  typedef enum {
   _DEV_ERROR_NOERROR, 
   _DEV_ERROR_CANRATE, 
   _DEV_ERROR_CANOPEN, 
   _DEV_ERROR_CLOCKSET, 
   _DEV_ERROR_HW_PERIPHERALS,
   _DEV_ERROR_RS232,
   _DEV_ERROR_CANOPEN_MAPS
 } _DEV_ERROR_CODE;

  typedef enum {
   _MODE_DL,    //Datalogging operating mode with predefined priorities
   _MODE_SC,    //Specific Command operating mode for legacy applications
   _MODE_CM,     //Costume Modules operating mode for specific tasks,    //Specific Command operating mode for legacy applications
 } _DEV_OPERATING_MODE;
 
extern volatile _DEV_OPERATING_MODE operatingMode;
extern volatile _DEV_STATUS deviceStatus;    
extern _DEV_ERROR_CODE deviceErrorCode;          
extern int buttonHeldFor;

extern volatile unsigned char RXbyte_received;               //flag indicating that a byte was received (to avoid LOAD on ISR, and allow post ISR treatment of the byte)
extern volatile unsigned char RXbyte_value;                  //the value of the received byte
extern volatile unsigned char startDataFetch_flag;           //flag indicating that a new Datafetch is to start (to avoid LOAD on ISR, and allow post ISR initialization of the datafetch)
extern volatile unsigned char processCO_Timed_events_flag;   //flag indicating that 8ms have elapsed. It is time to process CANOPEN timed events

void resetHW(void);

void alternateLEDS(void);

void checkButtonPresure(void);

//to be called upon release of <0.5 seconds btn push event 
void changeMode(void);

void configureTimers(void);

void timedStatusActivities(void);

void configureLEDs(void);

void setLEDS(void);

void configureButton(void);

void checkButtonAction(void);

void setClock(void);

void configureDevice(void);

#endif	/* SYSTEM_H */

