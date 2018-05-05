using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HUDLink))]//Require the script HUDLink to work
public class BattleManager : MonoBehaviour {

    private HUDLink HudLink { get { return GetComponent<HUDLink>(); } }

	void Start () {
        InitializeBattle();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitializeBattle()
    {
        BattleInformation.RoundNum = 1;
        BattleInformation.Turn = 1;
        BattleInformation.IsDwarfTurn = true;
        BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();

        if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
        {
            BattleInformation.TrollPlayer = new Player("IA");
        }
        else if(PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
        {
            BattleInformation.TrollPlayer = new Player("Online Player");
        }
        else
        {
            BattleInformation.TrollPlayer = GameInformation.GetPlayer2();
        }

        BattleInformation.DwarfPlayerPoint = 0;
        BattleInformation.TrollPlayerPoint = 0;
        BattleInformation.TakenDwarfCount = 0;
        BattleInformation.TakenTrollCount = 0;

        HudLink.player1TakenPawnGrid.UpdateGrid();
        HudLink.player2TakenPawnGrid.UpdateGrid();

        HudLink.turnText.UpdateText();

        ShowTurnBanner();

    }

    public void NextTurn()
    {
        BattleInformation.Turn++;
        BattleInformation.IsDwarfTurn = !BattleInformation.IsDwarfTurn;
        ShowTurnBanner();

    }

    private void ShowTurnBanner()
    {
        if (BattleInformation.IsDwarfTurn)
        {
            HudLink.dwarfTurnBanner.gameObject.SetActive(true);
        }
        else
        {
            HudLink.trollTurnBanner.gameObject.SetActive(true);
        }
    }

   
}
