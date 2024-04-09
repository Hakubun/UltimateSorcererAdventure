using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveDateTime 
{
    // Start is called before the first frame update
    public static void SaveDate(DateTime moment)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/rewardResetTime.date";
        FileStream stream = new FileStream(path, FileMode.Create);

        RewardTimeData data = new RewardTimeData(moment);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static RewardTimeData LoadRewardTime()
    {
        string path = Application.persistentDataPath + "/rewardResetTime.date";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            RewardTimeData data = formatter.Deserialize(stream) as RewardTimeData;

            stream.Close();

            return data;
        } else {
            //Debug.Log("no file, pass yesterday");
            RewardTimeData data = new RewardTimeData(DateTime.Now.AddDays(-1));
            return data;
        }
    }
}
