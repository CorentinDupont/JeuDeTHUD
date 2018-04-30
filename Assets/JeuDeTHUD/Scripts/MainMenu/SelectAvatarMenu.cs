using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAvatarMenu : MonoBehaviour {

    public GameObject avatarPrefab;
    public GameObject avatarUIList;

    [HideInInspector]
    public static Player tempNewPlayer;

	void Start () {
        PopulateAvatarUIList();

    }
	
	void Update () {
		
	}

    //Destroy all childs of avatar UI List
    private void ResetAvatarUIList()
    {
        foreach(GameObject avatarUIItem in avatarUIList.transform)
        {
            Destroy(avatarUIItem);
        }
    }

    //Populate avatar UI List with players present in PlayerPrefs
    private void PopulateAvatarUIList()
    {
        //Get all players
        List<Player> playerList = GameInformation.GetAllPlayers();

        //Add player as child
        foreach (Player player in playerList){
            Instantiate(avatarPrefab);
            avatarPrefab.transform.SetParent(avatarUIList.transform, false);
            avatarPrefab.GetComponent<AvatarItem>().avatarNameText.text = player.Name;
            avatarPrefab.GetComponent<AvatarItem>().avatarImage.sprite = Resources.Load<Sprite>(player.ImagePath);
        }
    }

    private void AddNewPlayer()
    {

    }
}
