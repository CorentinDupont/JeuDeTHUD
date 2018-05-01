using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAvatarMenu : MonoBehaviour {

    public GameObject avatarPrefab;
    public GameObject avatarUIList;
    public MainMenu mainMenu;
    public Text titleText;
    public Button deleteButton;
    public bool isSelectingPlayer2;
    public bool isDeleting;

    //player which is created in the process
    [HideInInspector]
    public static Player tempNewPlayer = new Player();

    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }

	void Start () {

    }

    //Destroy all childs of avatar UI List
    private void ResetAvatarUIList()
    {
        foreach(Transform avatarUIItemTransform in avatarUIList.transform)
        {
            if(avatarUIItemTransform.gameObject.name != "CreateNewAvatarPanel")
            {
                Destroy(avatarUIItemTransform.gameObject);
            }
            
        }
    }

    //Populate avatar UI List with players present in PlayerPrefs
    private void PopulateAvatarUIList()
    {
        DebugLog.DebugMessage("isSelectingPlayer2 : " + isSelectingPlayer2, true);
        //Get all players
        List<Player> playerList = GameInformation.GetAllPlayers();

        //Add player as child
        foreach (Player player in playerList){
            
            //Hide player if isSelecting player2 and if this player is the current player
            if(isSelectingPlayer2 && player.ID == GameInformation.GetCurrentPlayer().ID)
            {
                //Do nothing..
            }
            else
            {
                //Add avatar item in the UI list
                GameObject avatarItem = Instantiate(avatarPrefab);
                avatarItem.transform.SetParent(avatarUIList.transform, false);
                //Set some paramter in the avatar item script
                avatarItem.GetComponent<AvatarItem>().avatarNameText.text = player.Name;
                avatarItem.GetComponent<AvatarItem>().avatarImage.sprite = PPSerialization.Base64ToSprite(player.Base64Image);
                avatarItem.GetComponent<AvatarItem>().associatedPlayer = player;
                avatarItem.GetComponent<AvatarItem>().mainMenu = mainMenu;
                avatarItem.GetComponent<AvatarItem>().selectAvatarMenu = this;
                if (isSelectingPlayer2)
                {
                    avatarItem.GetComponent<AvatarItem>().isSelectingPlayer2 = true;
                }
                if (isDeleting)
                {
                    avatarItem.GetComponent<AvatarItem>().isDeleting = true;
                }
            }
            
        }
        //Hide the + button is selecting player 2
        if (!isSelectingPlayer2)
        {
            avatarUIList.transform.Find("CreateNewAvatarPanel").SetAsLastSibling();
        }
        else
        {
            Destroy(avatarUIList.transform.Find("CreateNewAvatarPanel").gameObject);
        }
    }

    public void UpdateTitle()
    {
        if (isSelectingPlayer2)
        {
            titleText.text = "Choisissez l'avatar du deuxième joueur";
        }
        else
        {
            titleText.text = "Choisissez votre avatar";
        }
    }

    public void AddNewPlayer()
    {
        PPSerialization.SaveNewPlayer(tempNewPlayer);
    }

    public void UpdateAvatarUIList()
    {
        ResetAvatarUIList();
        PopulateAvatarUIList();
    }

    public void SetIsSelectingPlayer2(bool isSelectingPlayer2)
    {
        this.isSelectingPlayer2 = isSelectingPlayer2;
    }
        
        

    public void SwitchIsDeleting()
    {
        isDeleting = !isDeleting;
        if(isDeleting == true)
        {
            deleteButton.transform.GetComponentInChildren<Text>().text = "Annuler suppression";
            titleText.text = "Quel est l'avatar à supprimer ?";
        }
        else
        {
            deleteButton.transform.GetComponentInChildren<Text>().text = "Supprimer ...";
            titleText.text = "Choisissez votre avatar";
        }
        UpdateAvatarUIList();
    }

    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        isDeleting = false;
        deleteButton.transform.GetComponentInChildren<Text>().text = "Supprimer ...";

        if (isSelectingPlayer2)
        {
            deleteButton.gameObject.SetActive(false);
        }
        else
        {
            deleteButton.gameObject.SetActive(true);
        }

        UpdateAvatarUIList();
        UpdateTitle();
    }


}
