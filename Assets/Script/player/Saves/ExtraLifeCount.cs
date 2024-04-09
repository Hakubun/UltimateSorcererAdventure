using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtraLifeCount 
{
    // Start is called before the first frame update
    public int lifeCount;

    public ExtraLifeCount(int count){
        lifeCount = count;
    }
}
