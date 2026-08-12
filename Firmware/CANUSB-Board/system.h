//IO PINS
#define ERROR_LED LATCbits.LATC5
#define POWER_LED LATCbits.LATC3
#define LED_ON 1
#define LED_OFF 0
#define SPI_CS LATCbits.LATC2
#define SPI_RESET LATCbits.LATC1
#define LED_USB_DEVICE_STATE LATCbits.LATC4

void SPI_config() ;

char SPI_writebByte(char ) ;

void SPI_write(char[] , unsigned ) ;

void setUpDevice();
