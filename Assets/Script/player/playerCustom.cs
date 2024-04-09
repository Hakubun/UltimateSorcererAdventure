using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCustom : MonoBehaviour
{
    [SerializeField] private Renderer PlayerRender;
    // Start is called before the first frame update
    [SerializeField] private Material[] MaterialList;

    [SerializeField] private GameObject[] WeaponList;
    public int materialID = 0;
    public int weaponID = 0;
    

    // Update is called once per frame
    private void Start() {
        PlayerCustomData data = SaveSystem.LoadPlayerCustom();
        materialID = data.MaterialID;
        weaponID = data.WeaponID;
        PlayerRender.sharedMaterial = MaterialList[materialID];
        
        for (int i = 0; i < WeaponList.Length; i++)
        {
            if (i == weaponID){
                WeaponList[i].SetActive(true);
            } else {
                WeaponList[i].SetActive(false);
            }
        }
    }

    public void changeCloth(){
        materialID += 1;
        if (materialID >= MaterialList.Length){
            materialID = 0;
        }
        PlayerRender.sharedMaterial = MaterialList[materialID];
        SaveSystem.SavePlayerCustom(this);
    }

    public void changeWeapon(){
        weaponID += 1;
        if (weaponID >= WeaponList.Length){
            weaponID = 0;
        }
        for (int i = 0; i < WeaponList.Length; i++ )
        {
            if (i == weaponID){
                WeaponList[i].SetActive(true);
            } else {
                WeaponList[i].SetActive(false);
            }
        }
        SaveSystem.SavePlayerCustom(this);
    }
}
