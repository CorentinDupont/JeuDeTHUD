﻿using System.Collections;
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
            DebugLog.DebugMessage("Count Down écoutlé", true);
        }
    }


    IEnumerator GetAllOnlineGames(Action<OnlineGameInfo[]> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_GAMES_URL)))
        {
            yield return req.SendWebRequest();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string onlineGamesJson = "{\"Items\":" + System.Text.Encoding.Default.GetString(result) +"}";
            DebugLog.DebugMessage(onlineGamesJson, true);
            OnlineGameInfo[] onlineGameInfos = JsonHelper.FromJson<OnlineGameInfo>(onlineGamesJson);
            DebugLog.DebugMessage("onlineGameInfo 0  = " + onlineGameInfos[0], true);
            onSuccess(onlineGameInfos);
        }
    }

    public void LaunchGetAllOnlineGames()
    {
        StartCoroutine(GetAllOnlineGames(PrintAllOnlineGames));
    }

    public void PrintAllOnlineGames(OnlineGameInfo[] onlineGames)
    {
        DebugLog.DebugMessage("call print : ", true);
        foreach (OnlineGameInfo onlineGame in onlineGames)
        {
            DebugLog.DebugMessage("one game : " + onlineGame.id_game, true);
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

[Serializable]
public class OnlineGameList
{
    public List<OnlineGameInfo> values;
}