using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCustomData 
{
    // Start is called before the first frame update
    public int MaterialID;
    public int WeaponID;

    public PlayerCustomData(playerCustom Custom){
        MaterialID = Custom.materialID;
        WeaponID = Custom.weaponID;
    }
}
