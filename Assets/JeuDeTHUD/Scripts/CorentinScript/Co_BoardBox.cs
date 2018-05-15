using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JeuDeThud.GameBoard.Pawn;
using JeuDeThud.Battle;

namespace JeuDeThud.GameBoard
{
    public class Co_BoardBox : MonoBehaviour
    {

        public Vector2 coordinate;
        public Material normalMaterial;
        public Material movementMaterial;
        public Material attackMaterial;
        public Material hoverMovementMaterial;
        public Material hoverAttackMaterial;
        public bool isMarkedForMovement;
        public bool isMarkedForAttack;
        public string boardBoxLabel;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
                        hit.transform.gameObject.GetComponent<Co_BoardBox>().isMarkedForMovement = true;
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
                        hit.transform.gameObject.GetComponent<Co_BoardBox>().isMarkedForMovement = true;
                    }
                }
            }
        }
        
        public void LookForDwarfAttack(Vector3 direction)
        {
            RaycastHit hit;



            if (Physics.Raycast(transform.position, direction, out hit, 2))
            {
                if (hit.transform.gameObject.GetComponent<Co_BoardBox>())
                {
                    if (hit.transform.gameObject.transform.childCount != 0)
                    {
                        hit.transform.gameObject.GetComponent<Renderer>().material = attackMaterial;
                        hit.transform.gameObject.GetComponent<Co_BoardBox>().isMarkedForAttack = true;
                        hit.transform.gameObject.GetComponent<Co_BoardBox>().LookForDwarfMovement(direction);
                    }
                }
            }
        }

        public void LookForTrollAttack(Vector3 direction)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, 2))
            {
                if (hit.transform.gameObject.GetComponent<Co_BoardBox>())
                {
                    if (hit.transform.gameObject.transform.childCount != 0)
                    {
                        hit.transform.gameObject.GetComponent<Renderer>().material = attackMaterial;
                        hit.transform.gameObject.GetComponent<Co_BoardBox>().isMarkedForAttack = true;
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            if (isMarkedForMovement)
            {
                this.GetComponent<Renderer>().material = hoverMovementMaterial;
            }
        }

        private void OnMouseExit()
        {
            if (isMarkedForMovement)
            {
                this.GetComponent<Renderer>().material = movementMaterial;
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //print(this.GetComponent<Co_BoardBox>().boardBoxLabel);
                if (isMarkedForMovement)
                {
                    //Save movement data from current player
                    //start board box
                    BattleInformation.currentPlayerShot.slot_1 = this.GetComponentInParent<Co_GameBoard>().GetSelectedPawn().GetComponentInParent<Co_BoardBox>().boardBoxLabel;
                    //end board box
                    BattleInformation.currentPlayerShot.slot_2 = this.boardBoxLabel;
                    //Mouvement pion
                    this.GetComponentInParent<Co_GameBoard>().GetSelectedPawn().GetComponent<Co_Pawn>().MoveTo(this.gameObject, false);

                    //On remet l'apparence du plateau de départ
                    this.GetComponentInParent<Co_GameBoard>().ResetGameBoardBoxesAspect();
                    //Désélectionne le pion
                    this.GetComponentInParent<Co_GameBoard>().SetSelectedPawn(null);
                }
            }
        }
    }
}

