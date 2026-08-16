using System.Collections;
using UnityEngine;
using integrationBoard;

public class AppController : MonoBehaviour
{
    private const int DataloggingPacketSize = 64;
    private const float ConnectedPollDelaySeconds = 0.001f;
    private const float ReconnectDelaySeconds = 1.0f;

    private Coroutine dataLoggingCoroutine;

    private void Start()
    {
        dataLoggingCoroutine = StartCoroutine(DataLoggingLoop());
    }

    private IEnumerator DataLoggingLoop()
    {
        while (true)
        {
            int result = DeviceLib.getDataloggingData(DataloggingPacketSize);

            if (result == 0 && DeviceLib.IsDataloggingConnected)
                yield return new WaitForSeconds(ConnectedPollDelaySeconds);
            else
                yield return new WaitForSeconds(ReconnectDelaySeconds);
        }
    }

    private void OnApplicationQuit()
    {
        if (dataLoggingCoroutine != null)
        {
            StopCoroutine(dataLoggingCoroutine);
            dataLoggingCoroutine = null;
        }

        DeviceLib.releaseDataloggingDevice();
    }
}
