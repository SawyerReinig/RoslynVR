
using UnityEngine;

public class CodingHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // gameObject.AddComponent<Rigidbody>(); 
        gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<Rigidbody>().AddForce(Vector3.up);
    }
}
