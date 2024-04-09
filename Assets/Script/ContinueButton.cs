using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private int ID; //0 = gem, 1 = extralife item
    [SerializeField] private int req_amount;
    [SerializeField] private Button button;
   // [SerializeField] private GameManager gm;
    public bool revived;

    // Start is called before the first frame update
    

    public async void clicked()
    {
        if (ID == 0)
        {
            int gem = await SaveSystem.LoadGem();
            gem -= req_amount;
            SaveSystem.SaveGem(gem);
            GameObject.Find("GameManager").GetComponent<GameManager>().RevivePlayer();
            revived = true;
        }
        else if (ID == 1)
        {
            int extralife = await SaveSystem.LoadExtraLife();
            extralife -= req_amount;
            SaveSystem.SaveExtraLife(extralife);
            GameObject.Find("GameManager").GetComponent<GameManager>().RevivePlayer();
            revived = true;
        }

    }
}
