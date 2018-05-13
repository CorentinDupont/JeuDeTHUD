using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DebugLogComponent))]
[RequireComponent(typeof(BattleManager))]
[RequireComponent(typeof(ShotController))]
[RequireComponent(typeof(HUDLink))]
public class OnlineBattleManager : MonoBehaviour {

    //Required Component
    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }
    private BattleManager BattleManager { get { return GetComponent<BattleManager>(); } }
    private ShotController ShotController { get { return GetComponent<ShotController>(); } }
    private HUDLink HUDLink { get { return GetComponent<HUDLink>(); } }

    //Component of other object
    private Co_GameBoard GameBoard { get { return GameObject.FindObjectOfType<Co_GameBoard>(); } }

    //Method called by BattleManager, to launch research of a shot of the online player.
    public void WaitOtherPlayerShot()
    {
        ShotController.LaunchGetOneShot(BattleInformation.OnlineGameInfo.id_game, BattleInformation.ShotCount);
    }

    public void ReproduceOnlinePlayerShot(ShotInfo shot)
    {
        if(shot != null)
        {
            PrintShotInfo(shot);
            GameBoard.ReproduceConstructedShot(shot, false, false);

        }
        else
        {
            ShotController.LaunchGetOneShot(BattleInformation.OnlineGameInfo.id_game, BattleInformation.ShotCount);
        }
        
    }


    //Dev method
    public void PrintShotInfo(ShotInfo shotInfo)
    {
        DebugLog.DebugMessage(
            "pawn : "+          shotInfo.pawn 
            +" slot1 : "+       shotInfo.slot_1 
            +" slot2 "+         shotInfo.slot_2 
            +" eat : "+         shotInfo.shot_eat 
            +" id_game : "+     shotInfo.id_game 
            +" id_shot : "+     shotInfo.id_shot 
            +"surrender : "+    shotInfo.surrender
            , true);
    }
}
