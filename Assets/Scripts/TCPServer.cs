using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json.Linq; // Add Newtonsoft.Json package via Unity Package Manager

public class TCPServer : MonoBehaviour
{
    private TcpListener server;
    private Thread serverThread;
    private bool isRunning = true;

    public string ip;
    public int port;

    void Start()
    {
        serverThread = new Thread(new ThreadStart(StartServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void StartServer()
    {
        try
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Debug.Log("TCP Server started on 127.0.0.1:8565");

            while (isRunning)
            {
                if (server.Pending())
                {
                    TcpClient client = server.AcceptTcpClient();
                    Debug.Log("Client connected!");

                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Server error: " + e.Message);
        }
    }

    void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (isRunning && client.Connected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.Log("Received: " + receivedMessage);

                    // Parse the JSON message
                    try
                    {
                        JObject json = JObject.Parse(receivedMessage);
                        string urlid = json["urlid"]?.ToString();
                        string code = json["code"]?.ToString();

                        Debug.Log($"Parsed urlid: {urlid}, code: {code}");

                        // Send acknowledgment to the client
                        string response = $"Received urlid: {urlid}, code: {code}";
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseData, 0, responseData.Length);
                    }
                    catch (Exception jsonEx)
                    {
                        Debug.LogError("JSON parsing error: " + jsonEx.Message);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Client error: " + e.Message);
        }

        client.Close();
        Debug.Log("Client disconnected.");
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        server?.Stop();
        serverThread?.Abort();
    }
}
