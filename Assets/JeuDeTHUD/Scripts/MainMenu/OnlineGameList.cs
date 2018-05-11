using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OnlineGameController))]
[RequireComponent(typeof(DebugLogComponent))]

public class OnlineGameList : MonoBehaviour {

    public GameObject onlineGameItemPrefab;
    public AudioSource buttonAudioSource;
    public Text errorText;
    public RectTransform onlineGameUnavailablePanel;
    
    [HideInInspector]
    public OnlineGameController OnlineGameController { get { return GetComponent<OnlineGameController>(); } }

    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

    private OnlineGameInfo[] currentOnlineGameInfos;

    //A l'apparition du menu
    private void OnEnable()
    {
        //charge les parties en ligne et remplie la liste, toutes les 0.1s.
        //InvokeRepeating("LoadOnlineGames", 0.0f, 0.1f);
        
        LoadOnlineGames();
    }

    //remplie la liste
    public void PopulateList(OnlineGameInfo[] onlineGames) {
        DebugLog.DebugMessage("Populate UI with Online Games Items ...", true);

        errorText.text = "";

        //Delete all games from UI
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }

        //Add all game in UI
        foreach (OnlineGameInfo onlineGame in onlineGames)
        {
            //Instantiate an item in the list, and set parent
            GameObject onlineGameItem = Instantiate(onlineGameItemPrefab);
            onlineGameItem.transform.SetParent(this.transform, false);

            //Set text of item
            onlineGameItem.GetComponentInChildren<Text>().text = "Partie n°" + onlineGame.id_game;

            //Set onlineGame of item
            onlineGameItem.GetComponent<OnlineGameItem>().onlineGameInfo = onlineGame;

            //Set audio source for clic sound
            onlineGameItem.GetComponent<ClicSound>().audioSource = buttonAudioSource;
        }
        currentOnlineGameInfos = onlineGames;
    }

    public void LoadOnlineGames()
    {
        DebugLog.DebugMessage("Load Available Online Games ...", true);
        OnlineGameController.LaunchGetAllOnlineGames();        
    }

    public void TryToJoinOnlineGame(OnlineGameInfo onlineGame)
    {
        DebugLog.DebugMessage("Try to join online game ...", true);
        OnlineGameController.LaunchCheckGameIsAvailable(onlineGame.id_game);
    }
    public void JoinOnlineGame(OnlineGameInfo onlineGame)
    {
        DebugLog.DebugMessage("Join Online Game ! ", true);
        SaveOnlineGameInfo(onlineGame);
        SetOnlineGameInfoListener(onlineGame);

        MainMenu.LaunchGame(true, false);
    }

    //Save in player prefs the online game info to pass it to the board scene
    private void SaveOnlineGameInfo(OnlineGameInfo onlineGame)
    {
        DebugLog.DebugMessage("Save Online Game Info ... ", true);
        PPSerialization.Save(Constants.onlineGameInfoKey, onlineGame);
    }

    //Call update online game in API, to make i disappear from the menu.
    private void SetOnlineGameInfoListener(OnlineGameInfo onlineGame)
    {
        DebugLog.DebugMessage("Mark current player as listener of Online Game ...", true);
        onlineGame.listener = Network.player.ipAddress;
        OnlineGameController.LaunchUpdateOnlineGame(onlineGame);
    }


    //method called at the end of request of OnlineGameController
    public void JoinGameIfIsAvailable(OnlineGameInfo onlineGame, bool isAvailable)
    {
        if (isAvailable)
        {
            JoinOnlineGame(onlineGame);
        }
        else
        {
            DebugLog.DebugMessage("Show Unavailable Game Modal", true);
            //show unavailable onlineGame modal
            onlineGameUnavailablePanel.gameObject.SetActive(true);

            //Refresh Online Games
            LoadOnlineGames();
        }
    }
}
