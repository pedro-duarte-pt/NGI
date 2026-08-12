#ifndef TIMERS_H
#define	TIMERS_H

#define _MACROEVENT 1000L		//macroevent timer in ms (used for led blinking and slow changing device events)

#define _MICROEVENT 40          //microevent timer in ms (used for rapid changing device events)


void configureTimers(void);

void timedStatusActivities(void);

extern unsigned char TMR0H_load;
extern unsigned char TMR0L_load;
extern unsigned char TMR1H_load;
extern unsigned char TMR1L_load;
extern unsigned char TMR2_load;
extern unsigned char TMR3H_load;
extern unsigned char TMR3L_load;

#endif	/* TIMERS_H */

