using UnityEngine;
using UnityEditor;
using System.IO;
using JetBrains.Annotations;

public class RuntimeAssetBundleCreator : MonoBehaviour
{

    void Update(){

        if(Input.GetKeyDown(KeyCode.F2)){
CreateAndSaveAssetBundle();

        }


    } 

     



    public void CreateAndSaveAssetBundle()
    {
        string prefabPath = Path.Combine(Application.streamingAssetsPath, "GeneratedPrefab.prefab");
        string dllPath = Path.Combine(Application.streamingAssetsPath, "DynamicScript.dll");
        string bundlePath = Path.Combine(Application.streamingAssetsPath);

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
