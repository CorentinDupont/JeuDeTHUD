using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Co_Pawn : MonoBehaviour {

    public GameObject boardBox;

    private Vector3 nextPawnPosition;
    private float startMovementTime;
    private Vector3 startMovementPosition;
    public string pawnLabel;

    // Use this for initialization
    void Start () {
        nextPawnPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //Test de la position du pion. Si la next position est amené à changer, le pion bougera.
        if (transform.position != nextPawnPosition)
        {
            transform.position = Vector3.Lerp(startMovementPosition, nextPawnPosition,(Time.time - startMovementTime) / 1);
        }
    }

    void OnMouseOver()
    {
        print(pawnLabel);
        //Si c'est un nains et que c'est le tour des nains ou que c'est un troll et ce n'est pas le tour des nains, et si il n'as pas déja fais une action avec un pion et si le joueur clique
        if (((BattleInformation.IsDwarfTurn && GetComponent<Co_Dwarf>()) || (!BattleInformation.IsDwarfTurn && GetComponent<Co_Troll>())) && !BattleInformation.PlayerHasMadeAnActionInHisTurn && Input.GetKey(KeyCode.Mouse0)){
            GameObject.FindWithTag("GameBoard").GetComponent<Co_GameBoard>().SetSelectedPawn(this.gameObject);

        }
    }

    public void MoveTo(GameObject boardBox)
    {
        nextPawnPosition = new Vector3(boardBox.transform.position.x, boardBox.transform.position.y + GetComponent<Renderer>().bounds.size.y / 2 + boardBox.GetComponent<Renderer>().bounds.size.y / 2, boardBox.transform.position.z);
        transform.SetParent(boardBox.transform);
        this.boardBox = boardBox;
        startMovementPosition = transform.position;
        startMovementTime = Time.time;

        BattleInformation.PlayerHasMadeAnActionInHisTurn = true;
    }
}
