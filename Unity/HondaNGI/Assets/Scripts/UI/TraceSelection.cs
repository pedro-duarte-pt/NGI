using System;
using System.Collections.Generic;

public static class TraceSelection
{
    private static readonly List<string> selectedSensors = new List<string>();

    public static event Action Changed;

    public static IReadOnlyList<string> SelectedSensors => selectedSensors;

    public static int MaxTraces { get; set; } = 4;

    public static bool IsSelected(string sensorId)
    {
        return selectedSensors.Contains(sensorId);
    }

    public static bool Toggle(string sensorId)
    {
        if (string.IsNullOrWhiteSpace(sensorId))
            return false;

        if (selectedSensors.Contains(sensorId))
        {
            selectedSensors.Remove(sensorId);
            Changed?.Invoke();
            return false;
        }

        if (selectedSensors.Count >= MaxTraces)
            return false;

        selectedSensors.Add(sensorId);
        Changed?.Invoke();
        return true;
    }

    public static void SetInitial(IEnumerable<string> sensorIds)
    {
        selectedSensors.Clear();

        if (sensorIds != null)
        {
            foreach (string id in sensorIds)
            {
                if (selectedSensors.Count >= MaxTraces)
                    break;

                if (!string.IsNullOrWhiteSpace(id) &&
                    SensorRegistry.Get(id) != null &&
                    !selectedSensors.Contains(id))
                {
                    selectedSensors.Add(id);
                }
            }
        }

        Changed?.Invoke();
    }

    public static void Clear()
    {
        selectedSensors.Clear();
        Changed?.Invoke();
    }
}
