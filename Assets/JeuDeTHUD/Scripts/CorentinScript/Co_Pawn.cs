using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Co_Pawn : MonoBehaviour {

    public GameObject boardBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse0)){
            GameObject.FindWithTag("GameBoard").GetComponent<Co_GameBoard>().setSelectedPawn(this.gameObject);
        }
    }
}
