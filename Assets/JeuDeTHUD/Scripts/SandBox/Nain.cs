using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nain : MonoBehaviour {

    public GameObject casePosition;

    public AudioSource audioSource;
    public AudioClip attackSound;

	// Use this for initialization
	void Start () {
        this.gameObject.transform.position = casePosition.transform.position;
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
