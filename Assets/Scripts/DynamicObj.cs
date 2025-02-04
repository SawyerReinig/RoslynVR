using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObj : MonoBehaviour
{
    public GenObject genObject;

    public string ID;



    public void setID(string id){

        ID = id;
        genObject.ID = id;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
