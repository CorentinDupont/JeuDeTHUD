using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTable : MonoBehaviour {

    public Image player1Image;
    public Image player2Image;

    public Text player1Name;
    public Text player2Name;

    public Text player1Score;
    public Text player2Score;

    public Image player1CardBackground;
    public Image player2CardBackground;

    public Color winnerColor;

    private void OnEnable()
    {
        player1Image.sprite = PPSerialization.Base64ToSprite(GameInformation.GetCurrentPlayer().Base64Image);
        player1Name.text = GameInformation.GetCurrentPlayer().Name;
        if(PlayerPrefs.GetInt(Constants.gameIsVsIAKey) == 1)
        {
            player2Image.sprite = null;
            player2Name.text = "IA";
        }
        else if(PlayerPrefs.GetInt(Constants.gameIsOnlineKey) == 1)
        {
            player2Image.sprite = null;
            player2Name.text = "Online Player";
        }
        else
        {
            player2Image.sprite = PPSerialization.Base64ToSprite(GameInformation.GetPlayer2().Base64Image);
            player2Name.text = GameInformation.GetPlayer2().Name;
        }

        player1Score.text = BattleInformation.Player1Point.ToString();
        player2Score.text = BattleInformation.Player2Point.ToString();

        if(BattleInformation.Player1Point > BattleInformation.Player2Point)
        {
            player1CardBackground.color = winnerColor;
        }
        else if(BattleInformation.Player1Point < BattleInformation.Player2Point)
        {
            player2CardBackground.color = winnerColor;
        }
        else if(BattleInformation.Player1Point == BattleInformation.Player2Point)
        {
            player1CardBackground.color = winnerColor;
            player2CardBackground.color = winnerColor;
        }




    }

    
}
