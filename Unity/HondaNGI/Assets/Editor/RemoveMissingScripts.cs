using UnityEditor;
using UnityEngine;

public static class RemoveMissingScripts
{
    [MenuItem("NGI/Tools/Remove Missing Scripts From Scene")]
    public static void Remove()
    {
        int total = 0;

        foreach (GameObject go in Object.FindObjectsByType<GameObject>(
                     FindObjectsInactive.Include,
                     FindObjectsSortMode.None))
        {
            int count =
                GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

            if (count <= 0)
                continue;

            Debug.Log(
                $"Removing {count} missing script(s) from: {go.name}",
                go
            );

            Undo.RegisterCompleteObjectUndo(
                go,
                "Remove Missing Scripts"
            );

            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);

            total += count;
        }

        Debug.Log($"Removed {total} missing script reference(s).");
    }
}