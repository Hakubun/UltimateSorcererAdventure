using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float dmg;

    public PlayerData (player player){
        health = player.HP;
        dmg = player.dmg;
        
    }


}
