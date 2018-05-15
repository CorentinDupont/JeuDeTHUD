using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using JeuDeThud.Save;

namespace JeuDeThud.MainMenu.Avatar
{
    public class NewAvatarName : MonoBehaviour
    {

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
}

