using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerArmor 
{
    // Start is called before the first frame update
    public float Armor;

    public PlayerArmor(float amount){
        Armor = amount;
    }
}
