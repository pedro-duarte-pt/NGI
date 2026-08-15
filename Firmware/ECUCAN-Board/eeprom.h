#ifndef EEPROM_H
#define EEPROM_H

/* PIC18F25K80 data EEPROM: 1024 bytes (0x000-0x3FF). */
void write_octet_eep(unsigned int address, unsigned char data);
unsigned char read_octet_eep(unsigned int address);

#endif /* EEPROM_H */
