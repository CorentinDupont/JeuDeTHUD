﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using JeuDeThud;

namespace JeuDeThud.Save
{
    public class PPSerialization
    {

        public static BinaryFormatter binaryFormatter = new BinaryFormatter();

        //Put an object in the playerPrefs
        public static void Save(string saveTag, object obj)
        {
            //Convert obj in binary
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            string temp = System.Convert.ToBase64String(memoryStream.ToArray());
            //Add binary to PlayerPrefs
            PlayerPrefs.SetString(saveTag, temp);
        }

        //Get an object from the playerPrefs
        public static object Load(string saveTag)
        {
            string temp = PlayerPrefs.GetString(saveTag);
            //if there is nothing corresponding to the key, return null
            if (temp == string.Empty)
            {
                return null;
            }
            //else, transform string in object and return it
            MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(temp));
            return binaryFormatter.Deserialize(memoryStream);
        }

        public static string ImageToBase64(Texture2D tex)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            byte[] bytes2 = tex.EncodeToPNG();
            binaryFormatter.Serialize(ms, bytes2);
            bytes = ms.ToArray();


            string enc = Convert.ToBase64String(bytes);
            return enc;
        }

        public static Sprite Base64ToSprite(string base64Image)
        {


            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64Image));
            byte[] bytes = (byte[])binaryFormatter.Deserialize(memoryStream);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(bytes);
            Sprite spriteFromBase64 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            return spriteFromBase64;
        }

        public static void SaveNewPlayer(Player player)
        {
            int newPlayerIndex = GameInformation.GetAllPlayers().Count;
            player.ID = newPlayerIndex;
            Save(Constants.commonPlayerKey + newPlayerIndex, player);
        }

        public static void SaveCurrentPlayer(Player player)
        {
            Save(Constants.currentPlayerKey, player);
        }

        public static void SavePlayer2(Player player)
        {
            Save(Constants.player2Key, player);
        }

        public static void DeletePlayer(Player player)
        {
            //Store all players
            List<Player> playerList = GameInformation.GetAllPlayers();
            //Remove player to delete of the storage list
            foreach (Player tempPlayer in playerList.ToArray())
            {
                if (tempPlayer.ID == player.ID)
                {
                    playerList.Remove(tempPlayer);
                }
            }

            DeleteAllPlayers();

            //Put all player but the deleted one in the PlayerPrefs
            int playerIndex = 0;
            foreach (Player tempPlayer in playerList)
            {
                tempPlayer.ID = playerIndex;
                Save(Constants.commonPlayerKey + playerIndex, tempPlayer);
                playerIndex++;
            }
        }

        public static void DeleteAllPlayers()
        {
            int playerIndex = 0;

            //Delete all players from PlayerPrefs
            while (Load(Constants.commonPlayerKey + playerIndex) != null)
            {
                PlayerPrefs.DeleteKey(Constants.commonPlayerKey + playerIndex);
                playerIndex++;
            }
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}

