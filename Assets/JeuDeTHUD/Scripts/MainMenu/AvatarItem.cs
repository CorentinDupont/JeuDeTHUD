using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour {

    public Image avatarImage;
    public Text avatarNameText;
    public MainMenu mainMenu;
    public SelectAvatarMenu selectAvatarMenu;
    public bool isSelectingPlayer2;
    public bool isDeleting;

    [HideInInspector]
    public Player associatedPlayer;

    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

    public void SelectPlayer()
    {
        if (!isSelectingPlayer2 && !isDeleting)
        {
            PPSerialization.SaveCurrentPlayer(associatedPlayer);
            mainMenu.AssociateCurrentPlayerToUI();
            selectAvatarMenu.gameObject.SetActive(false);
            mainMenu.gameObject.transform.parent.gameObject.SetActive(true);
        }
        else if(isSelectingPlayer2)
        {
            PPSerialization.SavePlayer2(associatedPlayer);
            mainMenu.LaunchGame(false, false);
        }
        else if (isDeleting)
        {
            //Delete player from PlayerPrefs
            PPSerialization.DeletePlayer(associatedPlayer);
            //Refresh the UI
            selectAvatarMenu.UpdateAvatarUIList();
        }
        
    }
}
