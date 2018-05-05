using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnText : MonoBehaviour {

    public void UpdateText()
    {
        GetComponent<Text>().text = "Tour " + BattleInformation.Turn;
    }
}
