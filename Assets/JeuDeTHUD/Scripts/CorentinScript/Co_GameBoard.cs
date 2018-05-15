using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JeuDeThud.API;
using JeuDeThud.Battle;
using JeuDeThud.Util;
using JeuDeThud.GameBoard.Pawn;

namespace JeuDeThud.GameBoard
{
    [RequireComponent(typeof(DebugLogComponent))]
    public class Co_GameBoard : MonoBehaviour
    {

        public GameObject boardBoxPrefab;
        public Material darkBoardBoxMaterial;
        public Material lightBoardBoxMaterial;

        public GameObject dwarfPrefab;
        public GameObject trollPrefab;
        public GameObject thudStonePrefab;

        public Material selectedPawnMaterial;
        public Material boardBoxMovementMaterial;

        public bool anObstacleWasEncountered = false;

        private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

        private int gameBoardSizeX = 15;
        private int gameBoardSizeY = 15;
        private int dwarfCount = 0;
        private int trollCount = 0;

        private GameObject selectedPawn;
        private Material normalSelectedPawnMaterial;

        private ShotInfo lastContructedShot;


        void Start()
        {
            this.GetComponent<Renderer>().enabled = false;
            GenerateThudGameBoard();
            print("D2 is on case : " + FindPawnByLabel("D2").GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().boardBoxLabel);
        }

        void Update()
        {

        }

        private void GenerateThudGameBoard()
        {
            //Position en bas à gauche de la première case du plateau (position plateau - taille du plateau/2 + taille d'une case /2)
            float xPos = this.transform.position.x - this.gameObject.GetComponent<Renderer>().bounds.size.x / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.x / 2;
            float zPos = this.transform.position.z - this.gameObject.GetComponent<Renderer>().bounds.size.z / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.z / 2;
            int boardBoxId = 0;
            //Pour chaque ligne
            for (int i = 0; i < gameBoardSizeY; i++)
            {
                //Pour chaque colonne
                for (int j = 0; j < gameBoardSizeX; j++)
                {

                    //Test de si la case est dans un coin (voir plateau THUD)
                    if ((i < 5 && (j >= 5 - i && j < 10 + i)) || (i >= 10 && (j > i - 10 && j < 14 + 10 - i)) || (i >= 5 && i < 10))
                    {

                        //Création d'une case
                        GameObject currentBoardBox = SpawnBoardBox(i, j, xPos, zPos, boardBoxId);


                        //Pose d'un Nain
                        if ((i == 0 && j != 7) // Ligne 0
                            || (j == 0 && i != 7) //Colonne 0
                            || (j == 14 && i != 7) //Ligne 14
                            || (i == 14 && j != 7) // Colonne 14
                            || (i < 5 && (j == 5 - i || j == 9 + i)) //Bord bas gauche et bas droite
                            || (i >= 10 && (j == i - 9 || j == 14 + 9 - i))) // Bord haut gauche et haut droite
                        {
                            SpawnPawn(dwarfPrefab, currentBoardBox);
                        }


                        //Pose des trolls
                        if ((i >= 6 && i <= 8) && (j >= 6 && j <= 8) && !(i == 7 && j == 7))
                        {
                            SpawnPawn(trollPrefab, currentBoardBox);
                        }

                        if ((i == 7 && j == 7))
                        {
                            SpawnPawn(thudStonePrefab, currentBoardBox);
                        }
                    }

                    //Modifie la prochaine position en x
                    xPos += boardBoxPrefab.GetComponent<Renderer>().bounds.size.x + 0.01f;
                    boardBoxId++;
                }

                //Modifie la prochaine position en z
                zPos += boardBoxPrefab.GetComponent<Renderer>().bounds.size.z + 0.01f;

                //Réinitialize la position en x, pour commencer une nouvelle ligne
                xPos = this.transform.position.x - this.gameObject.GetComponent<Renderer>().bounds.size.x / 2 + boardBoxPrefab.GetComponent<Renderer>().bounds.size.x / 2;
            }
        }

