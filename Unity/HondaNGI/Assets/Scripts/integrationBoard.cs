using UnityEngine;
using System.Collections.ObjectModel;
using System;
using System.Runtime.InteropServices;
using Usb = MonoLibUsb.MonoUsbApi;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;
using MonoLibUsb.Transfer;
using MonoLibUsb;



namespace integrationBoard
{

    internal enum ReqMode
    {
        Sync,
        Async
    }

    static public class DeviceLib
    {

        #region DEVICE SETUP
        private const int MY_CONFIG = 1;
        private const byte MY_EP_READ = 0x81;
        private const byte MY_EP_WRITE = 0x01;
        private const int MY_INTERFACE = 0;

        public static int checkForDataloggingData(int vecLen)
        {
            Debug.Log("checkForDataloggingData");

            int r = 0;
            int transferred;
            byte[] testReadData = new byte[vecLen];

            //clear response data
            for (int i = 1; i < vecLen; i++) { testReadData[i] = (byte)0; }
                        
            try
            {
                do
                {
                    if ((device_handle == null) || device_handle.IsInvalid)
                    {
                        if (getDataloggingDevice() > 0) { sessionHandle = null; device_handle = null; break; }
                    }

                    // Set configuration
                    //Debug.Log("Set Config..");
                    r = MonoUsbApi.SetConfiguration(device_handle, MY_CONFIG);
                    if (r != 0) break;

                    // Claim interface
                    //Debug.Log("Set Interface..");
                    r = MonoUsbApi.ClaimInterface(device_handle, MY_INTERFACE);
                    if (r != 0) break;

                    //////////////////////
                    // GET LOGGING DATA //
                    //////////////////////
                    int packetCount = 0;
                    int transferredTotal = 0;

                    // If the Async REQ_MODE enumeration is set, use
                    // the internal transfer function
                    if (REQ_MODE == ReqMode.Async)
                    {
                        r = (int)doBulkAsyncTransfer(device_handle,
                                                        MY_EP_READ,
                                                        testReadData,
                                                        vecLen,
                                                        out transferred,
                                                        CONN_TIMEOUT);
                    }
                    else
                    {
                        // Use the sync bulk transfer API function 
                        r = MonoUsbApi.BulkTransfer(device_handle,
                                                               MY_EP_READ,
                                                               testReadData,
                                                               vecLen,
                                                               out transferred,
                                                               CONN_TIMEOUT);
                    }

                    if (r == (int)MonoUsbError.ErrorTimeout)
                    {
                        Debug.Log("Read Timed Out.");
                    }
                    else if (r != 0)
                    {
                        // An error, other than ErrorTimeout was received. 
                        Debug.Log("Read failed: " + (MonoUsbError)r);
                    }
                    else
                    {
                        transferredTotal += transferred;
                        packetCount++;

                        // Display test data.
                        //Debug.Log("DATA Received.");

                        DataLogging.loadData(testReadData);

                        //Debug.Log(System.Text.Encoding.Default.GetString(testReadData, 0, transferred));
                    }

                } while (false);
            }
            catch (Exception e)
            {
                device_handle = null;
                sessionHandle = null;
                Debug.Log("Something went wrong");
                Console.WriteLine("{0} Exception caught.", e);
            }


            return r;
        }

        //private const short MY_PID = 0x0A02;
        //private const short MY_VID = 0x6666;
        private const short MY_PID = 0x0A02;
        private const short MY_VID = 0x6666;
        #endregion

        #region TRANSFER SETUP
        private const int CONN_TIMEOUT = 500;
         private static ReqMode REQ_MODE;
        #endregion

        private static MonoUsbSessionHandle sessionHandle = null;
        private static MonoUsbDeviceHandle device_handle = null;

        // This function originated from bulk_transfer_cb()
        // in sync.c of the Libusb-1.0 source code.
        private static void bulkTransferCB(MonoUsbTransfer transfer)
        {
            Marshal.WriteInt32(transfer.PtrUserData, 1);
            // caller interprets results and frees transfer 
        }

