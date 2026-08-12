/*********************************************************************
* Function: void APP_DeviceVendorBasicDemoInitialize(void);
* Overview: Initializes the demo code
********************************************************************/
void APP_DeviceVendorBasicDemoInitialize();

/*********************************************************************
* Function: void APP_DeviceVendorBasicDemoTasks(void);
* Overview: Keeps the demo running.
* PreCondition: The demo should have been initialized and started via
*   the APP_DeviceVendorBasicDemoInitialize() and APP_DeviceVendorBasicDemoStart() demos
*   respectively.
********************************************************************/

void APP_DeviceVendorBasicDemoTasks();

extern unsigned char loaded_bytes;
extern unsigned char INPacket[];