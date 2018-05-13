﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameItem : MonoBehaviour {

    public OnlineGameInfo onlineGameInfo;
    
    private OnlineGameController OnlineGameController { get { return transform.GetComponentInParent<OnlineGameList>().OnlineGameController; } }

    //method called by button click
    public void TryToJoinOnlineGame()
    {
        transform.GetComponentInParent<OnlineGameList>().TryToJoinOnlineGame(onlineGameInfo);
    }

    
}