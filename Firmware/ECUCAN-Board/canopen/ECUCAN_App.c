/*****************************************************************************
 *
 * Microchip CANopen Stack (Demonstration Object)
 *
 *****************************************************************************
 * FileName:        ECUCAN_App.c
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

#include	"ECUCAN_App.h"
#include    "../dlog.h"
#include	"CO_MAIN.H"
#include	"CO_DEFS.DEF"

#define	RTR_DIS	bytes.B1.bits.b2
#define STD_DIS	bytes.B1.bits.b3
#define PDO_DIS	bytes.B1.bits.b4
#define acyclic 0

// These are mapping constants for TPDO1 
// starting at 0x1A00 in the dictionary
//probably to delete
const unsigned long uTPDO1Map = 0x60000108;         //index 6000; subindex 01; len= 8
const unsigned long uRPDO1Map = 0x62000108;         //index 6200; subindex 01; len= 8
const unsigned long uPDO1Dummy = 0x00000008;

// Static data refered to by the dictionary
//probably to refactor
const unsigned char rMaxIndex1 = 1;
const unsigned char rMaxIndex2 = 8;

const unsigned long _dict_dl_vss = 0x60000108;           //index 6000; subindex 01; len= 8
const unsigned long _dict_dl_inj_low = 0x60000208;       //index 6000; subindex 02; len= 8
const unsigned long _dict_dl_inj_hi  = 0x60000308;        //index 6000; subindex 03; len= 8
const unsigned long _dict_dl_o2  = 0x60000408;            //index 6000; subindex 04; len= 8
const unsigned long _dict_dl_tps = 0x60000508;           //index 6000; subindex 05; len= 8
const unsigned long _dict_dl_rpm_low = 0x60000608;       //index 6000; subindex 06; len= 8
const unsigned long _dict_dl_rpm_hi = 0x60000708;        //index 6000; subindex 07; len= 8
const unsigned long _dict_dl_map = 0x60000808;        
const unsigned long _dict_dl_iat = 0x60000908;        
const unsigned long _dict_dl_ect = 0x60000A08;        
const unsigned long _dict_dl_pa  = 0x60000B08;        
const unsigned long _dict_dl_iac = 0x60000C08;        
const unsigned long _dict_dl_p0  = 0x60000D08;        
const unsigned long _dict_dl_p1  = 0x60000E08;        
const unsigned long _dict_dl_input1 = 0x60000F08;        
const unsigned long _dict_dl_input2 = 0x60001008;        
const unsigned long _dict_dl_cel1 = 0x60001108;        
const unsigned long _dict_dl_cel2 = 0x60001208;        
const unsigned long _dict_dl_cel3 = 0x60001308;        
const unsigned long _dict_dl_cel4 = 0x60001408;        
const unsigned long _dict_dl_bat  = 0x60001508;        
const unsigned long _dict_dl_gear = 0x60001604;        
const unsigned long _dict_dl_eld  = 0x60001708;       
const unsigned long _dict_dl_acc = 0x60001801;        
const unsigned long _dict_dl_pcs = 0x60001901;        
const unsigned long _dict_dl_altc = 0x60001A01;        
const unsigned long _dict_dl_fanc = 0x60001B01;        
const unsigned long _dict_dl_iab = 0x60001C01;        
const unsigned long _dict_dl_flr = 0x60001D01;        
const unsigned long _dict_dl_vtec1 = 0x60001E01;        
const unsigned long _dict_dl_vtec2 = 0x60001F01;        
const unsigned long _dict_dl_mil = 0x60002001;        
const unsigned long _dict_dl_pwrsteer = 0x60002101;        
const unsigned long _dict_dl_servcon = 0x60002201;        
const unsigned long _dict_dl_starter = 0x60002301;        
const unsigned long _dict_dl_vtp = 0x60002401;        
const unsigned long _dict_dl_ac = 0x60002501;        
const unsigned long _dict_dl_brake = 0x60002601;         
const unsigned long _dict_dl_dist1 = 0x60002708;         
const unsigned long _dict_dl_dist2 = 0x60002808;         
const unsigned long _dict_dl_dist3 = 0x60002908;         

//buffers to be used by the canopen structures in Xmt and Rcv canopen operations
//unsigned char uLocalXmtBuffer[8];			// Local buffer for TPDOs
//unsigned char uLocalRcvBuffer[8];			// local buffer fot RPDOs

// Static data refered to by the dictionary entries
const unsigned char TPDO1_objnum = 12;   //tpdo1 number of subindex entries on tpdo mapping
const unsigned char TPDO1_defnum = 2;   //tpdo1 number of subindex entries on tpdo comms configs

const unsigned char TPDO2_objnum = 15;   //tpdo1 number of subindex entries on tpdo mapping
const unsigned char TPDO2_defnum = 2;   //tpdo1 number of subindex entries on tpdo comms configs

//runtime variables to store the status of all TPDOs
TPDO TPDOs[CO_NUM_OF_TPDO];                   

/*********************************************************************
 * Function:        void ECUCAN_Initialize(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is the initialization to the demonstration
 *					object.
 *
 * Note:          	
 ********************************************************************/
