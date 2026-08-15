/*****************************************************************************
 *
 * Microchip CANopen Stack (Demonstration Object)
 *
 *****************************************************************************
 * FileName:        DEMOOBJ.H
 * Dependencies:    
 * Processor:       PIC18F with CAN
 * Compiler:       	C18 02.30.00 or higher
 * Linker:          MPLINK 03.70.00 or higher
 * Company:         Microchip Technology Incorporated
 *
 * Software License Agreement
 *
 * The software supplied herewith by Microchip Technology Incorporated
 * (the "Company") is intended and supplied to you, the Company's
 * customer, for use solely and exclusively with products manufactured
 * by the Company. 
 *
 * The software is owned by the Company and/or its supplier, and is 
 * protected under applicable copyright laws. All rights are reserved. 
 * Any use in violation of the foregoing restrictions may subject the 
 * user to criminal sanctions under applicable laws, as well as to 
 * civil liability for the breach of the terms and conditions of this 
 * license.
 *
 * THIS SOFTWARE IS PROVIDED IN AN "AS IS" CONDITION. NO WARRANTIES, 
 * WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED 
 * TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT, 
 * IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR 
 * CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
 *
 *
 * 
 * 
 *
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Ross Fosler			11/13/03	...	
 * 
 *****************************************************************************/


// These are mapping constants for TPDO1 
// starting at 0x1A00 in the dictionary
extern const unsigned long uTPDO1Map;
extern const unsigned long uRPDO1Map;
extern const unsigned long uPDO1Dummy;

extern const unsigned char rMaxIndex1;
extern const unsigned char rMaxIndex2;

extern const unsigned long _dict_dl_vss;
extern const unsigned long _dict_dl_inj_low;
extern const unsigned long _dict_dl_inj_hi;
extern const unsigned long _dict_dl_o2;
extern const unsigned long _dict_dl_tps;
extern const unsigned long _dict_dl_rpm_low;
extern const unsigned long _dict_dl_rpm_hi;
extern const unsigned long _dict_dl_map;        
extern const unsigned long _dict_dl_iat;        
extern const unsigned long _dict_dl_ect;        
extern const unsigned long _dict_dl_pa;        
extern const unsigned long _dict_dl_iac;        
extern const unsigned long _dict_dl_p0;        
extern const unsigned long _dict_dl_p1;        
extern const unsigned long _dict_dl_input1;        
extern const unsigned long _dict_dl_input2;        
extern const unsigned long _dict_dl_cel1;        
extern const unsigned long _dict_dl_cel2;        
extern const unsigned long _dict_dl_cel3;        
extern const unsigned long _dict_dl_cel4;        
extern const unsigned long _dict_dl_bat;        
extern const unsigned long _dict_dl_gear;        
extern const unsigned long _dict_dl_eld;  
extern const unsigned long _dict_dl_acc;        
extern const unsigned long _dict_dl_pcs;        
extern const unsigned long _dict_dl_altc;        
extern const unsigned long _dict_dl_fanc;        
extern const unsigned long _dict_dl_iab;        
extern const unsigned long _dict_dl_flr;        
extern const unsigned long _dict_dl_vtec1;        
extern const unsigned long _dict_dl_vtec2;        
extern const unsigned long _dict_dl_mil;        
extern const unsigned long _dict_dl_pwrsteer;        
extern const unsigned long _dict_dl_servcon;        
extern const unsigned long _dict_dl_starter;        
extern const unsigned long _dict_dl_vtp;        
extern const unsigned long _dict_dl_ac;        
extern const unsigned long _dict_dl_brake;     
extern const unsigned long _dict_dl_dist1;     
extern const unsigned long _dict_dl_dist2;     
extern const unsigned long _dict_dl_dist3;      


extern const unsigned char TPDO1_objnum;
extern const unsigned char TPDO1_defnum;
extern const unsigned char TPDO2_objnum;
extern const unsigned char TPDO2_defnum;


void ECUCAN_ProcessEvents(void);
void ECUCAN_Initialize(void);
void CO_COMM_TPDO1_COBIDAccessEvent(void);
void CO_COMM_TPDO2_COBIDAccessEvent(void);
void CO_COMM_TPDO1_TypeAccessEvent(void);
void CO_COMM_TPDO2_TypeAccessEvent(void);

void CO_COMM_RPDO1_COBIDAccessEvent(void);


typedef struct _TPDO
{
    struct _TPDO_status
    {
        unsigned	isQueuedForTX:1;      //for all TPDOs, send on first oportunity
        unsigned	wasTriggered:1;       //for acyclic synch OR Asynch only, set after event was triggered 
        unsigned	isWaitingSynch:1;     //for acyclic synch only, set after event was analysed
    }status;
    
	unsigned char 	synch_setting;          //to store the sub-index 02 of each TPDO communication parameter (1800h to 19FFh)
	unsigned char 	synch_counter;          //to keep track of how many synchs elapsed 
    
    unsigned char uLocalXmtBuffer[8];			// Local buffer for TPDOs
}TPDO;
        
        
extern TPDO TPDOs[];

