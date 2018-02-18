using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    int unNombre = 1;
    string unMot = "azeaze";
    bool aze = true;

    GameObject cube;
    Transform trans;

    public GameObject nainPrefab;

    private float timeOnSpawnNain = 0;
    //bool canLaunchTimerForSpawnNain = false;
    float delaySpawnNain = 2;

    // Use this for initialization
    void Start()
    {
        cube = GameObject.Find("Cube");
        trans = cube.GetComponent<Transform>();

        SpawnNain();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(2, 0, 2);
        Vector3 w = new Vector3(0, 10, 0);

        if(timeOnSpawnNain + delaySpawnNain < Time.fixedTime)
        {
            SpawnNain();
        }


        //trans.Translate(v * Time.deltaTime, Space.World);
        //trans.Rotate(w * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("oui !");
    }

    void SpawnNain()
    {
        GameObject newNain = GameObject.Instantiate(nainPrefab, this.gameObject.transform.position, Quaternion.identity);
        timeOnSpawnNain = Time.fixedTime;
        //canLaunchTimerForSpawnNain = true;
    }
}