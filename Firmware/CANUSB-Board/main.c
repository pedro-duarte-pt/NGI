// CONFIG1L
#pragma config CPUDIV = NOCLKDIV// CPU System Clock Selection bits (No CPU System Clock divide)
#pragma config USBDIV = ON      // USB Clock Selection bit (USB clock comes from the OSC1/OSC2 divided by 2)

// CONFIG1H
#pragma config FOSC = HS        // Oscillator Selection bits (HS oscillator)
#pragma config PLLEN = ON       // 4 X PLL Enable bit (Oscillator multiplied by 4)
#pragma config PCLKEN = ON      // Primary Clock Enable bit (Primary clock enabled)
#pragma config FCMEN = OFF      // Fail-Safe Clock Monitor Enable (Fail-Safe Clock Monitor disabled)
#pragma config IESO = OFF       // Internal/External Oscillator Switchover bit (Oscillator Switchover mode disabled)

// CONFIG2L
#pragma config PWRTEN = OFF     // Power-up Timer Enable bit (PWRT disabled)
#pragma config BOREN = SBORDIS  // Brown-out Reset Enable bits (Brown-out Reset enabled in hardware only (SBOREN is disabled))
#pragma config BORV = 19        // Brown-out Reset Voltage bits (VBOR set to 1.9 V nominal)

// CONFIG2H
#pragma config WDTEN = ON       // Watchdog Timer Enable bit (WDT is always enabled. SWDTEN bit has no effect.)
#pragma config WDTPS = 32768    // Watchdog Timer Postscale Select bits (1:32768)

// CONFIG3H
#pragma config HFOFST = ON      // HFINTOSC Fast Start-up bit (HFINTOSC starts clocking the CPU without waiting for the oscillator to stablize.)
#pragma config MCLRE = ON       // MCLR Pin Enable bit (MCLR pin enabled; RA3 input pin disabled)

// CONFIG4L
#pragma config STVREN = ON      // Stack Full/Underflow Reset Enable bit (Stack full/underflow will cause Reset)
#pragma config LVP = OFF         // Single-Supply ICSP Enable bit (Single-Supply ICSP enabled)
#pragma config BBSIZ = OFF      // Boot Block Size Select bit (512W boot block size)
#pragma config XINST = OFF      // Extended Instruction Set Enable bit (Instruction set extension and Indexed Addressing mode disabled (Legacy mode))

// CONFIG5L
#pragma config CP0 = OFF        // Code Protection bit (Block 0 not code-protected)
#pragma config CP1 = OFF        // Code Protection bit (Block 1 not code-protected)

// CONFIG5H
#pragma config CPB = OFF        // Boot Block Code Protection bit (Boot block not code-protected)
#pragma config CPD = OFF        // Data EEPROM Code Protection bit (Data EEPROM not code-protected)

// CONFIG6L
#pragma config WRT0 = OFF       // Table Write Protection bit (Block 0 not write-protected)
#pragma config WRT1 = OFF       // Table Write Protection bit (Block 1 not write-protected)

// CONFIG6H
#pragma config WRTC = OFF       // Configuration Register Write Protection bit (Configuration registers not write-protected)
#pragma config WRTB = OFF       // Boot Block Write Protection bit (Boot block not write-protected)
#pragma config WRTD = OFF       // Data EEPROM Write Protection bit (Data EEPROM not write-protected)

// CONFIG7L
#pragma config EBTR0 = OFF      // Table Read Protection bit (Block 0 not protected from table reads executed in other blocks)
#pragma config EBTR1 = OFF      // Table Read Protection bit (Block 1 not protected from table reads executed in other blocks)

// CONFIG7H
#pragma config EBTRB = OFF      // Boot Block Table Read Protection bit (Boot block not protected from table reads executed in other blocks)

// #pragma config statements should precede project file includes.
// Use project enums instead of #define for ON and OFF.

#include <xc.h>
#include "mcp2515.h"
#include "system.h"
#include "usb/usb_config.h"
#include "app_device_vendor_basic.h"
#include "app_led_usb_status.h"
#include "usb/usb.h"
#include "usb/usb_device.h"
#include "usb/usb_device_generic.h"

//global var to avoid reentrant functions
volatile unsigned char CANint = 0;

void SPI_config() {
    //set data direction for ports in SPI communication
    //RB4=Data In (I); RC7=Data Out (O); RB6=SPI clock (O); RC2=Chip Select (O))
    TRISBbits.TRISB4 = 1; //redundant, SDI is automatically controlled by the SPI module
    ANSELHbits.ANS10 = 0; //enable Digital input buffer of RB4
    TRISCbits.TRISC7 = 0; //SDO must have corresponding TRIS bit cleared
    TRISBbits.TRISB6 = 0; //SCK (Master mode) must have corresponding TRIS bit cleared
    TRISCbits.TRISC2 = 0; //CS
    TRISCbits.TRISC1 = 0; //RESET

    //set SPI protocol settings
    SSPSTATbits.SMP = 0; //Input data is sampled at the middle of data output time
    SSPSTATbits.CKE = 1; //Transmit occurs on transition from active to Idle clock state
    SSPCON1bits.CKP = 0; //
    SSPCON1bits.SSPM = 1; //SPI Master mode: Clock = FOSC/16 = 4MHz
    
    //activate SPI Module
    SPI_CS = 1;
    SPI_RESET = 1;
    SSPCON1bits.SSPEN = 1;
}

