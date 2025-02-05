using UnityEngine;
using UnityEditor;
using System.IO;
using JetBrains.Annotations;

public class RuntimeAssetBundleCreator : MonoBehaviour
{

     



    public void CreateAndSaveAssetBundle()
    {
        string prefabPath = Path.Combine(Application.persistentDataPath, "GeneratedPrefab.prefab");
        string dllPath = Path.Combine(Application.persistentDataPath, "GeneratedScript.dll");
        string bundlePath = Path.Combine(Application.persistentDataPath, "GeneratedBundle");

        if (!File.Exists(prefabPath) || !File.Exists(dllPath))
        {
            Debug.LogError("Prefab or DLL file not found!");
            return;
        }

        AssetBundleBuild bundle = new AssetBundleBuild
        {
            assetBundleName = "dynamicbundle",
            assetNames = new string[] { prefabPath, dllPath }
        };

        BuildPipeline.BuildAssetBundles(bundlePath, new AssetBundleBuild[] { bundle }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log($"AssetBundle created at {bundlePath}");
    }
}
