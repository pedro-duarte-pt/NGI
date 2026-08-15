// CONFIG1L
#pragma config RETEN = OFF      // VREG Sleep Enable bit (Ultra low-power regulator is Disabled (Controlled by REGSLP bit))
#pragma config INTOSCSEL = HIGH // LF-INTOSC Low-power Enable bit (LF-INTOSC in High-power mode during Sleep)
#pragma config SOSCSEL = HIGH   // SOSC Power Selection and mode Configuration bits (High Power SOSC circuit selected)
#pragma config XINST = OFF      // Extended Instruction Set (Disabled)

// CONFIG1H
#pragma config FOSC = HS1       // Oscillator (HS oscillator (Medium power, 4 MHz - 16 MHz))
#pragma config PLLCFG = OFF     // PLL x4 Enable bit (Disabled)
#pragma config FCMEN = OFF      // Fail-Safe Clock Monitor (Disabled)
#pragma config IESO = OFF       // Internal External Oscillator Switch Over Mode (Disabled)

// CONFIG2L
#pragma config PWRTEN = OFF     // Power Up Timer (Disabled)
#pragma config BOREN = SBORDIS  // Brown Out Detect (Enabled in hardware, SBOREN disabled)
#pragma config BORV = 3         // Brown-out Reset Voltage bits (1.8V)
#pragma config BORPWR = ZPBORMV // BORMV Power level (ZPBORMV instead of BORMV is selected)

// CONFIG2H
#pragma config WDTEN = OFF      // Watchdog Timer (WDT disabled in hardware; SWDTEN bit disabled)
#pragma config WDTPS = 1048576  // Watchdog Postscaler (1:1048576)

// CONFIG3H
#pragma config CANMX = PORTB    // ECAN Mux bit (ECAN TX and RX pins are located on RB2 and RB3, respectively)
#pragma config MSSPMSK = MSK7   // MSSP address masking (7 Bit address masking mode)
#pragma config MCLRE = ON       // Master Clear Enable (MCLR Enabled, RE3 Disabled)

// CONFIG4L
#pragma config STVREN = ON      // Stack Overflow Reset (Enabled)
#pragma config BBSIZ = BB2K     // Boot Block Size (2K word Boot Block size)

// CONFIG5L
#pragma config CP0 = OFF        // Code Protect 00800-01FFF (Disabled)
#pragma config CP1 = OFF        // Code Protect 02000-03FFF (Disabled)
#pragma config CP2 = OFF        // Code Protect 04000-05FFF (Disabled)
#pragma config CP3 = OFF        // Code Protect 06000-07FFF (Disabled)

// CONFIG5H
#pragma config CPB = OFF        // Code Protect Boot (Disabled)
#pragma config CPD = OFF        // Data EE Read Protect (Disabled)

// CONFIG6L
#pragma config WRT0 = OFF       // Table Write Protect 00800-01FFF (Disabled)
#pragma config WRT1 = OFF       // Table Write Protect 02000-03FFF (Disabled)
#pragma config WRT2 = OFF       // Table Write Protect 04000-05FFF (Disabled)
#pragma config WRT3 = OFF       // Table Write Protect 06000-07FFF (Disabled)

// CONFIG6H
#pragma config WRTC = OFF       // Config. Write Protect (Disabled)
#pragma config WRTB = OFF       // Table Write Protect Boot (Disabled)
#pragma config WRTD = OFF       // Data EE Write Protect (Disabled)

// CONFIG7L
#pragma config EBTR0 = OFF      // Table Read Protect 00800-01FFF (Disabled)
#pragma config EBTR1 = OFF      // Table Read Protect 02000-03FFF (Disabled)
#pragma config EBTR2 = OFF      // Table Read Protect 04000-05FFF (Disabled)
#pragma config EBTR3 = OFF      // Table Read Protect 06000-07FFF (Disabled)

// CONFIG7H
#pragma config EBTRB = OFF      // Table Read Protect Boot (Disabled)

// #pragma config statements should precede project file includes.

//set to 1 to skip datalogger presence validation
#define DEBUG_ON 0

#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "system.h"
#include "rs232.h"
#include "timers.h"
#include "dlog.h"
#include "canopen/CO_MAIN.H"
#include "canopen/ECUCAN_App.h"

volatile unsigned char RXbyte_received;
volatile unsigned char RXbyte_value;
volatile unsigned char startDataFetch_flag;
volatile unsigned char processCO_Timed_events_flag;
unsigned char CANERR_resolved = 1;
volatile unsigned long msCOUNTER;

void enableInterrupts() {
    INTCONbits.RBIE = 0;            //disable PortB interrupts
    RCONbits.IPEN = 0;              //disable interrupts priority levels
	INTCONbits.GIE = 1;             //Enables all interrupt sources
	INTCONbits.PEIE = 1;            //enables all peripheral interrupt sources
}

