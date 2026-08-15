#if defined(__XC8)       
 #include <xc.h>           
#endif   

#include "system.h"
#include "timers.h"
#include "dlog.h"
#include "canopen/CO_DEFS.DEF"

unsigned char TMR0H_load;
unsigned char TMR0L_load;
unsigned char TMR1H_load;
unsigned char TMR1L_load;
unsigned char TMR3H_load;
unsigned char TMR3L_load;

void configureTimers() {
    unsigned long tmp;
    unsigned int TMR_load;

    //configure timer0 for 1s period
    T0CONbits.TMR0ON = 0;       //disable timer 0
    T0CONbits.T08BIT = 0;     //16-bit timer
    T0CONbits.T0CS = 0;       //internal clock
    T0CONbits.T0SE = 0;       //counter edge. this really doesnt matter 
    T0CONbits.PSA = 0;        //prescaller enabled
    T0CONbits.T0PS = 7;       //prescaller value (1:256))
    TMR0IP = 1;            //TMR Overflow Interrupt Priority bit set to High
    TMR0IE = 1;            //enables Timer0 Interrupts
    
        tmp = (unsigned long) (_XTAL_FREQ*(unsigned)(4*OSCTUNEbits.PLLEN)/4/256*((float)_MACROEVENT/1000));
        
        if (tmp>0xFFFF) { 
            deviceErrorCode = _DEV_ERROR_CLOCKSET;
            deviceStatus = _DEV_ERROR;
        }
        TMR_load = 0xFFFF - (unsigned int) tmp;     //fsoc = osc/4 ; prescaler = 1/256

        TMR0H_load = (unsigned char)(TMR_load >> 8);
        TMR0L_load = (unsigned char)(TMR_load & 0xFF);

        TMR0H = TMR0H_load;           //load counter for 1 second High Byte
        TMR0L = TMR0L_load;           //load counter for 1 second Low Byte  
        
        TMR0IF = 0;
        T0CONbits.TMR0ON = 1;       //enable timer
        
    /////////////////// TMR1 ///////////////
    //configure timer1 for 8ms period (canopen periodic tasks)
    T1CONbits.TMR1ON = 0;       //disable timer 1
    T1CONbits.TMR1CS = 0;       //Fosc/4
    T1CONbits.RD16 = 1;
    T1CONbits.T1CKPS = 0b11;  //1:8 prescaler
    T1GCONbits.TMR1GE = 0;
    TMR1IP = 1;            //TMR Overflow Interrupt Priority bit set to High
    TMR1IE = 1;            //enables Timer1 Interrupts
   
        tmp = (unsigned long) ((_XTAL_FREQ*(unsigned)(3*OSCTUNEbits.PLLEN+1)/4/8)*((float)CO_TICK_PERIOD/1000));
        
        if (tmp>0xFFFF) { 
            deviceErrorCode = _DEV_ERROR_CLOCKSET;
            deviceStatus = _DEV_ERROR;        
        }
        TMR_load = 0xFFFF - (unsigned int) tmp;     //fsoc = osc/4 ; prescaler = 1/8 /8ms

        TMR1H_load = (unsigned char)(TMR_load >> 8);
        TMR1L_load = (unsigned char)(TMR_load & 0xFF);      
        
        TMR1H = TMR1H_load;           //load counter for timer's High Byte
        TMR1L = TMR1L_load;           //load counter for timer's Low Byte  
        
        TMR1IF = 0;
        T1CONbits.TMR1ON = 1;       //Enable timer 1  
   

    
    ////////// TMR3 ////////////////
    //configure timer3 for 40ms period (datalogging periodic tasks)
    T3CONbits.TMR3ON = 0;     //disable timer 3
    T3CONbits.TMR3CS = 0;     //Fosc/4
    T3CONbits.RD16 = 1;
    T3CONbits.T3CKPS = 3;  //1:8 prescaler
    T3GCONbits.TMR3GE = 0;
    TMR3IP = 1;            //TMR Overflow Interrupt Priority bit set to High
    TMR3IE = 1;            //enables Timer3 Interrupts
       
    tmp = (unsigned long) ((_XTAL_FREQ/4/8*(unsigned)(3*OSCTUNEbits.PLLEN+1))*((float)_MICROEVENT/1000));
    if (tmp>0xFFFF) { 
        deviceErrorCode = _DEV_ERROR_CLOCKSET; 
        deviceStatus = _DEV_ERROR;
    }
    TMR_load = 0xFFFF - (unsigned int) tmp;     //fsoc = osc/4 ; prescaler = 1/8 /40ms
    
    TMR3H_load = (unsigned char)(TMR_load >> 8);
    TMR3L_load = (unsigned char)(TMR_load & 0xFF);       
    
    TMR3H = TMR3H_load;           //counter for timer's High Byte
    TMR3L = TMR3L_load;           //counter for timer's Low Byte  

    TMR3IF = 0;
    T3CONbits.TMR3ON = 1;       //enable timer 3  
        

}


