﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using JeuDeThud.Util;
using JeuDeThud.Save;
using JeuDeThud.MainMenu.Avatar;

namespace JeuDeThud.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        private DebugLogComponent debugLogComponent;
        public RectTransform currentPlayerUIPanel;
        public SceneLoader sceneLoader;

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
            if (currentPlayer != null)
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

        public static void LaunchGame(bool isOnline, bool isVsIA)
        {
            if (isOnline)
            {
                PlayerPrefs.SetInt(Constants.gameIsOnlineKey, 1);
                PlayerPrefs.SetInt(Constants.gameIsVsIAKey, 0);
            }
            else if (isVsIA)
            {
                PlayerPrefs.SetInt(Constants.gameIsOnlineKey, 0);
                PlayerPrefs.SetInt(Constants.gameIsVsIAKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(Constants.gameIsOnlineKey, 0);
                PlayerPrefs.SetInt(Constants.gameIsVsIAKey, 0);
            }

            GameObject.FindObjectOfType<SceneLoader>().LoadScene();

        }

        public void LaunchIAGame()
        {
            PlayerPrefs.SetInt(Constants.gameIsOnlineKey, 0);
            PlayerPrefs.SetInt(Constants.gameIsVsIAKey, 1);
            GameObject.FindObjectOfType<SceneLoader>().LoadScene();
        }
    }
}
