using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAdsCounter 
{
    public static void UpdateAdsCounter(int chest, int gold, int gem)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/adswatch.count";
        FileStream stream = new FileStream(path, FileMode.Create);

        AdsWatchedData data = new AdsWatchedData(chest, gold, gem);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static AdsWatchedData LoadAdsCounter()
    {
        string path = Application.persistentDataPath + "/adswatch.count";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AdsWatchedData data = formatter.Deserialize(stream) as AdsWatchedData;

            stream.Close();

            return data;
        } else {
            Debug.Log("no file, pass yesterday");
            AdsWatchedData data = new AdsWatchedData(5,5,5);
            return data;
        }
    }
}
