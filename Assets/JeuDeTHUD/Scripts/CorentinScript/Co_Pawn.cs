using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Co_Pawn : MonoBehaviour {

    public GameObject boardBox;
    public string pawnLabel;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnMouseOver()
    {
        //print(pawnLabel);
        //Si c'est un nains et que c'est le tour des nains ou que c'est un troll et ce n'est pas le tour des nains, et si il n'as pas déja fais une action avec un pion et si le joueur clique
        if (((BattleInformation.IsDwarfTurn && GetComponent<Co_Dwarf>()) || (!BattleInformation.IsDwarfTurn && GetComponent<Co_Troll>())) && !BattleInformation.PlayerHasMadeAnActionInHisTurn && Input.GetKey(KeyCode.Mouse0)){
            GameObject.FindWithTag("GameBoard").GetComponent<Co_GameBoard>().SetSelectedPawn(this.gameObject);
            
        }
    }

    public void MoveTo(GameObject boardBox, bool notHumanAction)
    {
        Vector3 nextPawnPosition = new Vector3(boardBox.transform.position.x, boardBox.transform.position.y + GetComponent<Renderer>().bounds.size.y / 2 + boardBox.GetComponent<Renderer>().bounds.size.y / 2, boardBox.transform.position.z);
        transform.SetParent(boardBox.transform);
        this.boardBox = boardBox;
        Vector3 startMovementPosition = transform.position;
        float startMovementTime = Time.time;

        //disallow player to select another pawn
        BattleInformation.PlayerHasMadeAnActionInHisTurn = true;

        //Launch Movement
        StartCoroutine(MovePawnTo(AfterMovement, nextPawnPosition, startMovementPosition, startMovementTime, notHumanAction));
    }

    private IEnumerator MovePawnTo(Action<bool> onSuccess, Vector3 nextPawnPosition, Vector3 startMovementPosition, float startMovementTime, bool notHumanAction)
    {
        
        while (transform.position != nextPawnPosition)
        {
            transform.position = Vector3.Lerp(startMovementPosition, nextPawnPosition, (Time.time - startMovementTime) / 1);
            yield return null;
        }
        onSuccess(notHumanAction);

        

    }

    private void AfterMovement(bool notHumanAction)
    {
        //If this action was make by data and not by user interaction (from IA, or Online Player)
        if (notHumanAction)
        {
            //Continue reproducing
            GameObject.FindObjectOfType<Co_GameBoard>().ReproduceConstructedShot(null, true, false);
        }
        else
        {
            

            //check if it can do an attack ...
        }
    }
}
