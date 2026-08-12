
#include "canopen.h"
#include "system.h"
#include "canopen/CO_MAIN.H"

void setCANOPEN() {
    processCO_Timed_events_flag = 0;
    
	mSYNC_SetCOBID(0x10);		// Set the SYNC COB ID (MCHP format)
	mCO_SetNodeID(0x02);		// Set the Node ID
	mCO_SetBaud(0x00);			// Set the baudrate (1mbps))
    
	mNMTE_SetHeartBeat(5000);	// Set the initial heartbeat
	mNMTE_SetGuardTime(0000);	// Set the initial guard time
	mNMTE_SetLifeFactor(0x00);	// Set the initial life time

    mCO_InitAll();				// Initialize CANopen to run, bootup will be sent

}