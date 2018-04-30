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
}
