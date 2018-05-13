using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OnlineGameController))]
[RequireComponent(typeof(DebugLogComponent))]
public class WaitingOtherPlayer : MonoBehaviour {

    OnlineGameController OnlineGameController { get { return GetComponent<OnlineGameController>(); } }
    DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

    [HideInInspector]
    public OnlineGameInfo onlineGame;

    private void OnEnable()
    {
        //Create a game
        OnlineGameController.LaunchUploadNewOnlineGame();
    }

    private void SearchAnotherPlayer()
    {
        DebugLog.DebugMessage("Searching player....", true);
        //Recherche si un joueur rejoins la partie
        OnlineGameController.LaunchCheckIfPlayerJoin(onlineGame.id_game);
    }

    //method called after creating new game from OnlineGameController
    public void SetCreatedOnlineGame(OnlineGameInfo onlineGame)
    {
        this.onlineGame = onlineGame;
        DebugLog.DebugMessage("Game created !!", true);
        SearchAnotherPlayer();
    }

    //method called after receveing answer is player join in OnlineGameController
    public void LaunchGameIf(bool aPlayerJoin) {
        if (aPlayerJoin)
        {
            DebugLog.DebugMessage("A player join the game !!!!", true);
            PPSerialization.Save(Constants.onlineGameInfoKey, onlineGame);
            MainMenu.LaunchGame(true, false);
        }
        else
        {
            SearchAnotherPlayer();
        }
    }

    //method called after receveing answer from delete request launch in OnlineGameController
    public void CloseModal() {
        gameObject.SetActive(false);
    }

   public void CancelGameCreation()
    {
        OnlineGameController.LaunchDeleteOnlineGameById(onlineGame.id_game);
    }
}
