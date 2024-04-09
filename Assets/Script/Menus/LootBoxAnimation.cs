using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxAnimation : MonoBehaviour
{
    [SerializeField] private GameObject OpenBox;
    [SerializeField] private GameObject ClosedBox;
    // Start is called before the first frame update
    public void OpenBoxIMG(){
        OpenBox.SetActive(true);
        ClosedBox.SetActive(false);
        

    }
}