        // This function originated from do_sync_bulk_transfer()
        // in sync.c of the Libusb-1.0 source code.
        private static MonoUsbError doBulkAsyncTransfer(MonoUsbDeviceHandle dev_handle,
                                                          byte endpoint,
                                                          byte[] buffer,
                                                          int length,
                                                          out int transferred,
                                                          int timeout)
        {
            transferred = 0;
            MonoUsbTransfer transfer = new MonoUsbTransfer(0);
            if (transfer.IsInvalid) return MonoUsbError.ErrorNoMem;

            MonoUsbTransferDelegate monoUsbTransferCallbackDelegate = bulkTransferCB;
            int[] userCompleted = new int[] { 0 };
            GCHandle gcUserCompleted = GCHandle.Alloc(userCompleted, GCHandleType.Pinned);

            MonoUsbError e;
            GCHandle gcBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            transfer.FillBulk(
                dev_handle,
                endpoint,
                gcBuffer.AddrOfPinnedObject(),
                length,
                monoUsbTransferCallbackDelegate,
                gcUserCompleted.AddrOfPinnedObject(),
                timeout);

            e = transfer.Submit();
            if ((int)e < 0)
            {
                transfer.Free();
                gcUserCompleted.Free();
                return e;
            }
            int r;
            //Debug.Log("Transfer Submitted..");
            while (userCompleted[0] == 0)
            {
                e = (MonoUsbError)(r = Usb.HandleEvents(sessionHandle));
                if (r < 0)
                {
                    if (e == MonoUsbError.ErrorInterrupted)
                        continue;
                    transfer.Cancel();
                    while (userCompleted[0] == 0)
                        if (Usb.HandleEvents(sessionHandle) < 0)
                            break;
                    transfer.Free();
                    gcUserCompleted.Free();
                    return e;
                }
            }

            transferred = transfer.ActualLength;
            e = MonoUsbApi.MonoLibUsbErrorFromTransferStatus(transfer.Status);
            transfer.Free();
            gcUserCompleted.Free();
            return e;
        }

        public static UsbDevice MyUsbDevice;

        public static int getDataloggingDevice()
        {

            try
            {
                if ((sessionHandle == null) || (sessionHandle.IsInvalid))  { 
                    //Debug.Log("Create USB Handle..");
                    sessionHandle = new MonoUsbSessionHandle();
                    //Debug.Log("USB Handle created..");
                    if (sessionHandle.IsInvalid) throw new Exception("Invalid session handle.");
                }

                    //Debug.Log("Opening Device..");
                    device_handle = MonoUsbApi.OpenDeviceWithVidPid(sessionHandle, MY_VID, MY_PID);
                    if ((device_handle == null) || device_handle.IsInvalid) return 1;

                    //Optional: Reset the device and re-open
                    MonoUsbApi.ResetDevice(device_handle);
                    device_handle.Close();
                    device_handle = MonoUsbApi.OpenDeviceWithVidPid(sessionHandle, MY_VID, MY_PID);
                    if ((device_handle == null) || device_handle.IsInvalid) return 1;

            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong: Could not find device");
                Console.WriteLine("{0} Exception caught.", e);
                return 1;
            }
            return 0;
        }


