using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Co_GameBoard : MonoBehaviour {

    public GameObject boardBoxPrefab;
    public Material darkBoardBoxMaterial;
    public Material lightBoardBoxMaterial;

    public GameObject dwarfPrefab;
    public GameObject trollPrefab;
    public GameObject thudStonePrefab;

    private int gameBoardSizeX = 15;
    private int gameBoardSizeY = 15;

	void Start () {
        this.GetComponent<Renderer>().enabled = false;
        GenerateThudGameBoard();
    }
	
	void Update () {
		
	}

    void GenerateThudGameBoard() {
        //Position en bas à gauche de la première case du plateau (position plateau - taille du plateau/2 + taille d'une case /2)
        float xPos = this.transform.position.x - this.gameObject.GetComponent<Renderer>().bounds.size.x / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.x / 2;
        float zPos = this.transform.position.z - this.gameObject.GetComponent<Renderer>().bounds.size.z / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.z / 2;
        int boardBoxId = 0;
        //Pour chaque ligne
        for(int i=0; i<gameBoardSizeY; i++) {
            //Pour chaque colonne
            for(int j=0; j<gameBoardSizeX; j++){
                //Test de si la case est dans un coin (voir plateau THUD)
                if ((i < 5 && (j >= 5 - i && j < 10 + i)) || (i>=10 && (j > i-10 && j < 14 + 10 - i)) || (i>=5 && i<10)) {

                    //Création d'une case
                    GameObject currentBoardBox = SpawnBoardBox(i, j, xPos, zPos, boardBoxId);

                    //Pose d'un Nain
                    if((i == 0 && j != 7) // Ligne 0
                        || (j == 0 && i != 7) //Colonne 0
                        || (j == 14 && i != 7) //Ligne 14
                        || (i == 14 && j != 7) // Colonne 14
                        || (i < 5 && (j == 5 - i || j == 9 + i)) //Bord bas gauche et bas droite
                        || (i >= 10 && (j == i - 9 || j == 14 + 9 - i))) // Bord haut gauche et haut droite
                    {
                        SpawnPawn(dwarfPrefab, currentBoardBox);
                    }


                    //Pose des trolls
                    if ((i >= 6 && i <= 8) && (j >= 6 && j <= 8) && !(i == 7 && j == 7)) {
                        SpawnPawn(trollPrefab, currentBoardBox);
                    }

                    if ((i == 7 && j == 7)) {
                        SpawnPawn(thudStonePrefab, currentBoardBox);
                    }
                }

                //Modifie la prochaine position en x
                xPos += boardBoxPrefab.GetComponent<Renderer>().bounds.size.x;
                boardBoxId++;
            }

            //Modifie la prochaine position en z
            zPos += boardBoxPrefab.GetComponent<Renderer>().bounds.size.z;

            //Réinitialize la position en x, pour commencer une nouvelle ligne
            xPos = this.transform.position.x - this.gameObject.GetComponent<Renderer>().bounds.size.x / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.x / 2;
        }  
    }

    private GameObject SpawnBoardBox(int i, int j, float xPos, float zPos, int boardBoxId) {
        //Instantiation d'une case
        GameObject currentBoardBox = GameObject.Instantiate(boardBoxPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
        currentBoardBox.transform.SetParent(this.gameObject.transform);

        currentBoardBox.GetComponent<Co_BoardBox>().coordinate = new Vector2(i, j);

        //Change la couleur
        if (boardBoxId % 2 == 0)
        {
            currentBoardBox.GetComponent<Renderer>().material = darkBoardBoxMaterial;
        }
        else
        {
            currentBoardBox.GetComponent<Renderer>().material = lightBoardBoxMaterial;
        }

        return currentBoardBox;
    }

    private void SpawnPawn(GameObject pawnPrefab, GameObject currentBoardBox)
    {
        GameObject currentPawn = GameObject.Instantiate(pawnPrefab, currentBoardBox.transform.position, Quaternion.identity);
        currentPawn.transform.SetParent(currentBoardBox.transform);
        currentPawn.transform.position = new Vector3(currentPawn.transform.position.x, currentPawn.transform.position.y + currentPawn.GetComponent<Renderer>().bounds.size.y / 2 + currentBoardBox.GetComponent<Renderer>().bounds.size.y / 2, currentPawn.transform.position.z);
        currentPawn.GetComponent<Co_Pawn>().boardBox = currentBoardBox;
    }
}
