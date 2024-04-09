using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ShopAdsManager : MonoBehaviour
{
    [SerializeField] private AdsInitializer initializer;
    [SerializeField] private RewardedAdsButton EnergyButtons;
    [SerializeField] private RewardedAdsButton ChestButton;
    [SerializeField] private TextMeshProUGUI ChestCounterText;
    [SerializeField] private RewardedAdsButton GoldButton;
    [SerializeField] private TextMeshProUGUI GoldCounterText;
    [SerializeField] private RewardedAdsButton GemButton;

    [SerializeField] private AdsWatchedData CounterData;

    [SerializeField] private int ChestAdsCounter;
    [SerializeField] private int GoldAdsCounter;
    [SerializeField] private int GemAdsCounter;
    // Start is called before the first frame update

    //Logic of this script: the script will first load current time using (Debug.Log(System.DateTime.UtcNow.ToLocalTime()) then load from SaveDateTime to get the saved time which is the saved time to see if the date passed more than 24 hours
    private void Awake()
    {

    }
    void Start()
    {
        Debug.Log("Shop ads manager started");
        DateTime currentDateTime = DateTime.Now;
        RewardTimeData data = SaveDateTime.LoadRewardTime();
        DateTime compareDate = DateTime.Parse(data.DateString);
        CounterData = SaveAdsCounter.LoadAdsCounter();
        ChestAdsCounter = CounterData.chestCount;
        GoldAdsCounter = CounterData.goldCount;
        GemAdsCounter = CounterData.gemCount;
        ChestCounterText.text = CounterData.chestCount.ToString();
        GoldCounterText.text = CounterData.goldCount.ToString();
        // Debug.Log(Application.persistentDataPath);


        if (DateTime.Compare(currentDateTime, compareDate) >= 0)
        {
            NewDayReloadAds();
        }
        else
        {
            //ReloadEnergy();

            if (CounterData.chestCount > 0)
            {
                ReloadChest();

            }
            if (CounterData.goldCount > 0)
            {
                ReloadGold();

            }
            if (CounterData.gemCount > 0)
            {
                //ReloadGem();
            }
            // Debug.Log("not reset ready");
        }



        //Debug.Log("called start");


    }

    void NewDayReloadAds()
    {
        //SaveDateTime.SaveDate(DateTime.Now.AddDays(1));
        // EnergyButtons.LoadAd();
        Debug.Log("calling chest ads");
        ChestButton.LoadAd();
        Debug.Log("chest ads");
        GoldButton.LoadAd();
        Debug.Log("gold ads");
        //GemButton.LoadAd();
        SaveDateTime.SaveDate(DateTime.Now.AddDays(1));
        SaveAdsCounter.UpdateAdsCounter(5, 5, 5);

    }

    public void ReloadEnergy()
    {
        EnergyButtons.LoadAd();
    }

    void UpdateCounter()
    {
        SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);


    }
    public void ReloadChest()
    {
        ChestButton.LoadAd();

        UpdateCounter();

    }

    public void ReloadGold()
    {
        GoldButton.LoadAd();

        UpdateCounter();
    }

    // public void ReloadGem()
    // {
    //     GemButton.LoadAd();

    //     UpdateCounter();
    // }

    public void UpdateChestCounterText(int counter)
    {
        ChestCounterText.text = counter.ToString();

    }

    public void UpdateGoldCounterText(int counter)
    {
        GoldCounterText.text = counter.ToString();
    }
}
