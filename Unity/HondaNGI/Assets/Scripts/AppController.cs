using System.Collections;
using UnityEngine;
using integrationBoard;

public class AppController : MonoBehaviour
{
    private Coroutine dataLoggingCoroutine;

    private void Start()
    {
        dataLoggingCoroutine = StartCoroutine(DataLoggingLoop());
    }

    private IEnumerator DataLoggingLoop()
    {
        while (true)
        {
            DeviceLib.getDataloggingData(64);

            yield return new WaitForSeconds(0.001f);
        }
    }

    private void OnApplicationQuit()
    {
        if (dataLoggingCoroutine != null)
        {
            StopCoroutine(dataLoggingCoroutine);
        }

        DeviceLib.releaseDataloggingDevice();
    }

    void Update()
    {

    }

}