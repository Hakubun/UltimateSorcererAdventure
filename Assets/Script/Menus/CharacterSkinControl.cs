using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private playerCustom[] Characters;

    public void changeCloth(){
        foreach (playerCustom script in Characters)
        {
            script.changeCloth();
            
        }
    }

    public void changeWeapon(){
        foreach (playerCustom script in Characters)
        {
            script.changeWeapon();
            
        }
    }
}
