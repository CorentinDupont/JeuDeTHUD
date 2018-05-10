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


    // Use this for initialization
    void Start () {
        LaunchGetAllOnlineGames();

    }
	
	// Update is called once per frame
	void Update () {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            apiCheckCountdown = API_CHECK_MAXTIME;
            LaunchGetAllOnlineGames();
        }
        else
        {
            DebugLog.DebugMessage("Le serveur n'as pas répondu dans les temps.", true);
        }
    }


    IEnumerator GetAllOnlineGames(Action<List<OnlineGame>> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_GAMES_URL)))
        {
            yield return req.SendWebRequest();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string onlineGamesJson = System.Text.Encoding.Default.GetString(result);
            List<OnlineGame> listOnlineGames = JsonUtility.FromJson<List<OnlineGame>>(onlineGamesJson);
            onSuccess(listOnlineGames);
        }
    }

    public void LaunchGetAllOnlineGames()
    {
        StartCoroutine(GetAllOnlineGames(PrintAllOnlineGames));
    }

    public void PrintAllOnlineGames(List<OnlineGame> onlineGames)
    {
        foreach (OnlineGame onlineGame in onlineGames)
        {
            DebugLog.DebugMessage("id_game : " + onlineGame.id_game, true);
        }
    }
}

[Serializable]
public class OnlineGame
{
    public string starter;
    public string listener;
    public int id_game;
}