char SPI_writebByte(char data) {
    SSPCON1bits.WCOL = 0; // Clear the Write Collision flag, to allow writing;
    SSPBUF = data;    //send data
    while( !SSPSTATbits.BF ); // wait until 'BF' bit is set
    return SSPBUF;
}

void startStatusLEDTimer(void) {
    //configure 1 second timer
    T0CON = 0x07; //stop timer 0
    INTCONbits.TMR0IF = 0;
    INTCONbits.TMR0IE = 1;
    TMR0H = 0x44;  //load value for 1 sec in timer counter
    TMR0L = 0x7F;  //load value for 1 sec in timer counter
    T0CON = 0x87; //start timer in 16bit mode and with 256 prescaler  
}

void SPI_write(char data[], unsigned data_len) {
    for (unsigned i=0; i<data_len; i++) {
        SPI_writebByte(data[i]);
    }
}

void setUpDevice() {
    //disable all interrupts
    INTCON = 0b00000000; //Disable all interrupts;

    //configure IO ports 
    TRISCbits.RC3 = 0;      //POWER LED
    TRISCbits.RC4 = 0;      //STATUS LED
    TRISCbits.RC5 = 0;      //ERROR LED
    TRISBbits.TRISB5 = 1;   //configure as inputs for MCP2515 interrupt detection (RX0)
    TRISBbits.TRISB7 = 1;   //configure as inputs for MCP2515 interrupt detection (RX1)

    LATCbits.LATC3 = LED_OFF; //led off
    LATCbits.LATC4 = LED_OFF; //led off
    LATCbits.LATC5 = LED_OFF; //led off    
    
     //configure SPI
    SPI_config();
    
    //Configure MCP2515
    MCP2515_config();   
    
    //configure Interrupts
    ANSELHbits.ANS11 = 0;   //enable Digital input buffer of RB5
    INTCON2bits.RABPU = 0;  //enable pull-ups on RA & RB
    IOCA=0;                 //none of Port A is to be monitored for interrupts
    WPUB = 0b10100000;      //enable pull-ups on RB5 & RB7
    IOCB = 0b10100000;      //ativate interrupts on RB5 and RB7 only
    RCONbits.IPEN = 0;      //disable interrupts priority levels
    INTCON = 0b11001000;    //Enable Global, Peripheral and IOC (RA & RB) interrupts; no priority;
}

void __interrupt() checkInterrupts(void)
{
    char temp;

    if (INTCONbits.TMR0IE && INTCONbits.TMR0IF) {
        INTCONbits.TMR0IE = 0;
        INTCONbits.TMR0IF = 0;
        TMR0H = 0x44;       //load value for 1 sec in timer counter
        TMR0L = 0x7F;       //load value for 1 sec in timer counter
        
        POWER_LED = (unsigned) !POWER_LED;
        INTCONbits.TMR0IE = 1;

        if (MCP2515_CANERR) { 
            MCP2515_CANERR = 0; 
            ERROR_LED = 0;
        }
    }   
    
    if (INTCONbits.RABIF) {
        temp = PORTB;
        //clears the RABIF
        INTCONbits.RABIF = 0;     
        
         if(!PORTBbits.RB7 || !PORTBbits.RB5) {           
            //if either RXBF0 (RB5) or RXBF1 (RB7) on the MCP2515 were triggered:
            CANint = 1;
        }
    }
    
        #if defined(USB_INTERRUPT)
            USBDeviceTasks();
        #endif
}

void main(void) {
    
    setUpDevice(); 
    USBDeviceInit();
    USBDeviceAttach();
    
    startStatusLEDTimer();
    
    CANint = 0;
    MCP2515_received = false; 
    
    while (1) {
        //clear WatchDog
        ClrWdt();        
        
        //check for MCP2515 interrupt
        if (CANint) {
            CANint = 0;
            MCP2515_processInterrupt();
        }  

        if(MCP2515_received) {
            MCP2515_received = false;
            unsigned char i;
            
            

            for (i=0; i<13; i++) { INPacket[i+1] = MCP2515_message[i]; }
            for (i=14; i<USBGEN_EP_SIZE; i++) { INPacket[i] = 0x00; }
           
            /*
            if ((loaded_bytes+13)<USBGEN_EP_SIZE) {
                char i = 0;
                while(i<13) {
                    loaded_bytes = loaded_bytes+1;
                    //INPacket[loaded_bytes] = MCP2515_message[i];
                }
            }
            else {
                //USB master polling frequency unsuficient!
                //ERROR_LED =  LED_ON;
            }
            */
        }      

        

        #if defined(USB_POLLING)
            USBDeviceTasks();
        #endif        
            
        //Application specific tasks
        APP_DeviceVendorBasicDemoTasks();     
    
    }
}
