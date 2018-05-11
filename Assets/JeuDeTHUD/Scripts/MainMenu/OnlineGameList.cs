using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OnlineGameController))]
public class OnlineGameList : MonoBehaviour {

    public GameObject onlineGameItemPrefab;
    public AudioSource buttonAudioSource;
    public Text errorText;
    
    private OnlineGameController OnlineGameController { get { return GetComponent<OnlineGameController>(); } }
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
        

        //Delete all games from UI
        foreach (Transform childTransform in transform) {
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
        OnlineGameController.LaunchGetAllOnlineGames();
        //if any error occured
        if (OnlineGameController.waitingOnlineGameErrorMessage.Equals(""))
        {
            errorText.text = "";
            PopulateList(OnlineGameController.waitingOnlineGameInfos);
        }
        else
        {
            errorText.text = OnlineGameController.waitingOnlineGameErrorMessage;
        }
        
    }
}
