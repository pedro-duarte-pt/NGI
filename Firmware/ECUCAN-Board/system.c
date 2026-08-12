#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "main.h"
#include "rs232.h"
#include "system.h"
#include "timers.h"
#include "dlog.h"
#include "canopen.h"

_DEV_STATUS deviceStatus = _DEV_SETUP;    
_DEV_ERROR_CODE deviceErrorCode = _DEV_ERROR_NOERROR;  
_DEV_OPERATING_MODE operatingMode;
int buttonHeldFor = -1;         //how many second is the button being held for
//to be called upon release of 4 seconds btn push event

void configureDevice() {    
    //set PLL and CLK configuration
    setClock();
            
    //setup LEDs
    configureLEDs();
    
    //Setup buttons
    configureButton();
    
    //Setup Datalogger
    startDL();
    
    //Setup EUSART 
    setEUSART();
    //TO-DO: Check return code for errors

    //configureTimers();    
    configureTimers();
    
    //Setup CAN
    setCANOPEN();    
    
    //enable interuupts
    enableInterrupts();
}

void setClock() {
    OSCTUNEbits.PLLEN = 1;  //enabled 4xPPL. 4*8 = 32MHz
    //if error then deviceErrorCode = _DEV_ERROR_CLOCKSET;
}

void setLEDS() {
    switch(deviceStatus) {
        case _DEV_SETUP: _LED_YELLOW = _LED_GREEN; _LED_GREEN = (unsigned) !_LED_GREEN;  break;
        case _DEV_READY: _LED_YELLOW = _LED_OFF; _LED_GREEN = (unsigned) !_LED_GREEN;  break;
        case _DEV_ON: _LED_YELLOW = _LED_OFF; _LED_GREEN = _LED_ON;  break;
        case _DEV_ERROR: _LED_YELLOW = _LED_ON; _LED_GREEN = _LED_OFF; break;
        case _DEV_SLEEP: _LED_YELLOW = (unsigned) !_LED_YELLOW; _LED_GREEN = _LED_OFF;  break;
        case _DEV_CANERR: _LED_YELLOW = _LED_ON; _LED_GREEN = _LED_ON;  break;        
    }
}

void configureLEDs() {
    TRISCbits.TRISC2 = 0;
    TRISCbits.TRISC3 = 0;
    _LED_GREEN = _LED_OFF;
    _LED_YELLOW = _LED_OFF;
}

void configureButton() {
    TRISBbits.TRISB5 = 1;       //set pin 7 as input to receive interrupt
    INTCON2bits.NOT_RBPU = 0;   //enable pull-up function
    WPUBbits.WPUB7 = 1;         //enable pull-ups on specific pin (7)
    IOCBbits.IOCB7 = 1;         //enable interrupt on specific pin (7)
    if(PORTB) {}                //read portB as per datasheet
    RBIF = 0;                   //in order to clear flag
    INTCONbits.RBIE = 1;        //enable IOC 
}

void checkButtonAction() {
        if (buttonHeldFor>0) {
            //button was released
            if (buttonHeldFor>4) {
                resetHW();
            }
            else {
                changeMode();
            }
            buttonHeldFor = -2; // -2 works has a buffer against transients
        }
        else if(buttonHeldFor==-1){
            //button was just pressed, reset button. 0 works has a buffer against transients
            buttonHeldFor = 0;
        }
}

void resetHW() {
    RESET();
}

void checkButtonPresure() {
        //one second has passed and button is pressed, increase button seconds count
        if(buttonHeldFor>=0) { buttonHeldFor++; }
        if(buttonHeldFor<-1) { buttonHeldFor++; }    
}

//to be called upon release of <0.5 seconds btn push event 
void changeMode() {
    switch(operatingMode) {
        case _MODE_DL: operatingMode = _MODE_SC; break;
        case _MODE_SC: operatingMode = _MODE_CM; break;
        case _MODE_CM: operatingMode = _MODE_DL; break;
    }
}
