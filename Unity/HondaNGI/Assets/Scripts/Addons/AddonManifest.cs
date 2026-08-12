using System;

[Serializable]
public class AddonManifest
{
    public string id;
    public string name;
    public string version;
    public AddonCompatibilityInfo compatibility;
    public AddonScreenEntry[] screens;
}

[Serializable]
public class AddonCompatibilityInfo
{
    public string minAddonApi;
    public string maxAddonApi;
}

[Serializable]
public class AddonScreenEntry
{
    public string id;

    // Relative to the addon root folder.
    // Example: "screens/diagnostics"
    public string path;
}
