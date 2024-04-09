using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private StatusBarUI status;
    [SerializeField] private ShopAdsManager ShopAds;
    [SerializeField] private ShopLogic ShopLogics;
    [SerializeField] private GameObject AvailableTimeText;
    [SerializeField] private TextMeshProUGUI EnergyAdsButtonText;
    [SerializeField] private AdsWatchedData CounterData;
    // Start is called before the first frame update
    [SerializeField] private int ChestAdsCounter;
    [SerializeField] private int GoldAdsCounter;
    [SerializeField] private int GemAdsCounter;

    private DateTime nextEnergyAdsReload;
    [SerializeField] private int reloadDuration = 30;
    private bool isWaiting = true;


    //public int counter = 0;

    void Start()
    {
        Debug.Log("reward manager started");
        CounterData = SaveAdsCounter.LoadAdsCounter();
        ChestAdsCounter = CounterData.chestCount;
        GoldAdsCounter = CounterData.goldCount;
        GemAdsCounter = CounterData.gemCount;
        Load();
        // Debug.Log(nextEnergyAdsReload);
        //string timeValue = String.Format("Next Avaliable: {0:t}", nextEnergyAdsReload);
    }

    // Update is called once per frame
    void Update()
    {


        if (DateTime.Now >= nextEnergyAdsReload && isWaiting)
        {
            ShopAds.ReloadEnergy();
            EnergyAdsButtonText.text = "Open";
            isWaiting = false;
        }
        else if (DateTime.Now < nextEnergyAdsReload)
        {
            TimeSpan time = nextEnergyAdsReload - DateTime.Now;
            string timeValue = String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            EnergyAdsButtonText.text = timeValue;
        }
        else
        {
            EnergyAdsButtonText.text = "Open";
        }
    }

    public void Reward(int ID)
    {

        //1: Lucky Chest
        //2: One More Try!
        //3: Gold 100
        //4: gem 5 (removed this button, changed it to gold)
        //5: revive
        switch (ID)
        {
            case 1:
                // Debug.Log("WoodenChestReward chest reward!");

                ChestAdsCounter -= 1;
                ShopAds.UpdateChestCounterText(ChestAdsCounter);
                if (ChestAdsCounter > 0)
                {
                    ShopAds.ReloadChest();

                    SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);
                }
                else if (ChestAdsCounter == 0)
                {
                    SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);
                }
                ShopLogics.OpenWoodenChest();
                break;
            case 2:
                status.increaseEnergy(10);
                nextEnergyAdsReload = AddDuration(DateTime.Now, reloadDuration);
                Save();
                isWaiting = true;
                // Debug.Log(nextEnergyAdsReload);
                // AvailableTimeText.SetActive(true);
                // string timeValue = String.Format("Next Avaliable: {0:t}", nextEnergyAdsReload);
                // EnergyAdsButtonText.text = timeValue;
                break;
            case 3:
                ShopLogics.giveGold(240);

                GoldAdsCounter -= 1;
                ShopAds.UpdateGoldCounterText(GoldAdsCounter);

                if (GoldAdsCounter > 0)
                {
                    ShopAds.ReloadGold();

                    SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);
                }
                else if (GoldAdsCounter == 0)
                {
                    SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);
                }

                break;
            case 4://Not in the current version of shop
                   // Debug.Log("Add 5 Gem");
                GemAdsCounter -= 1;
                if (GemAdsCounter > 0)
                {
                    //ShopAds.ReloadGem();
                    SaveAdsCounter.UpdateAdsCounter(ChestAdsCounter, GoldAdsCounter, GemAdsCounter);
                }
                break;
            case 5:
                // Debug.Log("revive");
                GameObject.Find("GameManager").GetComponent<GameManager>().RevivePlayer();
                break;



        }
    }

    ///////Energy ads timer///////
    private void StartReloadEnergyAds()
    {

    }

    private DateTime AddDuration(DateTime datetime, int duration)
    {
        return datetime.AddSeconds(duration);
        //return datetime.AddMinutes(duration);
    }

    private DateTime StringToDate(string datetime)
    {
        if (String.IsNullOrEmpty(datetime))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(datetime);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString("nextEnergyAdsReload", nextEnergyAdsReload.ToString());
    }

    private void Load()
    {
        nextEnergyAdsReload = StringToDate(PlayerPrefs.GetString("nextEnergyAdsReload"));
    }



}
