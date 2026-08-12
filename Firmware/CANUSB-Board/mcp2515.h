//MCP2515 defines
// Registers (CTRL, Interrupts and state)
#define CANCTRL		0x0F
#define CANSTAT     0x0E
#define CANINTE     0x2B
#define CANINTF     0x2C
#define RXB0CTRL	0x60
#define RXB1CTRL	0x70
#define BFPCTRL 	0x0C
#define TXB0CTRL	0x30
#define TXB1CTRL	0x40
#define TXB2CTRL	0x50
#define TXRTSCTRL   0x0D
#define CNF1 0x2A
#define CNF2 0x29
#define CNF3 0x28

//Registers (Data messages)
#define TXB0SIDH 0x31
#define TXB1SIDH 0x41
#define TXB2SIDH 0x51
#define RXB0SIDH 0x61
#define RXB1SIDH 0x71

//Instructions
#define RESET_COM    0xC0
#define READ_COM     0x03
#define WRITE_COM    0x02
#define RXSTAT_COM   0xB0
#define BITMOD_COM   0x05
#define RTSTXB0_COM  0x81
#define RTSTXB1_COM  0x82
#define RTSTXB2_COM  0x84
//optional Instructions - seldom or never used
#define READRX0_COM  0x92
#define READRX1_COM  0x94
#define LOADTX0_COM  0x41
#define LOADTX1_COM  0x42
#define LOADTX2_COM  0x44
#define READSTAT_COM 0xA0

//Flags
#define MERR_FLAG 0x80
#define WAK_FLAG  0x40
#define ERR_FLAG  0x20
#define TX2_FLAG  0x10
#define TX1_FLAG  0x08
#define TX0_FLAG  0x04
#define RX1_FLAG  0x02
#define RX0_FLAG  0x01

//Operation MODE
//#define LOOPBACK 

#ifdef LOOPBACK 
    //Loopback mode with clkout = fosc, one-shot mode=off, clear ABAT
    #define OP_MODE 0x44
#else
    //Normal mode with clkout = fosc, one-shot mode=off, clear ABAT
    #define OP_MODE 0x04
#endif

#include <stdbool.h>

void MCP2515_write(char , char ) ;

void MCP2515_bitModify(char , char , bool );

void MCP2515_reset();

void MCP2515_RTS(char );

char MCP2515_readByte(char );

void MCP2515_read(char , char[] , int ) ;

char MCP2515_getRXStatus();

void MCP2515_config();

void MCP2515_readMessage(int ) ;

void MCP2515_processInterrupt() ;

extern char MCP2515_message[13];
extern char MCP2515_RXstatus;
extern char MCP2515_CANERR;
extern bool MCP2515_received;


