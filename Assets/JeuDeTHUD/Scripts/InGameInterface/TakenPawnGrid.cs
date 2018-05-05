using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakenPawnGrid : MonoBehaviour {

    public bool ownerIsDwarfPlayer;
    public GameObject dwarfPawnIconPrefab;
    public GameObject trollPawnIconPrefab;

    //update the number of icon in the grid
    public void UpdateGrid()
    {
        ResetGrid();
        int takenPawnCount = 0;
        if (ownerIsDwarfPlayer)
        {
            takenPawnCount = BattleInformation.TakenTrollCount;
        }
        else
        {
            takenPawnCount = BattleInformation.TakenDwarfCount;
        }

        for(int i=0; i < takenPawnCount; i++) {
            AddTakenPawn();
        }

    }

    //remove all icon in the grid
    public void ResetGrid()
    {
        foreach(Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
    }

    //add one icon in the grid
    public void AddTakenPawn()
    {
        GameObject instantiatePawnIcon = null;
        if (ownerIsDwarfPlayer)
        {
            instantiatePawnIcon = Instantiate(trollPawnIconPrefab);
        }
        else
        {
            instantiatePawnIcon = Instantiate(dwarfPawnIconPrefab);
        }
        instantiatePawnIcon.transform.SetParent(transform, false);
    }


}