    public static int releaseDataloggingDevice()
        {
            try
            {

                // Free and close resources
                if (device_handle != null)
                {
                    if (!device_handle.IsInvalid)
                    {
                        MonoUsbApi.ReleaseInterface(device_handle, MY_INTERFACE);
                        device_handle.Close();
                    }
                }
                if (sessionHandle != null)
                {
                    sessionHandle.Close();
                    sessionHandle = null;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Something went wrong");
                Console.WriteLine("{0} Exception caught.", e);
                return 1;
            }
            return 0;
        }

        public static int getDataloggingData(int vecLen)
        {
            //Debug.Log("getData");

            int r = 0;
            int transferred;
            byte[] testWriteData = new byte[vecLen];
            byte[] testReadData = new byte[vecLen];

            ReqMode REQ_MODE = ReqMode.Async;

            //command to ask for data logging info
            testWriteData[0] = 0x81;
            testReadData[0] = 0x81;

            //clear response data
            for (int i = 1; i < vecLen; i++) { testReadData[i] = (byte)0; }

            

            try
            {
                do
                {
                    if ((device_handle == null) || device_handle.IsInvalid)
                    {
                        if (getDataloggingDevice() > 0) { sessionHandle = null; device_handle = null; break; }
                    }

                    // Set configuration
                    //Debug.Log("Set Config..");
                    r = MonoUsbApi.SetConfiguration(device_handle, MY_CONFIG);
                    if (r != 0) break;

                    // Claim interface
                    //Debug.Log("Set Interface..");
                    r = MonoUsbApi.ClaimInterface(device_handle, MY_INTERFACE);
                    if (r != 0) break;

                    /////////////////////
                    // REQUEST LOGDATA //
                    /////////////////////
                    int packetCount = 0;
                    int transferredTotal = 0;
                    //Debug.Log("Sending test data..");

                    // If the Async TEST_MODE enumeration is set, use
                    // the internal transfer function
                    if (REQ_MODE == ReqMode.Async)
                    {
                        r = (int)doBulkAsyncTransfer(device_handle,
                                                        MY_EP_WRITE,
                                                        testWriteData,
                                                        vecLen,
                                                        out transferred,
                                                        CONN_TIMEOUT);
                    }
                    else
                    {
                        // Use the sync bulk transfer API function 
                        r = MonoUsbApi.BulkTransfer(device_handle,
                                                               MY_EP_WRITE,
                                                               testWriteData,
                                                               vecLen,
                                                               out transferred,
                                                               CONN_TIMEOUT);
                    }
                    if (r == 0)
                    {
                        packetCount++;
                        transferredTotal += transferred;
                    }



                    if (r == (int)MonoUsbError.ErrorTimeout)
                    {
                        // This is considered normal operation
                        //Debug.Log("Write Timed Out. " + packetCount.ToString() + " packet(s) written (" + transferredTotal.ToString() + " bytes)");
                    }
                    else if (r != (int)MonoUsbError.ErrorTimeout && r != 0)
                    {
                        // An error, other than ErrorTimeout was received. 
                        Debug.Log("Write failed:" + r.ToString());
                        device_handle = null;
                        sessionHandle = null;
                        break;
                    }

                    //////////////////////
                    // GET LOGGING DATA //
                    //////////////////////
                    //Debug.Log("Reading data..");
                    packetCount = 0;
                    transferredTotal = 0;

                    // If the Async REQ_MODE enumeration is set, use
                    // the internal transfer function
                    if (REQ_MODE == ReqMode.Async)
                    {
                        r = (int)doBulkAsyncTransfer(device_handle,
                                                        MY_EP_READ,
                                                        testReadData,
                                                        vecLen,
                                                        out transferred,
                                                        CONN_TIMEOUT);
                    }
                    else
                    {
                        // Use the sync bulk transfer API function 
                        r = MonoUsbApi.BulkTransfer(device_handle,
                                                               MY_EP_READ,
                                                               testReadData,
                                                               vecLen,
                                                               out transferred,
                                                               CONN_TIMEOUT);
                    }

                    if (r == (int)MonoUsbError.ErrorTimeout)
                    {
                        Debug.Log("Read Timed Out.");
                    }
                    else if (r != 0)
                    {
                        // An error, other than ErrorTimeout was received. 
                        Debug.Log("Read failed: " + (MonoUsbError)r);
                    }
                    else
                    {
                        transferredTotal += transferred;
                        packetCount++;

                        // Display test data.
                        //Debug.Log("DATA Received.");

                        DataLogging.loadData(testReadData);

                        //Debug.Log(System.Text.Encoding.Default.GetString(testReadData, 0, transferred));
                    }

                } while (false);
            }
            catch (Exception e)
            {
                device_handle = null;
                sessionHandle = null;
                Debug.Log("Something went wrong");
                Console.WriteLine("{0} Exception caught.", e);
            }
            

            return r;
        }

        public static void listDevices()
        {
            Debug.Log("Started Device listing...");

            // Dump all devices and descriptor information to console output.
            UsbRegDeviceList allDevices = UsbDevice.AllDevices;
            foreach (UsbRegistry usbRegistry in allDevices)
            {
                if (usbRegistry.Open(out MyUsbDevice))
                {
                    //Debug.Log(MyUsbDevice.Info.ToString());
                    for (int iConfig = 0; iConfig < MyUsbDevice.Configs.Count; iConfig++)
                    {
                        UsbConfigInfo configInfo = MyUsbDevice.Configs[iConfig];
                        //Debug.Log(configInfo.ToString());

                        ReadOnlyCollection<UsbInterfaceInfo> interfaceList = configInfo.InterfaceInfoList;
                        for (int iInterface = 0; iInterface < interfaceList.Count; iInterface++)
                        {
                            UsbInterfaceInfo interfaceInfo = interfaceList[iInterface];
                            //Debug.Log(interfaceInfo.ToString());

                            ReadOnlyCollection<UsbEndpointInfo> endpointList = interfaceInfo.EndpointInfoList;
                            for (int iEndpoint = 0; iEndpoint < endpointList.Count; iEndpoint++)
                            {
                                //Debug.Log(endpointList[iEndpoint].ToString());
                            }
                        }
                    }
                }
            }


            // Free usb resources.
            // This is necessary for libusb-1.0 and Linux compatibility.
            UsbDevice.Exit();

        }

    }
}

