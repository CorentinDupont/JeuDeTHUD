using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAvatarName : MonoBehaviour {

    public Image newAvatarImage;
    public InputField newAvatarNameInputField;

    public void SetNewAvatarImage()
    {
        newAvatarImage.sprite = PPSerialization.Base64ToSprite(SelectAvatarMenu.tempNewPlayer.Base64Image);
    }

    public void SetNewAvatarName()
    {
        SelectAvatarMenu.tempNewPlayer.Name = newAvatarNameInputField.text;
    }
}
