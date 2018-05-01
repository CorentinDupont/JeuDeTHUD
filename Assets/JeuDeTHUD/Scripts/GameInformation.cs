using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour {

    

	public static Player GetCurrentPlayer()
    {
        //Get the current player in the PlayerPrefs
        return (Player) PPSerialization.Load(Constants.currentPlayerKey);
    }

    public static Player GetPlayer2()
    {
        return (Player)PPSerialization.Load(Constants.player2Key);
    }

    public static List<Player> GetAllPlayers() {
        int playerIndex = 0;
        List<Player> playerList = new List<Player>();
        //Get all of the object with a key formatted like "commonPlayer'X'"
        while(PPSerialization.Load(Constants.commonPlayerKey + playerIndex) != null)
        {
            playerList.Add((Player)PPSerialization.Load(Constants.commonPlayerKey + playerIndex));
            playerIndex++;
        }

        return playerList;
    }

    

}
