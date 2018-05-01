using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAvatarMenu : MonoBehaviour {

    public GameObject avatarPrefab;
    public GameObject avatarUIList;
    public MainMenu mainMenu;

    [HideInInspector]
    public static Player tempNewPlayer = new Player();

	void Start () {
        PopulateAvatarUIList();

    }
	
	void Update () {
		
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
        //Get all players
        List<Player> playerList = GameInformation.GetAllPlayers();

        //Add player as child
        foreach (Player player in playerList){
            GameObject avatarItem = Instantiate(avatarPrefab);
            avatarItem.transform.SetParent(avatarUIList.transform, false);
            avatarItem.GetComponent<AvatarItem>().avatarNameText.text = player.Name;
            avatarItem.GetComponent<AvatarItem>().avatarImage.sprite = PPSerialization.Base64ToSprite(player.Base64Image);
            avatarItem.GetComponent<AvatarItem>().associatedPlayer = player;
            avatarItem.GetComponent<AvatarItem>().mainMenu = mainMenu;
            avatarItem.GetComponent<AvatarItem>().selectAvatarMenu = this;
        }

        avatarUIList.transform.Find("CreateNewAvatarPanel").SetAsLastSibling();
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

    
}
