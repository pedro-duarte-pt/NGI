using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core screen navigation service with a history stack. Navigation is still
/// declaratively requested by add-ons; only ScreenLoader performs the load.
/// </summary>
public static class ScreenNavigation
{
    private struct Location
    {
        public string AddonId;
        public string ScreenId;
        public Location(string addonId, string screenId)
        {
            AddonId = addonId;
            ScreenId = screenId;
        }
    }

    private static readonly Stack<Location> history = new Stack<Location>();
    private static ScreenLoader loader;
    private static Location? current;

    public static void Initialize(ScreenLoader screenLoader)
    {
        loader = screenLoader;
        history.Clear();
        current = null;
    }

    public static void NotifyLoaded(string addonId, string screenId)
    {
        current = new Location(addonId, screenId);
    }

    public static void Navigate(string screenId, string addonId = null)
    {
        if (loader == null || string.IsNullOrWhiteSpace(screenId))
            return;

        string targetAddon = !string.IsNullOrWhiteSpace(addonId)
            ? addonId
            : (current.HasValue ? current.Value.AddonId : null);

        if (string.IsNullOrWhiteSpace(targetAddon))
        {
            Debug.LogError("Navigation requires an addon id when no current screen exists.");
            return;
        }

        if (current.HasValue &&
            current.Value.AddonId == targetAddon &&
            current.Value.ScreenId == screenId)
            return;

        Location? previous = current;
        if (loader.TryLoadAddonScreen(targetAddon, screenId))
        {
            if (previous.HasValue)
                history.Push(previous.Value);
        }
    }

    public static void Back(string fallbackScreen, string fallbackAddon = null)
    {
        if (loader == null)
            return;

        while (history.Count > 0)
        {
            Location target = history.Pop();
            if (loader.TryLoadAddonScreen(target.AddonId, target.ScreenId))
                return;
        }

        if (string.IsNullOrWhiteSpace(fallbackScreen))
            return;

        string targetAddon = !string.IsNullOrWhiteSpace(fallbackAddon)
            ? fallbackAddon
            : (current.HasValue ? current.Value.AddonId : null);

        if (!string.IsNullOrWhiteSpace(targetAddon))
            loader.TryLoadAddonScreen(targetAddon, fallbackScreen);
    }
}
