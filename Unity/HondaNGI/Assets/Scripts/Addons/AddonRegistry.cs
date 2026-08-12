using System;
using System.Collections.Generic;

public sealed class LoadedAddon
{
    public AddonManifest Manifest { get; }
    public string RootPath { get; }

    public LoadedAddon(AddonManifest manifest, string rootPath)
    {
        Manifest = manifest;
        RootPath = rootPath;
    }
}

public static class AddonRegistry
{
    private static readonly Dictionary<string, LoadedAddon> addons =
        new Dictionary<string, LoadedAddon>(StringComparer.OrdinalIgnoreCase);

    public static IEnumerable<LoadedAddon> All => addons.Values;

    public static void Clear()
    {
        addons.Clear();
    }

    public static bool Register(LoadedAddon addon)
    {
        if (addon == null ||
            addon.Manifest == null ||
            string.IsNullOrWhiteSpace(addon.Manifest.id))
        {
            return false;
        }

        if (addons.ContainsKey(addon.Manifest.id))
            return false;

        addons.Add(addon.Manifest.id, addon);
        return true;
    }

    public static bool TryGet(string addonId, out LoadedAddon addon)
    {
        return addons.TryGetValue(addonId, out addon);
    }

    public static bool TryGetScreen(
        string addonId,
        string screenId,
        out LoadedAddon addon,
        out AddonScreenEntry screen)
    {
        screen = null;

        if (!TryGet(addonId, out addon))
            return false;

        if (addon.Manifest.screens == null)
            return false;

        foreach (AddonScreenEntry candidate in addon.Manifest.screens)
        {
            if (candidate == null)
                continue;

            if (string.Equals(
                    candidate.id,
                    screenId,
                    StringComparison.OrdinalIgnoreCase))
            {
                screen = candidate;
                return true;
            }
        }

        return false;
    }
}
