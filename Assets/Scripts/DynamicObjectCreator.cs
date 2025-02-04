using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityEditor;

public class DynamicObjectCreator : MonoBehaviour
{
    public string urlid;

    public string Shape;

    public string Behaviour;

    public string ParticleConfig;

    // public SHAPERuntime sHAPERuntime;

    public  RuntimeScriptManager runtimeScriptManager;
     public   ShapEManager shapEManager;

    public GameObject prefabObject;

    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CreateObj(){

         GameObject genObject = Instantiate(prefabObject,Vector3.zero,Quaternion.identity);

         DynamicObj dynamicObj=genObject.GetComponent<DynamicObj>();
         dynamicObj.setID(IDGenerator.GenerateID());
         shapEManager.promptcreateShape(Shape, dynamicObj.genObject);
         runtimeScriptManager.Target=genObject;
         runtimeScriptManager.callCompileScript(Behaviour);




        // sHAPERuntime.sendPrompt(Shape,urlid);




        //











    }
}
