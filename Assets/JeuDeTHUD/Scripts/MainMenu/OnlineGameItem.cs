using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGameItem : MonoBehaviour {

    public OnlineGameInfo onlineGameInfo;



    public void JoinOnlineGame() {
        SaveOnlineGameInfo();
        SetOnlineGameInfoListener();
    }

    //Save in player prefs the online game info to pass it to the board scene
    private void SaveOnlineGameInfo()
    {
        PPSerialization.Save(Constants.onlineGameInfoKey, onlineGameInfo);
    }

    //Call update online game in API, to make i disappear from the menu.
    private void SetOnlineGameInfoListener() {
        onlineGameInfo.listener = Network.player.ipAddress;
        transform.GetComponentInParent<OnlineGameList>().OnlineGameController.LaunchUpdateOnlineGame(onlineGameInfo);
    }
}
