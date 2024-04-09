using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEXP 
{
    public int exp;
    public int lvl;
    public int req;

    public PlayerEXP(int _exp,int _lvl,int _req){
        exp = _exp;
        lvl = _lvl;
        req = _req;
    }
}
