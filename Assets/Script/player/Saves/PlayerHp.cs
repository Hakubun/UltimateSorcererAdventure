using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHp
{
    // Start is called before the first frame update
    public float Health;

    public PlayerHp(float hp){
        Health = hp;
    }

}
