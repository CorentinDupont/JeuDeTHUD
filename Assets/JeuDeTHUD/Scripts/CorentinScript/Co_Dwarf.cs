using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JeuDeThud.Battle;
namespace JeuDeThud.GameBoard.Pawn
{
    [RequireComponent(typeof(Co_Pawn))]
    public class Co_Dwarf : MonoBehaviour
    {
        private Co_Pawn Pawn { get { return GetComponent<Co_Pawn>(); } }
        private BattleManager BattleManager { get { return FindObjectOfType<BattleManager>(); } }

        public void AttackTroll(Co_Pawn trollPawn)
        {
            //Move the current pawn to the target dwarf board box
            Pawn.MoveTo(trollPawn.boardBox, false);

            //Kill the dwarf
            trollPawn.Die();

            //Add points to the player
            BattleManager.AddPointTo(true, 4);
            //Add taken pawn to the player
            BattleManager.AddTakenPawn(true, 1);

            //Block player to do another attack
            BattleInformation.PlayerHasMadeAnAttackInThisTurn = true;
        }
    }

}