        private GameObject SpawnBoardBox(int i, int j, float xPos, float zPos, int boardBoxId)
        {
            //Instantiation d'une case
            GameObject currentBoardBox = GameObject.Instantiate(boardBoxPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
            currentBoardBox.transform.SetParent(this.gameObject.transform);

            currentBoardBox.GetComponent<Co_BoardBox>().coordinate = new Vector2(i, j);

            currentBoardBox.GetComponent<Co_BoardBox>().boardBoxLabel = System.Convert.ToChar(j + 65).ToString() + (15 - i).ToString();

            //print(currentBoardBox.GetComponent<Co_BoardBox>().boardBoxLabel);

            //Change la couleur
            if (boardBoxId % 2 == 0)
            {
                currentBoardBox.GetComponent<Renderer>().material = darkBoardBoxMaterial;
            }
            else
            {
                currentBoardBox.GetComponent<Renderer>().material = lightBoardBoxMaterial;
            }

            //Enregistrement de son apparence de départ
            currentBoardBox.GetComponent<Co_BoardBox>().normalMaterial = currentBoardBox.GetComponent<Renderer>().material;

            return currentBoardBox;
        }

        private void SpawnPawn(GameObject pawnPrefab, GameObject currentBoardBox)
        {
            GameObject currentPawn = GameObject.Instantiate(pawnPrefab, currentBoardBox.transform.position, Quaternion.identity);
            currentPawn.transform.SetParent(currentBoardBox.transform);
            currentPawn.transform.position = new Vector3(currentPawn.transform.position.x, currentPawn.transform.position.y + currentPawn.GetComponent<Renderer>().bounds.size.y / 2 + currentBoardBox.GetComponent<Renderer>().bounds.size.y / 2, currentPawn.transform.position.z);
            currentPawn.GetComponent<Co_Pawn>().boardBox = currentBoardBox;

            string[] dwarfCases = new string[32] { "I1", "J1", "K2", "L3", "M4", "N5", "O6", "O7", "O9", "O10", "N11", "M12", "L13", "K14", "J15", "I15", "G15", "F15", "E14", "D13", "C12", "B11", "A10", "A9", "A7", "A6", "B5", "C4", "D3", "E2", "F1", "G1" };
            string[] trollCases = new string[8] { "H7", "I7", "I8", "I9", "H9", "G9", "G8", "G7" };

            if (pawnPrefab == dwarfPrefab)
            {
                foreach (string dwarfCase in dwarfCases)
                {
                    if (currentBoardBox.GetComponent<Co_BoardBox>().boardBoxLabel.Equals(dwarfCase))
                    {
                        currentPawn.GetComponent<Co_Pawn>().pawnLabel = "D" + (Array.IndexOf(dwarfCases, dwarfCase) + 1).ToString();
                        dwarfCount++;
                    }
                }
            }
            else
            {
                foreach (string trollCase in trollCases)
                {
                    if (currentBoardBox.GetComponent<Co_BoardBox>().boardBoxLabel.Equals(trollCase))
                    {
                        currentPawn.GetComponent<Co_Pawn>().pawnLabel = "T" + (Array.IndexOf(trollCases, trollCase) + 1).ToString();
                        trollCount++;
                    }
                }
            }
        }

        public void SetSelectedPawn(GameObject clickedPawn)
        {

            //test if a pawn is already selected
            if (selectedPawn != null)
            {
                //On rend son apparence ordinaire
                selectedPawn.GetComponent<Renderer>().material = normalSelectedPawnMaterial;
                ResetGameBoardBoxesAspect();
            }

            //Si le pion cliqué est autre chose que la thudstone
            if (clickedPawn != null && clickedPawn.gameObject.tag != "ThudStone")
            {
                selectedPawn = clickedPawn;
                BattleInformation.currentPlayerShot.pawn = clickedPawn.GetComponent<Co_Pawn>().pawnLabel;
                normalSelectedPawnMaterial = clickedPawn.GetComponent<Renderer>().material;
                clickedPawn.GetComponent<Renderer>().material = selectedPawnMaterial;
                showMovementPossibilities();
            }
            else
            {
                selectedPawn = null;
            }

        }

        public GameObject GetSelectedPawn()
        {
            return selectedPawn;
        }

        private void showMovementPossibilities()
        {
            //Coordonnées du pion sélectionné
            Vector2 selectedPawnCoordinate = selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().coordinate;

            //Si le pion sélectionné est un nain
            if (selectedPawn.GetComponent<Co_Dwarf>())
            {

                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(-Vector3.right);//Left
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(-Vector3.forward);//Bottom
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(Vector3.right);//Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(Vector3.forward);//Top
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(new Vector3(1, 0, 1));//Top Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(new Vector3(-1, 0, 1));//Top Left
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(new Vector3(1, 0, -1));//Bottom Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForDwarfMovement(new Vector3(-1, 0, -1));//Bottom Right

            }
            else//Sinon si c'est un troll
            {

                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(-Vector3.right);//Left
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(-Vector3.forward);//Bottom
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(Vector3.right);//Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(Vector3.forward);//Top
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(new Vector3(1, 0, 1));//Top Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(new Vector3(-1, 0, 1));//Top Left
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(new Vector3(1, 0, -1));//Bottom Right
                selectedPawn.GetComponent<Co_Pawn>().boardBox.GetComponent<Co_BoardBox>().LookForTrollMovement(new Vector3(-1, 0, -1));//Bottom Right
            }
        }

        public void ResetGameBoardBoxesAspect()
        {
            foreach (Transform currentBoardBoxTransform in this.transform)
            {
                currentBoardBoxTransform.gameObject.GetComponent<Renderer>().material = currentBoardBoxTransform.gameObject.GetComponent<Co_BoardBox>().normalMaterial;
                currentBoardBoxTransform.gameObject.GetComponent<Co_BoardBox>().isMarkedForMovement = false;
            }
        }

        //Reproduce a shot received by online player or IA
        public void ReproduceConstructedShot(ShotInfo shot, bool haveReproduceMove, bool haveReproduceAttack)
        {
            DebugLog.DebugMessage("Start reproducing shot", true);

            ShotInfo shotToReproduce;

            if (shot == null)
            {
                DebugLog.DebugMessage("Keep same shot as before", true);
                shotToReproduce = this.lastContructedShot;
            }
            else
            {
                DebugLog.DebugMessage("It's a new shot !", true);
                shotToReproduce = shot;
                lastContructedShot = shotToReproduce;
            }

            //Move the pawn
            if (!haveReproduceMove)
            {
                if (shotToReproduce.slot_1 != shotToReproduce.slot_2)
                {
                    DebugLog.DebugMessage("Ask to Reproduce Movement", true);
                    FindBoardBoxByLabel(shotToReproduce.slot_1).transform.GetComponentInChildren<Co_Pawn>().MoveTo(FindBoardBoxByLabel(shotToReproduce.slot_2), true);
                }
                else
                {
                    ReproduceConstructedShot(null, true, false);
                }
            }
            else if (!haveReproduceAttack)
            {
                DebugLog.DebugMessage("Ask to Reproduce Attack", true);
                //Reproduce attack ...

                //Temporary skip this task
                ReproduceConstructedShot(null, true, true);
            }
            else
            {
                DebugLog.DebugMessage("Ask to pass turn", true);

                //Next Turn
                GameObject.FindObjectOfType<BattleManager>().NextTurn();
            }
        }

        public GameObject FindBoardBoxByLabel(string label)
        {
            foreach (Transform boardBox in this.transform)
            {
                if (boardBox.gameObject.GetComponent<Co_BoardBox>().boardBoxLabel.Equals(label))
                {
                    return boardBox.gameObject;
                }
            }
            return null;
        }


        public GameObject FindPawnByLabel(string label)
        {
            foreach (Transform boardBox in this.transform)
            {
                if (boardBox.transform.GetComponentInChildren<Co_Pawn>() != null && boardBox.transform.GetComponentInChildren<Co_Pawn>().pawnLabel.Equals(label))
                {
                    return boardBox.transform.GetComponentInChildren<Co_Pawn>().gameObject;
                }
            }
            return null;
        }
    }
}
