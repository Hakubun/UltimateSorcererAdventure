using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] LootSlots;
    [SerializeField] private GameObject BoxIMG;
    [SerializeField] private GameObject BoxOpenIMG;

    public void SetUpLoots(List<LootScriptableObject> Loots){
        for (int i = 0; i < 3; i++){
            LootSlots[i].GetComponent<LootSelectionBtn>().SetUpSlot(Loots[i]);
        }
    }

    public void ResetLootSlot(){
        BoxIMG.SetActive(true);
        BoxOpenIMG.SetActive(false);
    }
}
