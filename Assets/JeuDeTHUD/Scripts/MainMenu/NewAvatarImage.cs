using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAvatarImage : MonoBehaviour {

	public void RegisterSelectedImage()
    {
        SelectAvatarMenu.tempNewPlayer.ImagePath = "JeuDeTHUD/"GetComponent<AvatarItem>().avatarImage.name;
    }
}
