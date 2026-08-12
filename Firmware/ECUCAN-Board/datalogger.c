#include "datalogger.h"
#include "rs232.h"

int dataIndex;
int timeSlice;

void startupDL() {
}

//check if sensor is elegible for this dataFetch
int isElegible(int timeSlice) {
    int freq;
    
    if (datalogger[dataIndex*5]!=0x00) {
        if (timeSlice==0) { return 1; }
        
        freq = (10/(int)datalogger[dataIndex*5+3]);
        if ((timeSlice)%freq == 0) {
            return 1;
        }
        else {
            return 0;
        }
    }
    else {
        return 0;
    }
}

int getDLData(char answer) {
    datalogger[dataIndex*5+4] = answer;
    
    //get last byte
    //sendRS232Data(0x08);
    
    
        //get Next Sensor in Line in case the dataFetch is not over yet
        if (dataIndex<29) { 
            dataIndex++; 
            askDLData();
        }
    
    return 0;
}

int askDLData() {
    for (;dataIndex<30;dataIndex++) {
        if (isElegible(timeSlice)) { break; }
    }
    
    //if eligible and standard command
    if (datalogger[dataIndex*5]==0x01) {
    //get Sensor Data
        sendRS232Data(datalogger[dataIndex*5+1]);
    }
    else {
        //if elegible and specific RAM position
        //TODO
    }
    return 0;
}

int startDataFetch(int TS) {
    //no requests are supposed to be pending so:
    dataIndex=0;
    timeSlice = TS;
    
    //iterate thru the 30 possible positions
    askDLData();
    
    return 0;
}