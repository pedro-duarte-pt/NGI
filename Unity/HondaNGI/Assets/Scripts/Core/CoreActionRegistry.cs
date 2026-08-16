using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlled bridge between declarative add-ons and core-owned application actions.
/// Add-ons can request only actions explicitly registered by the core.
/// </summary>
public static class CoreActionRegistry
{
    private static readonly Dictionary<string, Action> Actions =
        new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);

    public static void Register(string id, Action action)
    {
        if (string.IsNullOrWhiteSpace(id) || action == null)
            return;
        Actions[id] = action;
    }

    public static void Unregister(string id)
    {
        if (!string.IsNullOrWhiteSpace(id))
            Actions.Remove(id);
    }

    public static bool Invoke(string id)
    {
        if (string.IsNullOrWhiteSpace(id) || !Actions.TryGetValue(id, out Action action))
        {
            Debug.LogWarning("Unknown or unavailable core action: " + id);
            return false;
        }
        action();
        return true;
    }
}
