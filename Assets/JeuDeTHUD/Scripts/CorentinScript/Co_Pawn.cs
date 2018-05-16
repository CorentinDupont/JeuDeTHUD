using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using JeuDeThud.Battle;
using JeuDeThud.GameBoard;
using JeuDeThud.Util;

namespace JeuDeThud.GameBoard.Pawn
{
    [RequireComponent(typeof(DebugLogComponent))]
    public class Co_Pawn : MonoBehaviour
    {

        public GameObject boardBox;
        public string pawnLabel;
        public bool isMarkedForAttack;
        public Material normalMaterial;
        public Material markForAttackMaterial;

        private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

        // Use this for initialization
        void Start()
        {
            normalMaterial = GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseOver()
        {
            //si le joueur clique
            if (Input.GetKey(KeyCode.Mouse0))
            {
                DebugLog.DebugMessage("Click on pawn", true);
                //Si c'est un nains et que c'est le tour des nains ou que c'est un troll et ce n'est pas le tour des nains, et si il n'as pas déja fais une action avec un pion
                if (((BattleInformation.IsDwarfTurn && GetComponent<Co_Dwarf>()) || (!BattleInformation.IsDwarfTurn && GetComponent<Co_Troll>())) && !BattleInformation.PlayerHasMadeAnActionInHisTurn)
                {
                    DebugLog.DebugMessage("Select the pawn", true);
                    GameObject.FindWithTag("GameBoard").GetComponent<Co_GameBoard>().SetSelectedPawn(this.gameObject);
                }
                else if(isMarkedForAttack && (BattleInformation.IsDwarfTurn && GetComponent<Co_Troll>()))
                {
                    DebugLog.DebugMessage("A Dwarf want to attack this Troll Pawn !", true);
                    FindObjectOfType<Co_GameBoard>().GetSelectedPawn().GetComponent<Co_Dwarf>().AttackTroll(this);
                    FindObjectOfType<Co_GameBoard>().ResetPawnState();
                    FindObjectOfType<Co_GameBoard>().ResetGameBoardBoxesAspect();

                }
                else if(isMarkedForAttack && (!BattleInformation.IsDwarfTurn && GetComponent<Co_Dwarf>()))
                {
                    DebugLog.DebugMessage("A Troll want to attack this Dwarf Pawn !", true);
                    FindObjectOfType<Co_GameBoard>().GetSelectedPawn().GetComponent<Co_Troll>().AttackDwarf(this);
                    FindObjectOfType<Co_GameBoard>().ResetPawnState();
                    FindObjectOfType<Co_GameBoard>().ResetGameBoardBoxesAspect();
                }
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
            print("Débutmouv issou");
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
         print("Finmouv issou");
            //If this action was make by data and not by user interaction (from IA, or Online Player)
            if (notHumanAction)
            {
                //Continue reproducing
                GameObject.FindObjectOfType<Co_GameBoard>().ReproduceConstructedShot(null, true, false);
            }
            else
            {
                // GameObject.FindObjectOfType<Co_GameBoard>().ShowAttackPossibilities();
                // Cac
                //check if it can do an attack ...
                if(BattleInformation.PlayerHasMadeAnAttackInThisTurn == false)
                {
                    FindObjectOfType<Co_GameBoard>().CheckPawnLines();
                }
                else
                {
                    
                }
                
                
            }
        }

        //Active possiility to attack this pawn
        public void MarkForAttack(bool isMarkedForAttack)
        {
            this.isMarkedForAttack = isMarkedForAttack;
            if (isMarkedForAttack)
            {
                GetComponent<Renderer>().material = markForAttackMaterial;
            }
            else
            {
                ResetAspect();
            }
            
        }

        //Reset the aspect of the pawn
        public void ResetAspect()
        {
            GetComponent<Renderer>().material = normalMaterial;
        }

        //When the pawn is attacked by other pawn
        public void Die()
        {
            Destroy(this.gameObject);
        }
    }
}

