using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HUDLink))]//Require the script HUDLink to work
public class BattleManager : MonoBehaviour {

    private HUDLink HudLink { get { return GetComponent<HUDLink>(); } }

    void Start() {
        InitializeBattle();
    }

    // Update is called once per frame
    void Update() {

    }

    private void InitializeBattle()
    {
        BattleInformation.RoundNum = 1;
        BattleInformation.Turn = 1;
        BattleInformation.IsDwarfTurn = true;
        BattleInformation.PlayerHasMadeAnActionInHisTurn = false;
        BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();

        if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
        {
            BattleInformation.TrollPlayer = new Player("IA");
        }
        else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
        {
            BattleInformation.TrollPlayer = new Player("Online Player");
        }
        else
        {
            BattleInformation.TrollPlayer = GameInformation.GetPlayer2();
        }

        BattleInformation.Player1Point = 0;
        BattleInformation.Player2Point = 0;
        BattleInformation.TakenDwarfCount = 0;
        BattleInformation.TakenTrollCount = 0;

        HudLink.player1TakenPawnGrid.UpdateGrid();
        HudLink.player2TakenPawnGrid.UpdateGrid();

        HudLink.turnText.UpdateText();

        ShowTurnBanner();

    }

    //Use to switch dwarf and troll turn
    public void NextTurn()
    {
        //Unselect selected pawn
        FindObjectOfType<Co_GameBoard>().SetSelectedPawn(null);

        //Allow next player to make an action
        BattleInformation.PlayerHasMadeAnActionInHisTurn = false;

        //Change play side
        BattleInformation.IsDwarfTurn = !BattleInformation.IsDwarfTurn;
        if (BattleInformation.IsDwarfTurn)
        {
            BattleInformation.Turn++;
            HudLink.turnText.UpdateText();
        }

        ShowTurnBanner();

    }

    //Show the turn banner
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

    public void AskStopRound()
    {
        HudLink.stopRoundModal.gameObject.SetActive(true);
        NextTurn();
    }

    public void NextRound()
    {
        if (BattleInformation.RoundNum == 1)
        {
            //Next round !
            BattleInformation.RoundNum = 2;

            //Switch players
            Player player1 = BattleInformation.DwarfPlayer;
            Player player2 = BattleInformation.TrollPlayer;
            BattleInformation.DwarfPlayer = player2;
            BattleInformation.TrollPlayer = player1;

            //Reset some parameters
            BattleInformation.TakenDwarfCount = 0;
            BattleInformation.TakenTrollCount = 0;
            BattleInformation.Turn = 1;
            BattleInformation.IsDwarfTurn = true;
            BattleInformation.PlayerHasMadeAnActionInHisTurn = false;

            //Change pawn type in taken pawn grids
            HudLink.player1TakenPawnGrid.ownerIsDwarfPlayer = false;
            HudLink.player2TakenPawnGrid.ownerIsDwarfPlayer = true;

            //Update the taken pawn grids
            HudLink.player1TakenPawnGrid.UpdateGrid();
            HudLink.player2TakenPawnGrid.UpdateGrid();

            //Update Turn Text
            HudLink.turnText.UpdateText();

            ShowTurnBanner();
        }
        else if (BattleInformation.RoundNum == 2)
        {
            HudLink.scoreTable.gameObject.SetActive(true);
        }
    }
    public void ReturnToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
