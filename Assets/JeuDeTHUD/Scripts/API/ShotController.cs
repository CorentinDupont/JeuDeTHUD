using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System;
using System.IO;
using UnityEngine.Networking;

[RequireComponent(typeof(DebugLogComponent))]
public class ShotController : MonoBehaviour {

    private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }
    private const string API_GAMES_URL = "http://localhost:3000/shots/";

    private const string FIELD1 = "shot_eat";
    private const string FIELD2 = "pawn";
    private const string FIELD3 = "slot_1";
    private const string FIELD4 = "slot_2";
    private const string FIELD5 = "id_game";
    private const string FIELD6 = "id_shot";
    private const string FIELD7 = "surrender";

    /*************************************************************************/
    /*************************** ALL REQUESTS METHOD *************************/
    /*************************************************************************/
    //Need to be launch with Coroutine. See public method below

    //Get one shot by id and game id
    private IEnumerator GetShotByIdAndGameId(string id_game, int id_shot, Action<ShotInfo> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_GAMES_URL + id_game +"/"+id_shot)))
        {
            yield return req.SendWebRequest();

            if (req.isNetworkError || req.isHttpError)
            {
                DebugLog.DebugMessage(req.error, true);
                onSuccess(null);
            }
            else
            {
                while (!req.isDone)
                {
                    yield return null;
                }
                byte[] result = req.downloadHandler.data;
                string onlineGameJson = System.Text.Encoding.Default.GetString(result);
                DebugLog.DebugMessage(onlineGameJson, true);
                ShotInfo shot = JsonUtility.FromJson<ShotInfo>(onlineGameJson);
                onSuccess(shot);
            }
        }
    }

    //Send one shot
    private IEnumerator SendNewShot(ShotInfo shotInfo, Action<ShotInfo> onSuccess)
    {
        GetComponent<OnlineBattleManager>().PrintShotInfo(shotInfo);
        //Construct body of request with WWWForm
        WWWForm form = new WWWForm();
        for(int i = 0; i<shotInfo.shot_eat.Count; i++)
        {
            form.AddField(FIELD1+"["+i+"]", shotInfo.shot_eat[i]);
        }
        form.AddField(FIELD2, shotInfo.pawn);
        form.AddField(FIELD3, shotInfo.slot_1);
        form.AddField(FIELD3, shotInfo.slot_2);
        form.AddField(FIELD3, shotInfo.id_game);
        form.AddField(FIELD3, shotInfo.id_shot);
        form.AddField(FIELD3, shotInfo.surrender.ToString());

        using (UnityWebRequest req = UnityWebRequest.Post(String.Format(API_GAMES_URL), form))
        {
            //Send request
            yield return req.SendWebRequest();
            //if an error occured
            if (req.isNetworkError || req.isHttpError)
            {
                DebugLog.DebugMessage(req.error, true);
                onSuccess(null);
            }
            else
            {
                //wait until the request is done
                while (!req.isDone)
                {
                    yield return null;
                }
                //get byte array result
                byte[] result = req.downloadHandler.data;

                //Get JSON result, and Serialize it
                string onlineGameJson = System.Text.Encoding.Default.GetString(result);
                DebugLog.DebugMessage(onlineGameJson, true);
                ShotInfo shot = JsonUtility.FromJson<ShotInfo>(onlineGameJson);

                //Call success function
                onSuccess(shot);
            }
        }
    }

    /*************************************************************************/
    /****************** ALL PUBLIC METHOD TO LAUNCH REQUEST ******************/
    /*************************************************************************/

    //public method to get shot by id and id game
    public void LaunchGetOneShot(string id_game, int id_shot)
    {
        StartCoroutine(GetShotByIdAndGameId(id_game, id_shot, ShareShotFoundByIdAndIdGame));
    }

    //public method to post new shot
    public void LaunchPostNewShot(ShotInfo shot)
    {
        StartCoroutine(SendNewShot(shot, ShareCreatedNewShot));
    }

    /****************************************************************************************************/
    /****************** METHOD WICH CALL OTHER COMPONENT TO RETURN DATA FROM COROUTINE ******************/
    /****************************************************************************************************/

    //private method to return shot by id and id game
    private void ShareShotFoundByIdAndIdGame(ShotInfo shot)
    {
        if (GetComponent<OnlineBattleManager>())
        {
            GetComponent<OnlineBattleManager>().ReproduceOnlinePlayerShot(shot);
        }
    }

    //private method to return created shot
    private void ShareCreatedNewShot(ShotInfo shot)
    {
        if (GetComponent<BattleManager>())
        {

        }
    }


}

public class ShotInfo
{
    public List<string> shot_eat = new List<string>();
    public string pawn;
    public string slot_1;
    public string slot_2;
    public string id_game;
    public int id_shot;
    public bool surrender;
}
