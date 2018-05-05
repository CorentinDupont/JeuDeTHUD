using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInformation : MonoBehaviour {

    public static int Turn { get; set; }
    public static bool IsDwarfTurn { get; set; }

    public static Player DwarfPlayer { get; set; }
    public static Player TrollPlayer { get; set; }

    public static int RoundNum { get; set; }
    
    public static int TrollPlayerPoint { get; set; }
    public static int DwarfPlayerPoint { get; set; }

    public static int TakenDwarfCount { get; set; }
    public static int TakenTrollCount { get; set; }
}
