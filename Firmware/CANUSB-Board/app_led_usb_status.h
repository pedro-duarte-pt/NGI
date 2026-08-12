/*********************************************************************
* Function: void APP_LEDUpdateUSBStatus(void);
* Overview: Uses one LED to indicate the status of the device on the USB bus.
*           A fast blink indicates successfully connected.  A slow pulse
*           indicates that it is still in the process of connecting.  Off
*           indicates thta it is not attached to the bus or the bus is suspended.
*           This should be called on every start of frame packet reception and
*           if a suspend/resume event occurs.
* PreCondition: LEDs are enabled.
********************************************************************/
void APP_LEDUpdateUSBStatus(void);
