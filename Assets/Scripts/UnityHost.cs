using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnityEditor;

public class UnityHost : MonoBehaviour
{
    public GameObject prefabToSend;
    private TcpListener server;

    void Start()
    {
        // Start TCP server to send AssetBundles to clients
        server = new TcpListener(System.Net.IPAddress.Any, 8080);
        server.Start();
        Debug.Log("Host is ready to send AssetBundles.");

        string dllPath = CompileCodeToDLL(@"
        using UnityEngine;
        public class DynamicScript : MonoBehaviour
        {
            void Start() { Debug.Log(""Dynamic script loaded and running!""); }
        }");

        if (dllPath != null)
        {
            string assetBundlePath = CreateAssetBundle(dllPath);
            SendAssetBundle(assetBundlePath);
        }
    }

    private string CompileCodeToDLL(string code)
    {
        string dllPath = Path.Combine(Application.persistentDataPath, "GeneratedScript.dll");

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a => MetadataReference.CreateFromFile(a.Location));

        var compilation = CSharpCompilation.Create("DynamicAssembly")
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

        using (var stream = new FileStream(dllPath, FileMode.Create))
        {
            var result = compilation.Emit(stream);
            if (!result.Success)
            {
                Debug.LogError("DLL Compilation Failed!");
                return null;
            }
        }
        Debug.Log($"DLL compiled at {dllPath}");
        return dllPath;
    }

    private string CreateAssetBundle(string dllPath)
    {
        string bundlePath = Path.Combine(Application.persistentDataPath, "DynamicBundle");

        // Save DLL as an asset
        string targetDllPath = Path.Combine("Assets/StreamingAssets", "GeneratedScript.dll");
        File.Copy(dllPath, targetDllPath, true);

        // Build AssetBundle with prefab and DLL
        AssetBundleBuild bundle = new AssetBundleBuild
        {
            assetBundleName = "dynamicbundle",
            assetNames = new string[]
            {
                AssetDatabase.GetAssetPath(prefabToSend),
                targetDllPath
            }
        };

        BuildPipeline.BuildAssetBundles(bundlePath, new AssetBundleBuild[] { bundle }, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log("AssetBundle Created!");

        return Path.Combine(bundlePath, "dynamicbundle");
    }

    private void SendAssetBundle(string assetBundlePath)
    {
        TcpClient client = server.AcceptTcpClient();
        NetworkStream stream = client.GetStream();
        byte[] fileData = File.ReadAllBytes(assetBundlePath);

        stream.Write(fileData, 0, fileData.Length);
        stream.Close();
        client.Close();

        Debug.Log("AssetBundle sent successfully!");
    }
}
