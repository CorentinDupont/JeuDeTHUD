using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System;
using System.IO;
using UnityEngine.Networking;



[RequireComponent(typeof(DebugLogComponent))]
public class OnlineGameController : MonoBehaviour {

    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }
    private const string API_GAMES_URL = "http://localhost:3000/games/";
    private const float API_CHECK_MAXTIME = 10;
    private float apiCheckCountdown = API_CHECK_MAXTIME;

    private const string FIELD1 = "starter";
    private const string FIELD2 = "listener";
    private const string FIELD3 = "id_game";

    public OnlineGameInfo[] waitingOnlineGameInfos;
    public string waitingOnlineGameErrorMessage = "";

    //Get All Waiting Online Games Requesst
    private IEnumerator GetAllOnlineGames(Action<OnlineGameInfo[]> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_GAMES_URL+"waitinggames/")))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
                //Debug and save error message
                DebugLog.DebugMessage(req.error, true);
                waitingOnlineGameErrorMessage = req.error;
                //set error text in UI
                if (GetComponent<OnlineGameList>())
                {
                    GetComponent<OnlineGameList>().errorText.text = req.error;
                }
            }
            else
            {
                waitingOnlineGameErrorMessage = "";
                while (!req.isDone)
                {
                    yield return null;
                }

                byte[] result = req.downloadHandler.data;
                string onlineGamesJson = "{\"Items\":" + System.Text.Encoding.Default.GetString(result) + "}";
                DebugLog.DebugMessage(onlineGamesJson, true);
                OnlineGameInfo[] onlineGameInfos = JsonHelper.FromJson<OnlineGameInfo>(onlineGamesJson);
                onSuccess(onlineGameInfos);
            }
            
        }
    }

    //Get one onlineGame
    private IEnumerator GetOnlineGameById(string id_game, Action<OnlineGameInfo> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_GAMES_URL + id_game)))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {

            }
            else
            {
                while (!req.isDone)
                {
                    yield return null;
                }
                byte[] result = req.downloadHandler.data;
                string onlineGameJson = System.Text.Encoding.Default.GetString(result);
                DebugLog.DebugMessage(onlineGameJson, true);
                OnlineGameInfo onlineGameInfo = JsonUtility.FromJson<OnlineGameInfo>(onlineGameJson);
                onSuccess(onlineGameInfo);
            }
        }
    }

    //Upload new waiting game request
    private IEnumerator UploadOneOnlineGame(Action<OnlineGameInfo> onSuccess)
    {
        WWWForm form = new WWWForm();
        form.AddField(FIELD1, Network.player.ipAddress);
        form.AddField(FIELD2, "");
        form.AddField(FIELD3, "");

        using (UnityWebRequest req = UnityWebRequest.Post(API_GAMES_URL, form))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log(req.error);
            }
            else
            {
                while (!req.isDone)
                {
                    yield return null;
                }
                byte[] result = req.downloadHandler.data;
                string onlineGameJson = System.Text.Encoding.Default.GetString(result);
                DebugLog.DebugMessage(onlineGameJson, true);
                OnlineGameInfo onlineGameInfo = JsonUtility.FromJson<OnlineGameInfo>(onlineGameJson);
                onSuccess(onlineGameInfo);
            }
        }
    }

    //Update new waiting game Request
    private IEnumerator UpdateOneOnlineGame(OnlineGameInfo onlineGame)
    {
        WWWForm form = new WWWForm();
        form.AddField(FIELD1, onlineGame.starter);
        form.AddField(FIELD2, onlineGame.listener);
        form.AddField(FIELD3, onlineGame.id_game);

        using (UnityWebRequest www = UnityWebRequest.Post(API_GAMES_URL + onlineGame.id_game, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form update complete!");
            }
        }
    }
    //public method to get all waiting games online
    public void LaunchGetAllOnlineGames()
    {
        StartCoroutine(GetAllOnlineGames(SetWaitingOnlineGameInfoArray));
    }

    //public method to post a new waiting game
    public void LaunchUploadNewOnlineGame()
    {
        StartCoroutine(UploadOneOnlineGame(ShareCreatedGame));
    }

    //public method to post a new waiting game
    public void LaunchUpdateOnlineGame(OnlineGameInfo onlineGame)
    {
        StartCoroutine(UpdateOneOnlineGame(onlineGame));
    }

    //public method to get one online game
    public void LaunchGetOnlineGameById(string id_game)
    {
        StartCoroutine(GetOnlineGameById(id_game, null));
    }

    //public method to check if a player join the partie
    public void LaunchCheckIfPlayerJoin(string id_game)
    {
        StartCoroutine(GetOnlineGameById(id_game, SetAPlayerJoinTheGame));
    }

    //print all games in console
    public void PrintAllOnlineGames(OnlineGameInfo[] onlineGames)
    {
        DebugLog.DebugMessage("call print : ", true);
        foreach (OnlineGameInfo onlineGame in onlineGames)
        {
            DebugLog.DebugMessage("one game : " + onlineGame.id_game, true);
        }
    }
    //set onlineGameInfo
    private void SetWaitingOnlineGameInfoArray(OnlineGameInfo[] onlineGames)
    {
        this.waitingOnlineGameInfos = onlineGames;
        //Populate list in UI
        if (GetComponent<OnlineGameList>()) {
            GetComponent<OnlineGameList>().PopulateList(onlineGames);
        }
    }

    //Inform Player if a player join his created game
    private void SetAPlayerJoinTheGame(OnlineGameInfo onlineGame)
    {
        bool aPlayerJoin = false;
        if(onlineGame.listener != "")
        {
            aPlayerJoin = true;
        }
        else
        {
            aPlayerJoin = false;
        }

        if (GetComponent<WaitingOtherPlayer>())
        {
            GetComponent<WaitingOtherPlayer>().LaunchGameIf(aPlayerJoin);
        }
    }

    //Set online game to waiting other player popup
    private void ShareCreatedGame(OnlineGameInfo onlineGame)
    {
        if (GetComponent<WaitingOtherPlayer>())
        {
            GetComponent<WaitingOtherPlayer>().SetCreatedOnlineGame(onlineGame);
        }
    }
}

[Serializable]
public class OnlineGameInfo
{
    public string starter;
    public string listener;
    public string id_game;
}

