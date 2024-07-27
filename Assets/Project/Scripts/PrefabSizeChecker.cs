using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrefabSizeChecker : MonoBehaviour
{
    void OnEnable()
    {
        CheckChildSizes();
    }

    void CheckChildSizes()
    {
        foreach (Transform child in transform)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject) as GameObject;
            if (prefab != null)
            {
                Vector3 size = GetPrefabSize(prefab);
                Debug.Log("Prefab size of " + child.name + ": " + size);
            }
            else
            {
                Debug.LogWarning("No corresponding prefab found for " + child.name);
            }
        }
    }

    Vector3 GetPrefabSize(GameObject prefab)
    {
        string assetPath = AssetDatabase.GetAssetPath(prefab);
        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogWarning("Asset path is null or empty for " + prefab.name);
            return Vector3.zero;
        }

        GameObject instance = PrefabUtility.LoadPrefabContents(assetPath);
        Renderer renderer = instance.GetComponent<Renderer>();

        Vector3 size = Vector3.zero;
        if (renderer != null)
        {
            size = renderer.bounds.size;
        }
        else
        {
            Debug.LogWarning("Renderer not found on " + prefab.name);
        }

        PrefabUtility.UnloadPrefabContents(instance);
        return size;
    }
}
