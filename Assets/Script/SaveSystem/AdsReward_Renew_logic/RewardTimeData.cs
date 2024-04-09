using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RewardTimeData
{
    // Start is called before the first frame updat
    public string DateString;

    public RewardTimeData(DateTime moment)
    {
        DateString = moment.ToString();
    }
}