//STATUS: TODO <------
void ECUCAN_Initialize(void)
{
    //configure TPDOs
    for (char i=0; i<CO_NUM_OF_TPDO; i++) {
        // Convert to MCHP
        mTOOLS_CO2MCHP(mCOMM_GetNodeID().byte + (0xC0000180L+0x100L*i));
        switch (i) {
            case 0:
                // Store the COB
                mTPDOSetCOB(1, mTOOLS_GetCOBID());
                // Set the pointer to the buffers - to kill - to be included in TX function 
                mTPDOSetTxPtr(1, (unsigned char *)(&(TPDOs[0].uLocalXmtBuffer[0])));
                //enable TPDO 1 - to kill - to be controlled by master
                mTPDOOpen(1);
                break;
            case 1:
                mTPDOSetCOB(2, mTOOLS_GetCOBID());
                // Set the pointer to the buffers - to kill - to be included in TX function 
                mTPDOSetTxPtr(2, (unsigned char *)(&(TPDOs[1].uLocalXmtBuffer[0])));
                //enable TPDO 1 - to kill - to be controlled by master
                mTPDOOpen(2);
                break;
            default: break;
        }
    }
	
    //configure RPDOs
/*     for (char i=0; i<CO_NUM_OF_RPDO; i++) {
	// Convert to MCHP
	//mTOOLS_CO2MCHP(mCOMM_GetNodeID().byte + (0xC0000200L+0x100L*i));
	
	// Store the COB
	//mRPDOSetCOB((i+1), mTOOLS_GetCOBID());
	

	// Set the pointer to the buffers (needs to be reviewed)
	//mRPDOSetRxPtr((i+1), (unsigned char *)(&uLocalRcvBuffer[0]));      
    }   
  */  
    
    //Load EPROM variables to RAM
    //loadDistance();
    
    //set device Operacional - to kill - to be controlled by master
    COMM_STATE_OPER = 1;
}

/*********************************************************************
 * Function:        void CO_COMMSyncEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        The function os triggered wwhen a SYNC signal is received
 *                  In case 					
 *
 * Note:       
 *   uDemoState.b0 = 1 ==> TPDO transmit flag. (send pdo asap)
 *   uDemoState.b1 = 1 ==> interrut occured (for acyclic and asynchronous transmits). check if synch or asynch to set either b0 or b2 flag
 *   uDemoState.b2 = 1 ==> (acyclic sync) event flag. when sync is received trigger TPDO for transmission   	
 ********************************************************************/
//STATUS: OK
void CO_COMMSyncEvent(void)
{   
    for (unsigned char i=0; i<CO_NUM_OF_TPDO; i++) {
        if ((TPDOs[i].synch_setting == acyclic) && (TPDOs[i].status.isWaitingSynch))
        {
            // Reset the Acyclic event Flag
           TPDOs[i].status.isWaitingSynch = 0;
            // transfer flag  (Flag PDO for transmission ASAP)
            TPDOs[i].status.isQueuedForTX = 1;
        }
        else
        if ((TPDOs[i].synch_setting >= 1) && (TPDOs[i].synch_setting <= 240))
        {
            // Adjust the sync counter
            TPDOs[i].synch_counter--;

            // If time to generate sync
            if (TPDOs[i].synch_counter == 0)
            {
                // Reset the sync counter
                TPDOs[i].synch_counter = TPDOs[i].synch_setting;

                //(Flag PDO for transmission ASAP)
                TPDOs[i].status.isQueuedForTX = 1;
            }
        }
    }
}

