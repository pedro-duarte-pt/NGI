#if defined(__XC8)
#include <xc.h>
#endif

#include "eeprom.h"

#define EEPROM_MAX_ADDRESS 0x03FFU

static void set_eeprom_address(unsigned int address)
{
    EEADRH = (unsigned char)((address >> 8) & 0x03U);
    EEADR = (unsigned char)(address & 0x00FFU);
}

void write_octet_eep(unsigned int address, unsigned char data)
{
    unsigned char gieState;

    if (address > EEPROM_MAX_ADDRESS) {
        return;
    }

    while (EECON1bits.WR) {
        /* Wait for any previous self-timed write to complete. */
    }

    set_eeprom_address(address);
    EEDATA = data;
    EECON1bits.EEPGD = 0;
    EECON1bits.CFGS = 0;
    EECON1bits.WREN = 1;

    /* Only the required unlock/start sequence needs to be atomic. */
    gieState = INTCONbits.GIE;
    INTCONbits.GIE = 0;
    EECON2 = 0x55;
    EECON2 = 0xAA;
    EECON1bits.WR = 1;
    INTCONbits.GIE = gieState;

    while (EECON1bits.WR) {
        /* EEPROM write continues autonomously; interrupts may run here. */
    }

    EECON1bits.WREN = 0;
}

unsigned char read_octet_eep(unsigned int address)
{
    if (address > EEPROM_MAX_ADDRESS) {
        return 0xFFU;
    }

    while (EECON1bits.WR) {
        /* Do not change EEPROM address registers during a write. */
    }

    set_eeprom_address(address);
    EECON1bits.EEPGD = 0;
    EECON1bits.CFGS = 0;
    EECON1bits.RD = 1;

    return EEDATA;
}
