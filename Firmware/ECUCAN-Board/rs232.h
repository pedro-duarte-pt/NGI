/* 
 * File:   rs232.h
 * Author: ASUS
 *
 * Created on 20 de Julho de 2016, 19:44
 */

#ifndef RS232_H
#define	RS232_H

void sendRS232Data(char);
char checkRS232(void);
int setEUSART(void);
char getRS232Byte();

#endif	/* RS232_H */