char checkTPDOisPutRdy(char TPDO) {
    if (TPDO==0) { return (unsigned) mTPDOIsPutRdy(1);}
    else if (TPDO==1) { return (unsigned) mTPDOIsPutRdy(2);}
    else if (TPDO==2) { return (unsigned) mTPDOIsPutRdy(3);}
    else if (TPDO==3) { return (unsigned) mTPDOIsPutRdy(4);}
    else if (TPDO==4) { return (unsigned) mTPDOIsPutRdy(5);}
    else if (TPDO==5) { return (unsigned) mTPDOIsPutRdy(6);}
    else if (TPDO==6) { return (unsigned) mTPDOIsPutRdy(7);}
    else if (TPDO==7) { return (unsigned) mTPDOIsPutRdy(8);}
    return 0;
}

char checkRPDOisGetRdy(char RPDO) {
    if (RPDO==0) { return mRPDOIsGetRdy(1);}
    else if (RPDO==1) { return mRPDOIsGetRdy(2);}
    else if (RPDO==2) { return mRPDOIsGetRdy(3);}
    else if (RPDO==3) { return mRPDOIsGetRdy(4);}
    else if (RPDO==4) { return mRPDOIsGetRdy(5);}
    else if (RPDO==5) { return mRPDOIsGetRdy(6);}
    else if (RPDO==6) { return mRPDOIsGetRdy(7);}
    else if (RPDO==7) { return mRPDOIsGetRdy(8);}
    return 0;
}

void writeTPDO(unsigned char TPDO) {
    if (TPDO==0) { mTPDOWritten(1);}
    else if (TPDO==1) { mTPDOWritten(2);}
    else if (TPDO==2) { mTPDOWritten(3);}
    else if (TPDO==3) { mTPDOWritten(4);}
    else if (TPDO==4) { mTPDOWritten(5);}
    else if (TPDO==5) { mTPDOWritten(6);}
    else if (TPDO==6) { mTPDOWritten(7);}
    else if (TPDO==7) { mTPDOWritten(8);}
}

void readRPDO(unsigned char RPDO) {
    if (RPDO==0) { mRPDORead(1);}
    else if (RPDO==1) { mRPDORead(2);}
    else if (RPDO==2) { mRPDORead(3);}
    else if (RPDO==3) { mRPDORead(4);}
    else if (RPDO==4) { mRPDORead(5);}
    else if (RPDO==5) { mRPDORead(6);}
    else if (RPDO==6) { mRPDORead(7);}
    else if (RPDO==7) { mRPDORead(8);}
}

/*********************************************************************
 * Function:        void ECUCAN_ProcessEvents(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        Application specific code executed on  while cycle 
 *                  in main routine
 *
 * Note:          	
 ********************************************************************/
//STATUS: OK
void ECUCAN_ProcessEvents(void)
{
    for (char i=0; i<CO_NUM_OF_TPDO; i++) {
        // If ready to send 
        if (checkTPDOisPutRdy(i) && TPDOs[i].status.isQueuedForTX)
        {
            // Tell the stack data is loaded for transmit
            writeTPDO(i);

            // Reset any synchronous or asynchronous flags
            TPDOs[i].status.isQueuedForTX = 0;
            TPDOs[i].status.wasTriggered = 0;
        }
    }

    for (char j=0; j<CO_NUM_OF_RPDO; j++) {
        // If any data has been received
        if (checkRPDOisGetRdy(j))
        {
            // PDO read, free the driver to accept more data
            readRPDO(j);
        }
    }
}

/*********************************************************************
 * Function:        void CO_COMM_RPDO1_COBIDAccessEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is a simple demonstration of a RPDO COB access
 *					handling function.
 *
 * Note:          	This function is called from the dictionary.
 ********************************************************************/
