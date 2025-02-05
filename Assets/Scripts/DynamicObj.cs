using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class DynamicObj : MonoBehaviour
{
    public GenObject genObject;

    public string ID;
    //public Dictionary<string, string> Log = null;
    public List<string> Log = new List<string>();


    public void setID(string id){

        ID = id;
        genObject.ID = id;

    }
    // Start is called before the first frame update
    void Start()
    {

        if(Log.Count > 0){

          //  UpdateTheScript(Log[0]);

            //print("GenerateCode is"+Log[1]);


            AddScriptFromDLL(gameObject,Log[0],Log[0]);



        }
        
    }



    void UpdateTheScript(string script)
    {
       // string scriptName = "MyScript"; // Replace with your script name
        // GameObject myObject = new GameObject("DynamicObject");

        Type scriptType = Type.GetType(script);
        if (scriptType != null)
        {
            this.gameObject.AddComponent(scriptType);
        }
        else
        {
            Debug.LogError("Script not found: " + script);
        }
    }

    public static void AddScriptFromDLL(GameObject targetObject, string dllName, string scriptName)
    {
        try
        {
            // Load the assembly
            Assembly assembly = Assembly.Load(dllName);
            if (assembly == null)
            {
                Debug.LogError($"Failed to load assembly: {dllName}");
                return;
            }

            // Get the script Type from the DLL
            Type scriptType = assembly.GetType(scriptName);
            if (scriptType == null)
            {
                Debug.LogError($"Script '{scriptName}' not found in {dllName}");
                return;
            }

            // Add the script dynamically
            targetObject.AddComponent(scriptType);
            Debug.Log($"Successfully added script '{scriptName}' from {dllName} to {targetObject.name}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading script '{scriptName}' from {dllName}: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
