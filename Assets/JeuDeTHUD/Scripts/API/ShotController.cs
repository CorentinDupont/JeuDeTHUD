using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System;
using System.IO;
using UnityEngine.Networking;
//using System.Threading.Tasks;
using System.Text;

using JeuDeThud.Battle;
using JeuDeThud.Util;

namespace JeuDeThud.API
{
    [RequireComponent(typeof(DebugLogComponent))]
    public class ShotController : MonoBehaviour
    {

        private DebugLogComponent DebugLog { get { return GetComponent<DebugLogComponent>(); } }
        private const string API_SHOTS_URL = "http://localhost:3000/shots/";


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
            using (UnityWebRequest req = UnityWebRequest.Get(String.Format(API_SHOTS_URL + id_game + "/" + id_shot)))
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

            form.AddField(FIELD2, shotInfo.pawn);
            DebugLog.DebugMessage(FIELD2 + " : " + shotInfo.pawn, true);
            form.AddField(FIELD3, shotInfo.slot_1);
            DebugLog.DebugMessage(FIELD3 + " : " + shotInfo.slot_1, true);
            form.AddField(FIELD4, shotInfo.slot_2);
            DebugLog.DebugMessage(FIELD4 + " : " + shotInfo.slot_2, true);
            for (int i = 0; i < shotInfo.shot_eat.Count; i++)
            {
                form.AddField(FIELD1 + "[" + i + "]", shotInfo.shot_eat[i]);
                DebugLog.DebugMessage(FIELD1 + "[" + i + "] : " + shotInfo.shot_eat[i], true);
            }
            form.AddField(FIELD5, shotInfo.id_game);
            DebugLog.DebugMessage(FIELD5 + " : " + shotInfo.id_game, true);
            form.AddField(FIELD6, shotInfo.id_shot);
            DebugLog.DebugMessage(FIELD6 + " : " + shotInfo.id_shot, true);
            form.AddField(FIELD7, shotInfo.surrender.ToString());
            DebugLog.DebugMessage(FIELD7 + " : " + shotInfo.surrender.ToString(), true);

            using (UnityWebRequest req = UnityWebRequest.Post(String.Format(API_SHOTS_URL), form))
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
                    string shotJson = System.Text.Encoding.Default.GetString(result);
                    DebugLog.DebugMessage(shotJson, true);
                    ShotInfo shot = JsonUtility.FromJson<ShotInfo>(shotJson);

                    //Call success function
                    onSuccess(shot);
                }
            }
        }



    /*
        private async Task<ShotInfo> SendNewShotTask(ShotInfo shot)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(API_SHOTS_URL));

            postData += FIELD2 + "=" + shot.pawn + "&";
            postData += FIELD3 + "=" + shot.slot_1 + "&";
            postData += FIELD4 + "=" + shot.slot_2 + "&";
            postData += FIELD5 + "=" + shot.id_game + "&";
            postData += FIELD6 + "=" + shot.id_shot + "&";
            postData += FIELD7 + "=" + shot.surrender;
            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

            string jsonResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
            ShotInfo info = JsonUtility.FromJson<ShotInfo>(jsonResponse);
            return info;
        }*/

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
            //StartCoroutine(SendNewShot(shot, ShareCreatedNewShot));
        
        }
        /*
        public async void LaunchTaskPostNewShot(ShotInfo shot)
        {
            ShotInfo shotInfo = await SendNewShotTask(shot);
            ShareCreatedNewShot(shotInfo);
        }

        //public method to post new shot
        public void LaunchPostNewShot(ShotInfo shot)
        {
            //StartCoroutine(SendNewShot(shot, ShareCreatedNewShot));

        }

        /*public async void LaunchTaskPostNewShot(ShotInfo shot)
        {
            ShotInfo shotInfo = await SendNewShotTask(shot);
            ShareCreatedNewShot(shotInfo);
        }*/

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
            if (GetComponent<OnlineBattleManager>())
            {
                GetComponent<OnlineBattleManager>().CheckIfCurrentPlayerShotIsCorrectlySended(shot);
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
}
    
