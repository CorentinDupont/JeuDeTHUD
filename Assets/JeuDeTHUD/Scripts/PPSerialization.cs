using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PPSerialization {

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
        if(temp == string.Empty)
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
        byte[] bytes = (byte[]) binaryFormatter.Deserialize(memoryStream);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(bytes);
        Sprite spriteFromBase64 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        return spriteFromBase64;
    }

    public static void SaveNewPlayer(Player player)
    {
        int newPlayerIndex = GameInformation.GetAllPlayers().Count;
        Save(Constants.commonPlayerKey + newPlayerIndex, player);
    }
}
