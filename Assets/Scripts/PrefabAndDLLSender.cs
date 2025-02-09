using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class PrefabAndDLLSender : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    public string ip;

    void Start()
    {
        SendFiles();
    }

    void SendFiles()
    {
        string prefabPath = Path.Combine(Application.streamingAssetsPath, "GeneratedPrefab.prefab");
        string dllPath = Path.Combine(Application.streamingAssetsPath, "DynamicScript.dll");

        if (!File.Exists(prefabPath) || !File.Exists(dllPath))
        {
            Debug.LogError("❌ Prefab or DLL file not found!");
            return;
        }

        client = new TcpClient(ip, 8080); // Change to your Meta Quest's IP
        stream = client.GetStream();

        // Send Prefab
        byte[] prefabData = File.ReadAllBytes(prefabPath);
        stream.Write(prefabData, 0, prefabData.Length);
        
        // Send DLL
        byte[] dllData = File.ReadAllBytes(dllPath);
        stream.Write(dllData, 0, dllData.Length);

        stream.Close();
        client.Close();

        Debug.Log("✅ Prefab and DLL sent successfully to Meta Quest!");
    }
}