void __interrupt() checkInterrupts(void) {
    char rcbuf;

    //200 us timer interrupt for VSS and fuel consumption calc
    if (TMR2IE && TMR2IF) {
        TMR2IF = 0;
        
        //TMR2 = 105;     //Defined manually to reduce error. TODO: Have to understand the error!!
        TMR2 = TMR2_load; 
        
        INJ_sampling_interval = INJ_sampling_interval+1;
        if (VSS_sampling_interval>0) {
            VSS_sampling_interval = VSS_sampling_interval+1;
            msCOUNTER = msCOUNTER +1;
        }
    }   
    
    //1 second timer interrupt for led blinking; ECU status check and macroevent
    if (TMR0IE && TMR0IF) {
        TMR0IF = 0;
        TMR0H = TMR0H_load;           //counter for 1 second High Byte
        TMR0L = TMR0L_load;           //counter for 1 second Low Byte  
        
        checkButtonPresure();
        setLEDS();                                      //if DEVICE is WAITING FOR DATALOGGER
        if(deviceStatus == _DEV_READY) { 
            while(!TRMT1);
            //send the data
            TXREG = 0xAB;
        }   
        else{
            if(deviceStatus == _DEV_ON) { 
                //choose what to do based on time elapsed and operating mode
                TPDOEvent(_MACROEVENT);
            }
        }
    } 
    
    //8 ms timer interrupt - for canopen stack periodic tasks
    if (TMR1IE && TMR1IF) {
        TMR1IF = 0;
        
        TMR1H = TMR1H_load;           //load counter for timer's High Byte
        TMR1L = TMR1L_load;           //load counter for timer's Low Byte  
        
        processCO_Timed_events_flag = 1;
    }   
    
    //40ms timer interrupt - for the start of each datalogging fetch and microevent
    if (TMR3IE && TMR3IF) {
        TMR3IF = 0;
        
        TMR3H = TMR3H_load;           //counter for timer's High Byte
        TMR3L = TMR3L_load;           //counter for timer's Low Byte  
        
        if(deviceStatus == _DEV_ON) { 
            //choose what to do based on time elapsed and operating mode
            TPDOEvent(_MICROEVENT);
            startDataFetch_flag = 1;
        }
    }     
    
    
    // EUSART1 receive.
    // The ECU protocol is request/response, so one response byte is expected
    // for each outstanding request. Keep the ISR minimal and defer processing
    // to the main loop.
    if (RCIE && RCIF) {
        if (RCSTA1bits.OERR) {
            CREN1 = 0;
            CREN1 = 1;
        }

        rcbuf = RCREG;

        if (!RCSTA1bits.FERR) {
            if (deviceStatus == _DEV_READY) {
                if (DEBUG_ON) {
                    deviceStatus = _DEV_ON;
                    operatingMode = _MODE_DL;
                }
                else {
                    checkECU(rcbuf);
                }
            }
            else if (deviceStatus == _DEV_ON) {
                RXbyte_value = (unsigned char)rcbuf;
                RXbyte_received = 1;
            }
        }
    }
    
    //if button was pressed or released
    if (RBIE && RBIF) {
        if(PORTB) {};           //clear port B
        checkButtonAction();    //at least 1 seconds has passed since button press:
        RBIF = 0;               //clear flag
    }
    
    if (PIR5bits.IRXIF) {
        PIR5bits.IRXIF = 0;
        if ((TXB1CONbits.TXERR||TXB0CONbits.TXERR||TXB2CONbits.TXERR)&&(deviceStatus == _DEV_ON)) {
            deviceStatus = _DEV_CANERR;
        }
    }
    
    //IF sucessful transmission and current status is Can error
    if ((PIR5bits.TXB0IF||PIR5bits.TXB1IF||PIR5bits.TXB2IF)&&(deviceStatus == _DEV_CANERR)) {
        PIR5bits.TXB0IF = PIR5bits.TXB1IF = PIR5bits.TXB2IF = 0;
        deviceStatus = _DEV_ON; //change status
    }
}

void main(void) {    
    msCOUNTER = 0;
    
    //Startup device
    deviceStatus = _DEV_SETUP;
    deviceErrorCode = _DEV_ERROR_NOERROR;
    
    //call configuration routines
    configureDevice();
    
    //if initial configuration returns error
    if (deviceErrorCode != _DEV_ERROR_NOERROR) { 
        deviceStatus = _DEV_ERROR;
        //setLEDS(); //<== instead of calling setLEDs, rewrite it (compiler otimization recommendation, trumps code reuse..)
        _LED_YELLOW = _LED_ON; _LED_GREEN = _LED_OFF;
        while(1) { ClrWdt(); } 
    }

    //if initial configuration goes OK, device is ready
    deviceStatus = _DEV_READY;
    
    //and waiting for datalogger.
    if(DEBUG_ON) {
        deviceStatus = _DEV_ON;
    }
    else {
        while(deviceStatus!=_DEV_ON) { ClrWdt(); }
    }
    //When datalogger device is found: STATUS=ON, breaking this deadlock
    //this status change is done during ISR
    //deviceStatus = _DEV_ON;
    
    //testing
    //resetDistance();
    
    // Initialize the ECU reader
	ECUCAN_Initialize();					    
    
	while(1)
	{
        //clear WatchDog
        ClrWdt();

        //process received data from the ECU
        if (RXbyte_received == 1) {
            RXbyte_received = 0;
            handleByte(RXbyte_value); //process RS232 receive event in datalogging module
        }
        
		// Process CANopen timed events
        if (processCO_Timed_events_flag == 1) {
            processCO_Timed_events_flag = 0;
            //mCO_ProcessAllTimeEvents();
        }
        
        //start a new sensor polling
        if (startDataFetch_flag == 1) {
            startDataFetch_flag = 0;
            startDataFetch();
        }
        
		// Process CANopen events
		mCO_ProcessAllEvents();		
		
		// Process application specific functions
		ECUCAN_ProcessEvents();		
	}
}