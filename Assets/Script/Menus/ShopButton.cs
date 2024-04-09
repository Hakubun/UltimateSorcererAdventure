using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private AudioSource AcceptSound;
    [SerializeField] private AudioSource ErrorSound;
    [SerializeField] private StatusBarUI StatusBar;
    [SerializeField] private GameObject ErrorMessage;
    [SerializeField] private TextMeshProUGUI ErrorText;
    private ShopLogic Shop;
    [SerializeField] private int ButtonID;
    private void Start()
    {
        StatusBar = GameObject.FindWithTag("Status").GetComponent<StatusBarUI>();
        Shop = GameObject.Find("ShopMenu").GetComponent<ShopLogic>();
    }



    // Start is called before the first frame update
    public async void SpendGold(int price)
    {

        int coinNum = await SaveSystem.LoadCoin();
        if (coinNum >= price)
        {
            coinNum -= price;
            SaveSystem.SaveCoin(coinNum);
            StatusBar.UpdateCoin();

            Shop.SpendGold(ButtonID);
            AcceptSound.Play();
        }
        else
        {
            ErrorText.text = "Not Enough Coin!";
            ErrorMessage.SetActive(true);
            ErrorSound.Play();
        }
        // Debug.Log(price + " Amount of gold spent");


    }

    public async void SpendGem(int price)
    {
        int gemNum = await SaveSystem.LoadGem();
        if (gemNum >= price)
        {
            gemNum -= price;
            SaveSystem.SaveGem(gemNum);
            StatusBar.UpdateGem();

            Shop.SpendGem(ButtonID);
            AcceptSound.Play();
        }
        else
        {
            ErrorText.text = "Not Enough Gem!";
            ErrorMessage.SetActive(true);
            ErrorSound.Play();
        }
    }
}
