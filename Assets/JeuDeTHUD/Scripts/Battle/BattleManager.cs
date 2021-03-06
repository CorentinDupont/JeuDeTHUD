﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using JeuDeThud.API;
using JeuDeThud.Battle.UI;
using JeuDeThud.Util;
using JeuDeThud.Save;
using JeuDeThud.GameBoard;

namespace JeuDeThud.Battle
{
    [RequireComponent(typeof(HUDLink))]//Require the script HUDLink to work
    [RequireComponent(typeof(OnlineBattleManager))]
    [RequireComponent(typeof(DebugLogComponent))]
    public class BattleManager : MonoBehaviour
    {

        public HUDLink HudLink { get { return GetComponent<HUDLink>(); } }
        private OnlineBattleManager OnlineBattleManager { get { return GetComponent<OnlineBattleManager>(); } }
        private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

        void Start()
        {
            InitializeBattle();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitializeBattle()
        {

            DebugLog.DebugMessage("Initialize battle ...", true);
            BattleInformation.RoundNum = 1;
            BattleInformation.Turn = 1;
            BattleInformation.ShotCount = 1;
            BattleInformation.IsDwarfTurn = true;
            BattleInformation.PlayerHasMadeAnActionInHisTurn = false;
            BattleInformation.currentPlayerShot = new ShotInfo();



            if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
            {
                DebugLog.DebugMessage("Battle is VS IA", true);
                DebugLog.DebugMessage("Current player play Dwarf, IA play Troll", true);
                BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
                BattleInformation.TrollPlayer = new Player("IA");
                BattleInformation.TrollPlayer.ID = -1;
            }
            else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
            {
                DebugLog.DebugMessage("Battle is VS Online Player", true);
                //Est ce que le joueur courant est le créateur de la partie : 
                if (Network.player.ipAddress == BattleInformation.OnlineGameInfo.starter)
                {
                    DebugLog.DebugMessage("Current player play Dwarf, Online player play Troll", true);
                    BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
                    BattleInformation.TrollPlayer = new Player("Online Player");
                    BattleInformation.TrollPlayer.ID = -2;
                }
                else
                {
                    DebugLog.DebugMessage("Current player play Troll, Online Player play Dwarf", true);
                    BattleInformation.DwarfPlayer = new Player("Online Player");
                    BattleInformation.DwarfPlayer.ID = -2;
                    BattleInformation.TrollPlayer = GameInformation.GetCurrentPlayer();
                }
            }
            else
            {
                DebugLog.DebugMessage("Battle is VS Local Player", true);
                DebugLog.DebugMessage("Current player play Dwarf, Player2 play Troll", true);
                BattleInformation.DwarfPlayer = GameInformation.GetCurrentPlayer();
                BattleInformation.TrollPlayer = GameInformation.GetPlayer2();
            }


            DebugLog.DebugMessage("Init Points", true);
            BattleInformation.Player1Point = 0;
            BattleInformation.Player2Point = 0;
            BattleInformation.TakenDwarfCount = 0;
            BattleInformation.TakenTrollCount = 0;

            DebugLog.DebugMessage("Init HUD", true);
            HudLink.player1TakenPawnGrid.UpdateGrid();
            HudLink.player2TakenPawnGrid.UpdateGrid();

            HudLink.turnText.UpdateText();

            ShowTurnBanner();

            DebugLog.DebugMessage("Set parameters to the player who play his turn ...", true);
            DebugLog.DebugMessage("Dwarf Player name : " + BattleInformation.DwarfPlayer.Name + "     Current Player name : " + GameInformation.GetCurrentPlayer().Name, false);

            if (BattleInformation.DwarfPlayer.ID == GameInformation.GetCurrentPlayer().ID)
            {
                BattleInformation.currentPlayerShot = new ShotInfo();
                DebugLog.DebugMessage("Wait for current player action", true);
                //Keep the player to play his turn
            }
            else
            {


                if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
                {
                    DebugLog.DebugMessage("Wait for IA action", true);

                    //Unallow current player to do an action
                    BattleInformation.PlayerHasMadeAnActionInHisTurn = true;

                    //IA Turn
                }
                else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
                {
                    DebugLog.DebugMessage("Wait for online player action", true);

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
                    DebugLog.DebugMessage("Wait for local player 2 action", true);

                    //Keep the player 2 to play his turn
                }

            }
        }
        public void EndOfTurn()
        {
            if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1 && (BattleInformation.DwarfPlayer.ID == GameInformation.GetCurrentPlayer().ID && BattleInformation.IsDwarfTurn) || (BattleInformation.TrollPlayer.ID == GameInformation.GetCurrentPlayer().ID && !BattleInformation.IsDwarfTurn))
            {
                OnlineBattleManager.PrintShotInfo(BattleInformation.currentPlayerShot);
                //Add missing values to current player shot
                BattleInformation.currentPlayerShot.id_game = BattleInformation.OnlineGameInfo.id_game;
                BattleInformation.currentPlayerShot.id_shot = BattleInformation.ShotCount;
                //Send current player shot info to the online player via API
                OnlineBattleManager.SendCurrentPlayerShot(BattleInformation.currentPlayerShot);
            }
            else
            {
                NextTurn();
            }

        }

