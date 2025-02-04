using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using RealityEditor;
using System;
using System.Text;

public class GenObject : MonoBehaviour
{


    public string DownloadURL;
    public string ServerURL;
    
   
    public string ID;
    public ModelDownloader modelDownloader;
    public GameObject Target;


    Coroutine FileCheck;



    public string DebugPrompt;


    // public string prompt;
    // Start is called before the first frame update
    void Start()
    {
        modelDownloader = FindObjectOfType<ModelDownloader>();
       // FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL + ID + "_ShapE.zip"));
    }

       public Renderer Genobjrenderer;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)){
            GenerateObject(DebugPrompt);
        }    


        if(Target.transform.childCount>0){
            Genobjrenderer=Target.GetComponentInChildren<Renderer>();
            Genobjrenderer.material.shader = Shader.Find("VertexColorShader");


        }
        
    }



    public void GenerateObject(string prompt){

        StartCoroutine(SendTheCommand(ServerURL+"/generate",prompt ,ID));
        FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL +"/download/"+ ID +"_ShapE.zip"));


        // ""http://localhost:5000/download/example_file.zip";"

    }



        public void downloadModel(string url, GameObject warp)
    {
        modelDownloader.AddTask(
            new ModelIformation()
            {
                ModelURL = url,
                gameobjectWarp = warp
            }
        );

        // loadingIcon.SetActive(false);
        // loadingParticles.Stop();
        // SmoothCubeRenderer.enabled = false;

        modelDownloader.startDownload();
    }





     IEnumerator CheckURLPeriodically(string urltocheck)
    {
        yield return new WaitForSeconds(10f);
        while (true)
        {
            yield return CheckURL(urltocheck);
            yield return new WaitForSeconds(checkInterval);
        }
    }

     public float checkInterval = 5f; // Check the URL every 5 seconds
    public event Action<bool> OnURLResponse = delegate { };

     IEnumerator CheckURL(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation requestAsyncOperation = www.SendWebRequest();

        while (!requestAsyncOperation.isDone)
        {
            yield return null;
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("URL is responding!");
            StopCoroutine(FileCheck);
            FileCheck=null;
            downloadModel(url, Target);
            OnURLResponse(true);
        }
        else
        {
            //  Debug.LogError("Error checking URL: " + www.error);
            OnURLResponse(false);
        }

        www.Dispose();
    }


  public IEnumerator SendTheCommand(string url, string command, string urlId)
    {
        if (!string.IsNullOrEmpty(command))
        {
            // Create the JSON payload
            string jsonPayload = $"{{\"prompt\":\"{command}\",\"URID\":\"{urlId}\",\"filename\":\"{urlId}_ShapE\"}}";

            // Create a UnityWebRequest for POST
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return request.SendWebRequest();

            // Handle the response
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Command sent successfully: " + command);
                Debug.Log("Response: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        else
        {
            Debug.LogError("Command is empty!");
        }
    }
}




