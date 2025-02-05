using UnityEngine;
using System.IO;
using UnityEditor;

public class RuntimePrefabCreator : MonoBehaviour
{
    public GameObject objectToPrefab; // Assign this in the inspector

    void Update()
    {
        // Press 'P' to create a prefab at runtime
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGameObjectAsPrefab(objectToPrefab);
        }
    }

    private void SaveGameObjectAsPrefab(GameObject go)
    {


        MonoBehaviour[] scripts = objectToPrefab.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            Debug.Log("Attached script: " + script.GetType().Name);
           
        }


        string prefabPath = Path.Combine(Application.streamingAssetsPath, "GeneratedPrefab.prefab");

        // Create a new prefab asset
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

        Debug.Log($"Prefab saved at: {prefabPath}");
    }
}
