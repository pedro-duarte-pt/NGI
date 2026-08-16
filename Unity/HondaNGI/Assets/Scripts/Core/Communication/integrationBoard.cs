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
                        if (getDataloggingDevice() > 0) { r = 1; break; }
                    }


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
                        Debug.LogWarning("Read failed: " + (MonoUsbError)r);
                        releaseDataloggingDevice();
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
                releaseDataloggingDevice();
                Debug.LogWarning("CANUSB operation failed: " + e.Message);
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
        private static bool interfaceClaimed = false;

        public static bool IsDataloggingConnected =>
            sessionHandle != null && !sessionHandle.IsInvalid &&
            device_handle != null && !device_handle.IsInvalid;

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

            if (dev_handle == null || dev_handle.IsInvalid ||
                sessionHandle == null || sessionHandle.IsInvalid)
                return MonoUsbError.ErrorNoDevice;

            MonoUsbTransfer transfer = new MonoUsbTransfer(0);
            GCHandle gcUserCompleted = default(GCHandle);
            GCHandle gcBuffer = default(GCHandle);

            try
            {
                if (transfer.IsInvalid)
                    return MonoUsbError.ErrorNoMem;

                MonoUsbTransferDelegate callback = bulkTransferCB;
                int[] userCompleted = new int[] { 0 };
                gcUserCompleted = GCHandle.Alloc(userCompleted, GCHandleType.Pinned);
                gcBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                transfer.FillBulk(
                    dev_handle,
                    endpoint,
                    gcBuffer.AddrOfPinnedObject(),
                    length,
                    callback,
                    gcUserCompleted.AddrOfPinnedObject(),
                    timeout);

                MonoUsbError error = transfer.Submit();
                if ((int)error < 0)
                    return error;

                while (userCompleted[0] == 0)
                {
                    int result = Usb.HandleEvents(sessionHandle);
                    error = (MonoUsbError)result;

                    if (result >= 0)
                        continue;

                    if (error == MonoUsbError.ErrorInterrupted)
                        continue;

                    transfer.Cancel();

                    // Give libusb a chance to complete the cancellation before
                    // the transfer and pinned buffers are released.
                    while (userCompleted[0] == 0)
                    {
                        result = Usb.HandleEvents(sessionHandle);
                        if (result < 0 &&
                            (MonoUsbError)result != MonoUsbError.ErrorInterrupted)
                            break;
                    }

                    return error;
                }

                transferred = transfer.ActualLength;
                return MonoUsbApi.MonoLibUsbErrorFromTransferStatus(transfer.Status);
            }
            finally
            {
                if (!transfer.IsInvalid)
                    transfer.Free();

                if (gcBuffer.IsAllocated)
                    gcBuffer.Free();

                if (gcUserCompleted.IsAllocated)
                    gcUserCompleted.Free();
            }
        }

        public static UsbDevice MyUsbDevice;

        public static int getDataloggingDevice()
        {
            if (IsDataloggingConnected)
                return 0;

            // Never abandon a previous SafeHandle and leave cleanup to the
            // Mono finalizer thread. The crash report showed libusb_close()
            // being reached from SafeHandle.Finalize after a disconnect.
            releaseDataloggingDevice();

            try
            {
                sessionHandle = new MonoUsbSessionHandle();
                if (sessionHandle == null || sessionHandle.IsInvalid)
                    throw new Exception("Invalid USB session handle.");

                device_handle = MonoUsbApi.OpenDeviceWithVidPid(
                    sessionHandle, MY_VID, MY_PID);

                if (device_handle == null || device_handle.IsInvalid)
                {
                    releaseDataloggingDevice();
                    return 1;
                }

                // Configure and claim once per connection, not once per packet.
                int result = MonoUsbApi.SetConfiguration(device_handle, MY_CONFIG);
                if (result != 0)
                {
                    releaseDataloggingDevice();
                    return result;
                }

                result = MonoUsbApi.ClaimInterface(device_handle, MY_INTERFACE);
                if (result != 0)
                {
                    releaseDataloggingDevice();
                    return result;
                }

                interfaceClaimed = true;
                Debug.Log("CANUSB connected.");
                return 0;
            }
            catch (Exception e)
            {
                Debug.LogWarning("CANUSB connection failed: " + e.Message);
                releaseDataloggingDevice();
                return 1;
            }
        }

        public static int releaseDataloggingDevice()
        {
            MonoUsbDeviceHandle oldDevice = device_handle;
            MonoUsbSessionHandle oldSession = sessionHandle;

            // Clear the public state first so no subsequent call reuses handles
            // that are currently being torn down.
            device_handle = null;
            sessionHandle = null;

            try
            {
                if (oldDevice != null)
                {
                    if (!oldDevice.IsInvalid && interfaceClaimed)
                    {
                        try { MonoUsbApi.ReleaseInterface(oldDevice, MY_INTERFACE); }
                        catch (Exception e) { Debug.LogWarning("CANUSB release interface: " + e.Message); }
                    }

                    interfaceClaimed = false;

                    if (!oldDevice.IsClosed)
                        oldDevice.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("CANUSB device cleanup: " + e.Message);
            }
            finally
            {
                interfaceClaimed = false;
            }

            try
            {
                if (oldSession != null && !oldSession.IsClosed)
                    oldSession.Close();
            }
            catch (Exception e)
            {
                Debug.LogWarning("CANUSB session cleanup: " + e.Message);
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
                        if (getDataloggingDevice() > 0) { r = 1; break; }
                    }


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
                        Debug.LogWarning("Write failed: " + (MonoUsbError)r);
                        releaseDataloggingDevice();
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
                        Debug.LogWarning("Read failed: " + (MonoUsbError)r);
                        releaseDataloggingDevice();
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
                releaseDataloggingDevice();
                Debug.LogWarning("CANUSB operation failed: " + e.Message);
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

