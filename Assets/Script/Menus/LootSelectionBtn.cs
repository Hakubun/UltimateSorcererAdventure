using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LootSelectionBtn : MonoBehaviour
{
    public TMP_Text Info;
    public TMP_Text Name;
    public Image Icon;
    public Image Frame;
    public int ID;
    public float Multiplier;

    public ShopLogic Shop;
    // Start is called before the first frame update
    public void SetUpSlot(LootScriptableObject lootSO)
    {
        Info.text = lootSO.info;
        Name.text = lootSO.name;
        Icon.sprite = lootSO.Icon;
        Frame.sprite = lootSO.Frame;
        ID = lootSO.ID;
        Multiplier = lootSO.multiplier;
        this.gameObject.GetComponent<Button>().interactable = true;
    }

    public async void OnClick()
    {

        switch (ID)//0 = HP, 1 = Attack, 2 = Armor
        {
            case 0:
                //hp
                float hp = await SaveSystem.LoadPlayerHP();
                Debug.Log("Original hp = " + hp);
                hp *= Multiplier;
                SaveSystem.SavePlayerHP(hp);
                Debug.Log("Updated hp = " + hp);
                //Clicked(_price);
                break;
            case 1:
                //Attack
                float dmg = await SaveSystem.LoadPlayerDmg();
                Debug.Log("Original dmg = " + dmg);
                dmg *= Multiplier;
                SaveSystem.SavePlayerDmg(dmg);
                Debug.Log("Updated Dmg = " + dmg);
                
                break;
            case 2:
                //armor
                float armor = await SaveSystem.LoadPlayerArmor();
                Debug.Log("Original armor = " + armor);
                armor += Multiplier;
                SaveSystem.SavePlayerArmor(armor);
                Debug.Log("Updated Dmg = " + armor);
                
                break;
        }


        Shop.LootClicked();


    }



}
