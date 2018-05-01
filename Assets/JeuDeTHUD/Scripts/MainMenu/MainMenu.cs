using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private DebugLogComponent debugLogComponent;
    public RectTransform currentPlayerUIPanel;

    private void Start()
    {
        debugLogComponent = GetComponent<DebugLogComponent>();
        AssociateCurrentPlayerToUI();
    }

    public void QuitGame()
    {
        Application.Quit();
        debugLogComponent.DebugMessage("Quit", true);
    }

    public void AssociateCurrentPlayerToUI()
    {
        Player currentPlayer = GameInformation.GetCurrentPlayer();
        if(currentPlayer != null)
        {
            currentPlayerUIPanel.GetComponent<AvatarItem>().avatarNameText.text = currentPlayer.Name;
            currentPlayerUIPanel.GetComponent<AvatarItem>().avatarImage.sprite = PPSerialization.Base64ToSprite(currentPlayer.Base64Image);
            currentPlayerUIPanel.gameObject.SetActive(true);
        }
        else
        {
            currentPlayerUIPanel.gameObject.SetActive(false);
        }
    }
}
