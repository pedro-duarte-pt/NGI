#define _BAUD_RATE 38400			//BAUD Rate of the RS232 interface
#define _1REG_LIMIT 255            //number of bits supported by 1 register configuration

#if defined(__XC8)       
 #include <xc.h>        
#endif   

#include "system.h"
#include "rs232.h"

int setEUSART(void) {
  unsigned int x;
  unsigned long _fosc = _XTAL_FREQ;
  RXbyte_received = 0;                //flag to indicate received byte (to avoid load in RX ISR)
  _fosc = _fosc *  (unsigned) OSCTUNEbits.PLLEN*4;
  
  x = ((_fosc/(_BAUD_RATE*16))-1);         //SPBRG for Low Baud Rate
  if(x>_1REG_LIMIT)                             //If High Baud Rage Required
  {
    deviceStatus = _DEV_ERROR;
    deviceErrorCode = _DEV_ERROR_RS232;
    return 1;                                   //return error for now.. Must be upgraded to support 16bits registers
  }
  else
  {
    TXSTAbits.TXEN = 0;                         //disabled TX on EUSART 2 <- NEW
    RCSTA2bits.SPEN = 0;                        //disable EUSART 2 <- NEW
    
    //Set input on eusart 1
    TRISCbits.TRISC7 = 1;                       //Set RX pin as input for EUSART1

    TXSTA1 = 0b00000100;   
    RCSTA1 = 0b00010000;
    BAUDCON1 = 0b00000000;
    SPBRG1 = (unsigned char)x;                                  //Writing baudrate timer Register
    RCSTA1bits.SPEN = 1;                        //ENABLE Reception on EUSART 1
    TXSTA1bits.TXEN = 1;                        //Enables Transmission on EUSART 1

    RCIE = 1;                                   //Activate interrupt flag
    
    return 0;                                   //Returns success
  }
}

void sendRS232Data(char data){
  //wait for register to be available
  while(!TXSTA1bits.TRMT);
  
  //send the data
  TXREG1 = data;
}