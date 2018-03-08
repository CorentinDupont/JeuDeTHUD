using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Co_BoardBox : MonoBehaviour {

    public Vector2 coordinate;
    public Material normalMaterial;
    public Material movementMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LookForDwarfMovement(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 2))
        {
            if (hit.transform.gameObject.GetComponent<Co_BoardBox>())
            {
                if (hit.transform.gameObject.transform.childCount == 0)
                {
                    hit.transform.gameObject.GetComponent<Renderer>().material = movementMaterial;
                    hit.transform.gameObject.GetComponent<Co_BoardBox>().LookForDwarfMovement(direction);
                }
            }
        }
    }

    public void LookForTrollMovement(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 2))
        {
            if (hit.transform.gameObject.GetComponent<Co_BoardBox>())
            {
                if (hit.transform.gameObject.transform.childCount == 0)
                {
                    hit.transform.gameObject.GetComponent<Renderer>().material = movementMaterial;
                }
            }
        }
    }
}
