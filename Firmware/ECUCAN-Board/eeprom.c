#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "eeprom.h"

void write_octet_eep(unsigned char address, unsigned char data)
{
    while( EECON1bits.WR  )     // make sure it's not busy with an earlier write.
    {}
    EEADRH = 0x00;      //use only 8bit adddress. drop the 2 extra bits.
    EEADR = address;
    EEDATA = data;
    EECON1bits.EEPGD = 0;   //access eeprom instead of program memory
    EECON1bits.CFGS  = 0;   //Accesses Flash program or data EEPROM memory
    EECON1bits.WREN  = 1;   //Allows write cycles to Flash program/data EEPROM
    INTCONbits.GIE   = 0;
    // required sequence start
    EECON2 = 0x55;
    EECON2 = 0xAA;
    EECON1bits.WR    = 1;   //start Write
    while( EECON1bits.WR  )     // wait for write instruction to end.
    {} 
    // required sequence end
    INTCONbits.GIE   = 1;
    EECON1bits.WREN  = 0;   //Disables write cycles to Flash program/data EEPROM
}

unsigned char read_octet_eep(unsigned char address)
{
    while( EECON1bits.WR  )     // make sure it's not busy with an earlier write.
    {}
    EEADRH = 0x00;      //use only 8bit adddress. drop the 2 extra bits.
    EEADR = address;
    EECON1bits.EEPGD = 0;   //access eeprom instead of program memory
    EECON1bits.CFGS  = 0;   //Accesses Flash program or data EEPROM memory
    EECON1bits.RD    = 1;   //start Read
    return( EEDATA );
}
 
