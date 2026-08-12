using System;

public static class AddonCompatibility
{
    public static bool IsCompatible(AddonManifest addon, out string reason)
    {
        if (addon == null)
        {
            reason = "Addon manifest is null.";
            return false;
        }

        if (addon.compatibility == null)
        {
            reason = "Addon has no compatibility metadata.";
            return false;
        }

        if (!TryParseVersion(AppInfo.AddonApiVersion, out Version runtimeVersion))
        {
            reason = "Application Addon API version is invalid: " + AppInfo.AddonApiVersion;
            return false;
        }

        if (!TryParseVersion(addon.compatibility.minAddonApi, out Version minVersion))
        {
            reason = "Addon minAddonApi is invalid: " + addon.compatibility.minAddonApi;
            return false;
        }

        if (runtimeVersion < minVersion)
        {
            reason = "Addon requires Addon API " + minVersion +
                     " or newer. Application provides " + runtimeVersion + ".";
            return false;
        }

        if (!string.IsNullOrWhiteSpace(addon.compatibility.maxAddonApi))
        {
            if (!TryParseVersion(addon.compatibility.maxAddonApi, out Version maxVersion))
            {
                reason = "Addon maxAddonApi is invalid: " + addon.compatibility.maxAddonApi;
                return false;
            }

            if (runtimeVersion > maxVersion)
            {
                reason = "Addon supports Addon API up to " + maxVersion +
                         ". Application provides " + runtimeVersion + ".";
                return false;
            }
        }

        reason = "Compatible with Addon API " + AppInfo.AddonApiVersion + ".";
        return true;
    }

    private static bool TryParseVersion(string value, out Version version)
    {
        version = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Version.TryParse(value, out version);
    }
}
