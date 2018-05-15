using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

using JeuDeThud.Save;

namespace JeuDeThud.MainMenu.Avatar
{
    public class NewAvatarImage : MonoBehaviour
    {

        public void RegisterSelectedImage()
        {
            SelectAvatarMenu.tempNewPlayer.Base64Image = PPSerialization.ImageToBase64(GetComponent<AvatarItem>().avatarImage.sprite.texture);
        }
    }
}

