using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdsWatchedData 
{
    public int chestCount;
    public int goldCount;
    public int gemCount;

    public AdsWatchedData(int chest, int gold, int gem)
    {
        chestCount = chest;
        goldCount = gold;
        gemCount = gem;
    }
}
