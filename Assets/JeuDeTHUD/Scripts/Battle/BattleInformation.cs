using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInformation : MonoBehaviour {

    public static int Turn { get; set; }
    public static bool IsDwarfTurn { get; set; }

    public static Player DwarfPlayer { get; set; }
    public static Player TrollPlayer { get; set; }

    public static int RoundNum { get; set; }

    public static int Player1Point { get; set; }
    public static int Player2Point { get; set; }

    public static int TakenDwarfCount { get; set; }
    public static int TakenTrollCount { get; set; }

    public static bool PlayerHasMadeAnActionInHisTurn { get; set; }

    public static OnlineGameInfo OnlineGameInfo{ get { return (OnlineGameInfo)PPSerialization.Load(Constants.onlineGameInfoKey); } }


}
