using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

    int unNombre = 1;
    string unMot = "azeaze";
    bool aze = true;

    GameObject cube;
    Transform trans;

    // Use this for initialization
    void Start () {
        cube = GameObject.Find("Cube");
        trans = cube.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = new Vector3(2, 0, 2);
        Vector3 w = new Vector3(0, 10, 0);

        //trans.Translate(v * Time.deltaTime, Space.World);
        //trans.Rotate(w * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("oui !");
    }
}
