using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Co_GameBoard : MonoBehaviour {

    public GameObject boardBoxPrefab;
    public Material darkBoardBoxMaterial;
    public Material lightBoardBoxMaterial;
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

                    //Instantiation d'une case
                    GameObject currentBoardBox = GameObject.Instantiate(boardBoxPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
                    currentBoardBox.transform.SetParent(this.gameObject.transform);

                    //Change la couleur
                    if (boardBoxId % 2 == 0)
                    {
                        currentBoardBox.GetComponent<Renderer>().material = darkBoardBoxMaterial;
                    }
                    else
                    {
                        currentBoardBox.GetComponent<Renderer>().material = lightBoardBoxMaterial;
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
}
