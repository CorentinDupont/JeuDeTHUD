using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JeuDeThud.Battle.UI
{
    public class HUDLink : MonoBehaviour
    {

        public TakenPawnGrid player1TakenPawnGrid;
        public TakenPawnGrid player2TakenPawnGrid;

        public TurnText turnText;

        public TurnBanner dwarfTurnBanner;
        public TurnBanner trollTurnBanner;

        public Image stopRoundModal;

        public RectTransform scoreTable;

        public RectTransform player2ThinkingBlock;

        public Button nextTurnButton;
        public Button surrenderButton;
    }
}