        //Use to switch dwarf and troll turn
        public void NextTurn()
        {
            //Unselect selected pawn
            FindObjectOfType<Co_GameBoard>().SetSelectedPawn(null);

            BattleInformation.ShotCount++;

            //Change play side
            BattleInformation.IsDwarfTurn = !BattleInformation.IsDwarfTurn;
            if (BattleInformation.IsDwarfTurn)
            {
                BattleInformation.Turn++;
                HudLink.turnText.UpdateText();
            }


            DebugLog.DebugMessage("trollPlayerId : " + BattleInformation.TrollPlayer.ID + " | current player ID : " + GameInformation.GetCurrentPlayer().ID + " | isDwarfTurn : " + BattleInformation.IsDwarfTurn, false);
            //If this turn is for current player
            if ((BattleInformation.DwarfPlayer.ID == GameInformation.GetCurrentPlayer().ID && BattleInformation.IsDwarfTurn) || (BattleInformation.TrollPlayer.ID == GameInformation.GetCurrentPlayer().ID && !BattleInformation.IsDwarfTurn))
            {


                //Disable buttons
                SetButtonsState(true);

                //Show online player loading shot block
                HudLink.player2ThinkingBlock.gameObject.SetActive(false);

                //Autorize player to do an action
                BattleInformation.PlayerHasMadeAnActionInHisTurn = false;
                BattleInformation.PlayerHasMadeAnAttackInThisTurn = false;

                DebugLog.DebugMessage("Wait for current player action", true);
                //Keep the player to play his turn
            }
            else
            {
                if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
                {
                    //IA Turn
                    DebugLog.DebugMessage("Wait for IA action", true);

                }
                else if (PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
                {

                    //Disable buttons
                    SetButtonsState(false);

                    //Show online player loading shot block
                    HudLink.player2ThinkingBlock.gameObject.SetActive(true);

                    DebugLog.DebugMessage("Wait for online player action", true);

                    //Wait online player shot
                    OnlineBattleManager.WaitOtherPlayerShot();
                }
                else
                {

                    DebugLog.DebugMessage("Wait for local player 2 action", true);

                    //Keep the player 2 to play his turn
                    //Allow next player to make an action
                    BattleInformation.PlayerHasMadeAnActionInHisTurn = false;
                    BattleInformation.PlayerHasMadeAnAttackInThisTurn = false;
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
            BattleInformation.currentPlayerShot.surrender = true;
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

                //Reset GameBoard
                FindObjectOfType<Co_GameBoard>().ResetThudGameBoard();

                ShowTurnBanner();
            }
            else if (BattleInformation.RoundNum == 2)
            {
                HudLink.scoreTable.gameObject.SetActive(true);
            }
        }
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        //Change state of "real life player" buttons
        private void SetButtonsState(bool areInteractable)
        {
            HudLink.nextTurnButton.interactable = areInteractable;
            HudLink.surrenderButton.interactable = areInteractable;
        }

        public void AddPointTo(bool isDwarfPlayer, int earnPoint)
        {
            if (isDwarfPlayer)
            {
                if(BattleInformation.DwarfPlayer.ID == GameInformation.GetCurrentPlayer().ID)
                {
                    DebugLog.DebugMessage("Player 1, who play dwarfs, win " + earnPoint + " points ! ", true);
                    BattleInformation.Player1Point += earnPoint;
                }
                else
                {
                    DebugLog.DebugMessage("Player 2, who play dwarfs, win " + earnPoint + " points ! ", true);
                    BattleInformation.Player2Point += earnPoint;
                }
            }
            else
            {
                if (BattleInformation.TrollPlayer.ID == GameInformation.GetCurrentPlayer().ID)
                {
                    DebugLog.DebugMessage("Player 1, who play trolls, win " + earnPoint + " points ! ", true);
                    BattleInformation.Player1Point += earnPoint;
                }
                else
                {
                    DebugLog.DebugMessage("Player 2, who play trolls, win " + earnPoint + " points ! ", true);
                    BattleInformation.Player2Point += earnPoint;
                }
            }

            
        }

        public void AddTakenPawn(bool isDwarfPlayer, int count)
        {
            for(int i=0; i<count; i++)
            {
                if (isDwarfPlayer)
                {
                    if (BattleInformation.DwarfPlayer.ID == GameInformation.GetCurrentPlayer().ID)
                    {
                        HudLink.player1TakenPawnGrid.AddTakenPawn();
                    }
                    else
                    {
                        HudLink.player2TakenPawnGrid.AddTakenPawn();
                    }
                }
                else
                {
                    if (BattleInformation.TrollPlayer.ID == GameInformation.GetCurrentPlayer().ID)
                    {
                        HudLink.player1TakenPawnGrid.AddTakenPawn();
                    }
                    else
                    {
                        HudLink.player2TakenPawnGrid.AddTakenPawn();
                    }
                }
            }
        }

        
    }
}
