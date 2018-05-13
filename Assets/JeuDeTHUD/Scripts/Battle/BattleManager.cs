using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HUDLink))]//Require the script HUDLink to work
[RequireComponent(typeof(OnlineBattleManager))]
public class BattleManager : MonoBehaviour {

    private HUDLink HudLink { get { return GetComponent<HUDLink>(); } }
    private OnlineBattleManager OnlineBattleManager { get { return GetComponent<OnlineBattleManager>(); } }

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
        BattleInformation.ShotCount = 1;
        BattleInformation.IsDwarfTurn = true;
        BattleInformation.PlayerHasMadeAnActionInHisTurn = false;

        

        if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
        {
            BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
            BattleInformation.TrollPlayer = new Player("IA");
        }
        else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
        {
            //Est ce que le joueur courant est le créateur de la partie : 
            if(Network.player.ipAddress == BattleInformation.OnlineGameInfo.starter)
            {
                BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
                BattleInformation.TrollPlayer = new Player("Online Player");
            }
            else
            {
                BattleInformation.DwarfPlayer = new Player("Online Player");
                BattleInformation.TrollPlayer = GameInformation.GetCurrentPlayer();
            }
        }
        else
        {
            BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
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

        if(BattleInformation.DwarfPlayer == GameInformation.GetCurrentPlayer())
        {
            //Keep the player to play his turn
        }
        else
        {
            

            if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1) {
                //Unallow current player to do an action
                BattleInformation.PlayerHasMadeAnActionInHisTurn = true;

                //IA Turn
            }
            else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
            {
                //Unallow current player to do an action
                BattleInformation.PlayerHasMadeAnActionInHisTurn = true;

                //Disable buttons
                SetButtonsState(false);

                //Show online player loading shot block
                HudLink.player2ThinkingBlock.gameObject.SetActive(true);

                //Wait online player shot
                OnlineBattleManager.WaitOtherPlayerShot();
            }
            else
            {
                //Keep the player 2 to play his turn
            }
           
        }
    }

    //Use to switch dwarf and troll turn
    public void NextTurn()
    {
        //Unselect selected pawn
        FindObjectOfType<Co_GameBoard>().SetSelectedPawn(null);


        
        

        //Change play side
        BattleInformation.IsDwarfTurn = !BattleInformation.IsDwarfTurn;
        if (BattleInformation.IsDwarfTurn)
        {
            BattleInformation.Turn++;
            HudLink.turnText.UpdateText();
        }

        BattleInformation.ShotCount++;

        if ((BattleInformation.DwarfPlayer == GameInformation.GetCurrentPlayer() && BattleInformation.IsDwarfTurn) || (BattleInformation.TrollPlayer == GameInformation.GetCurrentPlayer() && !BattleInformation.IsDwarfTurn))
        {
            //Disable buttons
            SetButtonsState(true);

            //Show online player loading shot block
            HudLink.player2ThinkingBlock.gameObject.SetActive(false);

            //Autorize player to do an action
            BattleInformation.PlayerHasMadeAnActionInHisTurn = false;

            //Keep the player to play his turn
        }
        else
        {
            if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
            {
                //IA Turn
            }
            else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
            {
                //Disable buttons
                SetButtonsState(false);

                //Show online player loading shot block
                HudLink.player2ThinkingBlock.gameObject.SetActive(true);

                //Wait online player shot
                OnlineBattleManager.WaitOtherPlayerShot();
            }
            else
            {
                //Keep the player 2 to play his turn
                //Allow next player to make an action
                BattleInformation.PlayerHasMadeAnActionInHisTurn = false;
            }
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

            //Continue upgrade shot count
            BattleInformation.ShotCount++;

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

    //Change state of "real life player" buttons
    private void SetButtonsState(bool areInteractable)
    {
        HudLink.nextTurnButton.interactable = areInteractable;
        HudLink.surrenderButton.interactable = areInteractable;
    }
}
