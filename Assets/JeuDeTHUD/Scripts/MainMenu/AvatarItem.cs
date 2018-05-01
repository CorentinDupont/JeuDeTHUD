using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour {

    public Image avatarImage;
    public Text avatarNameText;
    public MainMenu mainMenu;
    public SelectAvatarMenu selectAvatarMenu;

    [HideInInspector]
    public Player associatedPlayer;

    public void SelectPlayer()
    {
        PPSerialization.SaveCurrentPlayer(associatedPlayer);
        mainMenu.AssociateCurrentPlayerToUI();
        selectAvatarMenu.gameObject.SetActive(false);
        mainMenu.gameObject.transform.parent.gameObject.SetActive(true);

    }
}