//STATUS: OK
void CO_COMM_RPDO1_COBIDAccessEvent(void)
{
	switch (mCO_DictGetCmd())
	{
		case DICT_OBJ_READ: 	// Read the object
			// Translate MCHP COB to CANopen COB
			mTOOLS_MCHP2CO(mRPDOGetCOB(1));
			
			// Return the COBID
			*(unsigned long *)(uDict.obj->pReqBuf) = mTOOLS_GetCOBID();
			break;

		case DICT_OBJ_WRITE: 	// Write the object
			// Translate the COB to MCHP format
			mTOOLS_CO2MCHP(*(unsigned long *)(uDict.obj->pReqBuf));
			
			// If the request is to stop the PDO
			if ((*(UNSIGNED32 *)(&mTOOLS_GetCOBID())).PDO_DIS)
			{
				// And if the COB received matches the stored COB and type then close
				if (!((mTOOLS_GetCOBID() ^ mRPDOGetCOB(1)) & 0xFFFFEFFFL))
				{
					// but only close if the PDO endpoint was open
					if (mRPDOIsOpen(1)) {mRPDOClose(1);}
		
					// Indicate to the local object that this PDO is disabled
					(*(UNSIGNED32 *)(&mRPDOGetCOB(1))).PDO_DIS = 1;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}

			// Else if the TPDO is not open then start the TPDO
			else
			{
				// And if the COB received matches the stored COB and type then open
				if (!((mTOOLS_GetCOBID() ^ mRPDOGetCOB(1)) & 0xFFFFEFFFL))
				{
					// but only open if the PDO endpoint was closed
					if (!mRPDOIsOpen(1)) {mRPDOOpen(1);}
						
					// Indicate to the local object that this PDO is enabled
					(*(UNSIGNED32 *)(&mRPDOGetCOB(1))).PDO_DIS = 0;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}
			break;
	}	
}

/*********************************************************************
 * Function:        void CO_COMM_TPDO1_COBIDAccessEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is a simple demonstration of a TPDO COB access
 *					handling function.
 *
 * Note:          	This function is called from the dictionary.
 ********************************************************************/
//STATUS: OK
void CO_COMM_TPDO1_COBIDAccessEvent(void)
{
	switch (mCO_DictGetCmd())
	{
		case DICT_OBJ_READ: 	// Read the object
			// Translate MCHP COB to CANopen COB
			mTOOLS_MCHP2CO(mTPDOGetCOB(1));
			
			// Return the COBID
			*(unsigned long *)(uDict.obj->pReqBuf) = mTOOLS_GetCOBID();
			break;

		case DICT_OBJ_WRITE: 	// Write the object
			// Translate the COB to MCHP format
			mTOOLS_CO2MCHP(*(unsigned long *)(uDict.obj->pReqBuf));
			
			// If the request is to stop the PDO
			if ((*(UNSIGNED32 *)(&mTOOLS_GetCOBID())).PDO_DIS)
			{
				// And if the COB received matches the stored COB and type then close
				if (!((mTOOLS_GetCOBID() ^ mTPDOGetCOB(1)) & 0xFFFFEFFFL))
				{
					// but only close if the PDO endpoint was open
					if (mTPDOIsOpen(1)) {mTPDOClose(1);}
		
					// Indicate to the local object that this PDO is disabled
					(*(UNSIGNED32 *)(&mTPDOGetCOB(1))).PDO_DIS = 1;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}

			// Else if the TPDO is not open then start the TPDO
			else
			{
				// And if the COB received matches the stored COB and type then open
				if (!((mTOOLS_GetCOBID() ^ mTPDOGetCOB(1)) & 0xFFFFEFFFL))
				{
					// but only open if the PDO endpoint was closed
					if (!mTPDOIsOpen(1)) {mTPDOOpen(1);}
						
					// Indicate to the local object that this PDO is enabled
					(*(UNSIGNED32 *)(&mTPDOGetCOB(1))).PDO_DIS = 0;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}
			break;
	}	
}


/*********************************************************************
 * Function:        void CO_COMM_TPDO2_COBIDAccessEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is a simple demonstration of a TPDO COB access
 *					handling function.
 *
 * Note:          	This function is called from the dictionary.
 ********************************************************************/
//STATUS: OK
void CO_COMM_TPDO2_COBIDAccessEvent(void)
{
	switch (mCO_DictGetCmd())
	{
		case DICT_OBJ_READ: 	// Read the object
			// Translate MCHP COB to CANopen COB
			mTOOLS_MCHP2CO(mTPDOGetCOB(2));
			
			// Return the COBID
			*(unsigned long *)(uDict.obj->pReqBuf) = mTOOLS_GetCOBID();
			break;

		case DICT_OBJ_WRITE: 	// Write the object
			// Translate the COB to MCHP format
			mTOOLS_CO2MCHP(*(unsigned long *)(uDict.obj->pReqBuf));
			
			// If the request is to stop the PDO
			if ((*(UNSIGNED32 *)(&mTOOLS_GetCOBID())).PDO_DIS)
			{
				// And if the COB received matches the stored COB and type then close
				if (!((mTOOLS_GetCOBID() ^ mTPDOGetCOB(2)) & 0xFFFFEFFFL))
				{
					// but only close if the PDO endpoint was open
					if (mTPDOIsOpen(2)) {mTPDOClose(2);}
		
					// Indicate to the local object that this PDO is disabled
					(*(UNSIGNED32 *)(&mTPDOGetCOB(2))).PDO_DIS = 1;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}

			// Else if the TPDO is not open then start the TPDO
			else
			{
				// And if the COB received matches the stored COB and type then open
				if (!((mTOOLS_GetCOBID() ^ mTPDOGetCOB(2)) & 0xFFFFEFFFL))
				{
					// but only open if the PDO endpoint was closed
					if (!mTPDOIsOpen(2)) {mTPDOOpen(2);}
						
					// Indicate to the local object that this PDO is enabled
					(*(UNSIGNED32 *)(&mTPDOGetCOB(2))).PDO_DIS = 0;
				}
				else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			}
			break;
	}	
}

/*********************************************************************
 * Function:        void CO_COMM_TPDO1_TypeAccessEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is a simple demonstration of a TPDO type access
 *					handling function.
 *
 * Note:          	This function is called from the dictionary.
 ********************************************************************/
//STATUS: OK
void CO_COMM_TPDO1_TypeAccessEvent(void)
{
	unsigned char tempType;
	
	switch (mCO_DictGetCmd())
	{
		//case DICT_OBJ_INFO:		// Get information about the object
			// The application should use this to load the 
			// structure with legth, access, and mapping.
		//	break;

		case DICT_OBJ_READ: 	// Read the object
			// Write the Type to the buffer
			*(uDict.obj->pReqBuf) = TPDOs[0].synch_setting;
			break;

		case DICT_OBJ_WRITE: 	// Write the object
			tempType = *(uDict.obj->pReqBuf);
			if (tempType <= 240)
			{
				// Set the new type and resync
				TPDOs[0].synch_counter = TPDOs[0].synch_setting = tempType;
			}
			else 
			if ((tempType == 254) || (tempType == 255))
			{
				TPDOs[0].synch_setting = tempType;
			}
			else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			
			break;
	}	
}



/*********************************************************************
 * Function:        void CO_COMM_TPDO2_TypeAccessEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        This is a simple demonstration of a TPDO type access
 *					handling function.
 *
 * Note:          	This function is called from the dictionary.
 ********************************************************************/
//STATUS: OK
void CO_COMM_TPDO2_TypeAccessEvent(void)
{
	unsigned char tempType;
	
	switch (mCO_DictGetCmd())
	{
		//case DICT_OBJ_INFO:		// Get information about the object
			// The application should use this to load the 
			// structure with legth, access, and mapping.
		//	break;

		case DICT_OBJ_READ: 	// Read the object
			// Write the Type to the buffer
			*(uDict.obj->pReqBuf) = TPDOs[1].synch_setting;
			break;

		case DICT_OBJ_WRITE: 	// Write the object
			tempType = *(uDict.obj->pReqBuf);
			if (tempType <= 240)
			{
				// Set the new type and resync
				TPDOs[1].synch_counter = TPDOs[1].synch_setting = tempType;
			}
			else 
			if ((tempType == 254) || (tempType == 255))
			{
				TPDOs[1].synch_setting = tempType;
			}
			else {mCO_DictSetRet(E_PARAM_RANGE);} //error
			
			break;
	}	
}

/*********************************************************************
 * Function:        void CO_PDO1LSTimerEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        none
 *
 * Note:          	none
 ********************************************************************/
//STATUS: N/A
void CO_PDO1LSTimerEvent(void)
{
	
}

/*********************************************************************
 * Function:        void CO_PDO1TXFinEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        none
 *
 * Note:          	none
 ********************************************************************/
//STATUS: N/A
void CO_PDO1TXFinEvent(void)
{
	
}

/*********************************************************************
 * Function:        void CO_PDO1LSTimerEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        none
 *
 * Note:          	none
 ********************************************************************/
//STATUS: N/A
void CO_PDO2LSTimerEvent(void)
{
	
}

/*********************************************************************
 * Function:        void CO_PDO1TXFinEvent(void)
 *
 * PreCondition:    none
 *
 * Input:       	none
 *                  
 * Output:         	none  
 *
 * Side Effects:    none
 *
 * Overview:        none
 *
 * Note:          	none
 ********************************************************************/
//STATUS: N/A
void CO_PDO2TXFinEvent(void)
{
	
}