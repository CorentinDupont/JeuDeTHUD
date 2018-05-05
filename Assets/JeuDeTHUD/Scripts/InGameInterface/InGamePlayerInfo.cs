using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerInfo : MonoBehaviour {

    public bool isPlayer1;

	// Use this for initialization
	void Start () {
        if (isPlayer1)
        {
            //Change image and text of player 1 info panel
            transform.GetComponentInChildren<AvatarItem>().avatarImage.sprite = PPSerialization.Base64ToSprite(GameInformation.GetCurrentPlayer().Base64Image);
            transform.GetComponentInChildren<AvatarItem>().avatarNameText.text = GameInformation.GetCurrentPlayer().Name;
        }
        else
        {
            if (PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
            {
                //Change image and text of IA info panel
                transform.GetComponentInChildren<AvatarItem>().avatarImage.sprite = null;
                transform.GetComponentInChildren<AvatarItem>().avatarNameText.text = "IA";
            }
            else if(PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
            {
                //Change image and text of IA info panel
                transform.GetComponentInChildren<AvatarItem>().avatarImage.sprite = null;
                transform.GetComponentInChildren<AvatarItem>().avatarNameText.text = "Online Player";
            }
            else //Is versus local player
            {
                //Change image and text of player 2 info panel
                transform.GetComponentInChildren<AvatarItem>().avatarImage.sprite = PPSerialization.Base64ToSprite(GameInformation.GetPlayer2().Base64Image);
                transform.GetComponentInChildren<AvatarItem>().avatarNameText.text = GameInformation.GetPlayer2().Name;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
