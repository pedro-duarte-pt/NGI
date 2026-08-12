#include <xc.h>
#include "mcp2515.h"
#include "system.h"

char MCP2515_message[13];
char MCP2515_RXstatus;
char MCP2515_CANERR;
bool MCP2515_received;
    
void MCP2515_write(char address, char data) {
    char buf[3];
    
    buf[0] = 2; //MCP2515 write instruction
    buf[1] = address;
    buf[2] = data;
    
    SPI_CS = 0;
    SPI_write(buf, 3);   
    SPI_CS = 1;
}

void MCP2515_bitModify(char address, char bitMask, bool value) {
    char buf[4];
    
    buf[0] = BITMOD_COM; //MCP2515 bit modify instruction
    buf[1] = address;
    buf[2] = bitMask;
    if (value) { buf[3] = 0xFF; }
    else {buf[3] = 0x00; }
    
    SPI_CS = 0;
    SPI_write(buf, 4);   
    SPI_CS = 1;
}

void MCP2515_reset() {
    SPI_CS = 0;
    SPI_writebByte(RESET_COM);   
    SPI_CS = 1;
}

void MCP2515_RTS(char buffer) {
    SPI_CS = 0;
    switch(buffer) {
        case 0:
            SPI_writebByte(RTSTXB0_COM); 
            break;
        case 1:
            SPI_writebByte(RTSTXB1_COM); 
            break;
        default:
            SPI_writebByte(RTSTXB2_COM); 
            break;
    }
    SPI_CS = 1;
}

char MCP2515_readByte(char regaddr) {
    char result;
    SPI_CS = 0;
    SPI_writebByte(3);  //MCP2515 read instruction
    SPI_writebByte(regaddr); //specify address to read
    result = SPI_writebByte(0x00); //Send dummy data to read value
    SPI_CS = 1;
    return result;
} 

void MCP2515_read(char regaddr, char vec[], int len) {
    SPI_CS = 0;
    SPI_writebByte(3);  //MCP2515 read instruction
    SPI_writebByte(regaddr); //specify address to read
    for (int i=0; i<len; i++) {
        vec[i] = SPI_writebByte(0x00); //Send dummy data to read value     
    }
    SPI_CS = 1;
}

char MCP2515_getRXStatus() {
    char temp;
    
    SPI_CS = 0;
    temp = SPI_writebByte(RXSTAT_COM);   
    SPI_CS = 1; 
    
    return temp;
}

void MCP2515_config() {
    //Clear error flag
    MCP2515_CANERR = 0;
    //reset MCP2515
    MCP2515_reset();

    MCP2515_write(CANCTRL,0x84);   //Configuration mode
    //CANBUS settings
    MCP2515_write(CNF1,0x40);      //SJW=2TQ; TQ=2/Fosc (0,1us for 20MHz fosc => 10Tq / bit at 1Mbps)
    MCP2515_write(CNF2,0xD1);      //PS2 explicit; triple bit sampling; PHS=3TQ; PS1=2TQ
    MCP2515_write(CNF3,0x03);      //PS2=4TQ for a sampling around 60% of the bit time at 1Mbps with 10TQ/bit
    //TX settings
    MCP2515_write(TXB0CTRL,0);     //clear TXREQ flag and configure TX0 as low priority
    MCP2515_write(TXB1CTRL,1);     //clear TXREQ flag and configure TX1 as medium priority
    MCP2515_write(TXB2CTRL,2);     //clear TXREQ flag and configure TX2 as high priority
    MCP2515_write(TXRTSCTRL,0);    //Configure RTS pins as digital inputs
    //RX settings
    MCP2515_write(RXB0CTRL,0b01100100);    //Receive all messages; rollover
    MCP2515_write(RXB1CTRL,0b01100000);    //Receive all messages; 
    MCP2515_write(BFPCTRL,0b00001111);     //Configure BF pins as Interrupt pins
    //Activate device
    MCP2515_write(CANINTE,0xFF);        //Enable all interrupts
    MCP2515_write(CANCTRL,OP_MODE);     //normal or loopback mode (Depends on LOOPBACK define)
}

void MCP2515_readMessage(int buffer) {
    MCP2515_received = true;
    
    //get accepted filter criteria
    MCP2515_RXstatus = MCP2515_getRXStatus();
    
    //case no Message Service Routine exists, download entire message
    switch(buffer) {
        case 0:
            MCP2515_read(RXB0SIDH, MCP2515_message, 13); 
            break;
        default:
            MCP2515_read(RXB1SIDH, MCP2515_message, 13);
            break;
    }
}

void MCP2515_processInterrupt() {
    char temp;
    temp = MCP2515_readByte(CANINTF);
        
    if (temp&TX2_FLAG){
        //load new message
        MCP2515_bitModify(CANINTF,TX2_FLAG,false);
    }
    if (temp&TX1_FLAG){
        //load new message
        MCP2515_bitModify(CANINTF,TX1_FLAG,false);
    }
    if (temp&TX0_FLAG){
        //load new message
        MCP2515_bitModify(CANINTF,TX0_FLAG,false);
    }
    if (temp&RX1_FLAG){
        MCP2515_readMessage(1);
        MCP2515_bitModify(CANINTF,RX1_FLAG,false);
    }
    if (temp&RX0_FLAG){
        MCP2515_readMessage(0);
        MCP2515_bitModify(CANINTF,RX0_FLAG,false);
    }
    if (temp&MERR_FLAG) {
        ERROR_LED =  LED_ON; //set error led
        MCP2515_bitModify(CANINTF,MERR_FLAG,false);
        MCP2515_CANERR = 1; //since the error bit was clear, flag LED to be unset
    }
    if (temp&WAK_FLAG){
        MCP2515_bitModify(CANINTF,WAK_FLAG,false);
    }
    if (temp&ERR_FLAG){
        ERROR_LED =  LED_ON; //set error led
        MCP2515_bitModify(CANINTF,ERR_FLAG,false);
        MCP2515_CANERR = 1; //since the error bit was clear, flag LED to be unset
    }
}