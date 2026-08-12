using System.IO;
using UnityEngine;

public class AddonLoader : MonoBehaviour
{
    [SerializeField]
    private ScreenLoader screenLoader;

    [SerializeField]
    private string startupAddonId = "core.default";

    [SerializeField]
    private string startupScreenId = "diagnostics";

    private void Start()
    {
        LoadAllAddons();

        if (screenLoader == null)
        {
            Debug.LogError(
                "AddonLoader requires a ScreenLoader reference."
            );

            return;
        }

        if (string.IsNullOrWhiteSpace(startupAddonId))
        {
            Debug.LogError(
                "AddonLoader startupAddonId is empty."
            );

            return;
        }

        if (string.IsNullOrWhiteSpace(startupScreenId))
        {
            Debug.LogError(
                "AddonLoader startupScreenId is empty."
            );

            return;
        }

        screenLoader.LoadAddonScreen(
            startupAddonId,
            startupScreenId
        );
    }

    public void LoadAllAddons()
    {
        AddonRegistry.Clear();

        string addonsRoot = Path.Combine(
            Application.streamingAssetsPath,
            "Addons"
        );

        if (!Directory.Exists(addonsRoot))
        {
            Debug.LogWarning(
                "Addons folder not found: " +
                addonsRoot
            );

            return;
        }

        string[] addonFolders =
            Directory.GetDirectories(addonsRoot);

        foreach (string addonFolder in addonFolders)
            TryLoadAddon(addonFolder);
    }

    private void TryLoadAddon(string addonFolder)
    {
        string manifestPath = Path.Combine(
            addonFolder,
            "addon.json"
        );

        if (!File.Exists(manifestPath))
        {
            Debug.LogWarning(
                "Skipping addon folder without addon.json: " +
                addonFolder
            );

            return;
        }

        string json = File.ReadAllText(manifestPath);

        AddonManifest manifest =
            JsonUtility.FromJson<AddonManifest>(json);

        if (manifest == null)
        {
            Debug.LogError(
                "Could not deserialize addon manifest: " +
                manifestPath
            );

            return;
        }

        bool compatible =
            AddonCompatibility.IsCompatible(
                manifest,
                out string reason
            );

        if (!compatible)
        {
            Debug.LogError(
                "[ADDON INCOMPATIBLE] " +
                manifest.name +
                " v" +
                manifest.version +
                " | " +
                reason
            );

            return;
        }

        LoadedAddon loadedAddon =
            new LoadedAddon(
                manifest,
                addonFolder
            );

        if (!AddonRegistry.Register(loadedAddon))
        {
            Debug.LogError(
                "Could not register addon or duplicate addon id: " +
                manifest.id
            );

            return;
        }

        int screenCount =
            manifest.screens != null
            ? manifest.screens.Length
            : 0;

        Debug.Log(
            "[ADDON OK] " +
            manifest.name +
            " v" +
            manifest.version +
            " | " +
            reason +
            " | " +
            screenCount +
            " screen(s)"
        );
    }
}
