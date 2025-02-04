using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityEditor;
public class ShapEManager : MonoBehaviour
{

    public string ServerURL;
    public string port;

    public GameObject GenPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)){

            createGenObject();



        }
        
    }

    public void createGenObject(){

        GameObject genObject = Instantiate(GenPrefab,Vector3.zero,Quaternion.identity);

        GenObject genScript =genObject.GetComponent<GenObject>();

        genScript.ID= IDGenerator.GenerateID();

        genScript.DownloadURL=ServerURL+":"+port;

        genScript.ServerURL=ServerURL+":"+port;






    }


        public void promptcreateShape(string prompt,GenObject genScript){

        // GameObject genObject = Instantiate(GenPrefab,Vector3.zero,Quaternion.identity);
        //genScript.ID= IDGenerator.GenerateID();
        genScript.DownloadURL=ServerURL+":"+port;
        genScript.ServerURL=ServerURL+":"+port;
        genScript.GenerateObject(prompt);






    }
}
