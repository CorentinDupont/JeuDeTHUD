using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JeuDeThud.Battle;

namespace JeuDeThud.GameBoard.Pawn
{
    [RequireComponent(typeof(Co_Pawn))]
    public class Co_Troll : MonoBehaviour
    {
        private Co_Pawn Pawn { get { return GetComponent<Co_Pawn>(); } }
        private BattleManager BattleManager { get { return FindObjectOfType<BattleManager>(); } }

        public void AttackDwarf(Co_Pawn dwarfPawn)
        {
            //Move the current pawn to the target dwarf board box
            Pawn.MoveTo(dwarfPawn.boardBox, false);

            //Kill the dwarf
            dwarfPawn.Die();

            //Add points to the player
            BattleManager.AddPointTo(false, 1);
            //Add taken pawn to the player
            BattleManager.AddTakenPawn(false, 1);
        }
    }
}

