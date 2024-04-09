using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class StatusBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EnergyCount;
    [SerializeField] private TextMeshProUGUI CoinCount;
    [SerializeField] private TextMeshProUGUI GemCount;
    [SerializeField] private TextMeshProUGUI timerText;

    private int maxEnergy = 100;
    private int currentEnergy;
    private int restoreDuration = 5;
    private DateTime nextEnergyTime;
    private DateTime lastEnergyTime;
    private bool isRestoring = false;

    // Start is called before the first frame update
    async void Start()
    {
        int coinNum = await SaveSystem.LoadCoin();
        int gemNum = await SaveSystem.LoadGem();
        CoinCount.text = coinNum.ToString();
        GemCount.text = gemNum.ToString();

        if (!PlayerPrefs.HasKey("currentEnergy"))
        {
            PlayerPrefs.SetInt("currentEnergy", maxEnergy);
            Load();
            StartCoroutine(RestoreEnergy());
        }
        else
        {
            Load();
            StartCoroutine(RestoreEnergy());
        }
        UpdateEnergy();
    }
    private async void OnEnable()
    {
        int coinNum = await SaveSystem.LoadCoin();
        int gemNum = await SaveSystem.LoadGem();
        CoinCount.text = coinNum.ToString();
        GemCount.text = gemNum.ToString();
    }

    public async void UpdateCoin()
    {
        int coinNum = await SaveSystem.LoadCoin();
        CoinCount.text = coinNum.ToString().ToString();
    }

    public async void UpdateGem()
    {
        int gemNum = await SaveSystem.LoadGem();
        GemCount.text = gemNum.ToString();
    }


    ///////////////////////////////////////////////////////////////
    ///////////////////Energy Function/////////////////////////////
    ///////////////////////////////////////////////////////////////
    public void UseEnergy(int _amount)
    {
        if (currentEnergy >= _amount)
        {
            currentEnergy -= _amount;
            UpdateEnergy();

            if (isRestoring == false)
            {
                if (currentEnergy + 1 == maxEnergy)
                {
                    nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
                }
                StartCoroutine(RestoreEnergy());
            }
        }
        else
        {
            //Debug.Log("Insufficient Energy");

        }
    }

    public void increaseEnergy(int amount)
    {
        // if (currentEnergy < maxEnergy)
        // {

        // }
        currentEnergy += amount;
        UpdateEnergy();
    }


    private IEnumerator RestoreEnergy()
    {
        UpdateEnergyTimer();
        isRestoring = true;

        while (currentEnergy < maxEnergy)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime nextDateTime = nextEnergyTime;
            bool isEnergyAdding = false;

            // Debug.Log("Current="+ currentDateTime);
            // Debug.Log("Next = "+ nextDateTime);
            while (currentDateTime > nextDateTime)
            {

                if (currentEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currentEnergy++;
                    UpdateEnergy();
                    DateTime timeToAdd = lastEnergyTime > nextDateTime ? lastEnergyTime : nextDateTime;
                    nextDateTime = AddDuration(timeToAdd, restoreDuration);
                }
                else
                {
                    break;
                }
            }
            if (isEnergyAdding == true)
            {
                lastEnergyTime = DateTime.Now;
                nextEnergyTime = nextDateTime;
            }

            UpdateEnergyTimer();
            UpdateEnergy();
            Save();
            yield return null;
        }

        isRestoring = false;

    }
    private DateTime AddDuration(DateTime datetime, int duration)
    {
        //return datetime.AddSeconds(duration);
        return datetime.AddMinutes(duration);
    }

    private void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            timerText.text = "Full";
            return;
        }

        TimeSpan time = nextEnergyTime - DateTime.Now;
        //Changed the D and the 2 to 0, String.Format("{0:D2}:{1:D1}", time.Minutes, time.Seconds);
        string timeValue = String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        timerText.text = timeValue;

    }

    private void UpdateEnergy()
    {

        if (currentEnergy > maxEnergy)
        {
            //currentEnergy = maxEnergy;
            timerText.text = "Full";
        }
        EnergyCount.text = currentEnergy.ToString() + "/" + maxEnergy;
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

    private void Load()
    {
        currentEnergy = PlayerPrefs.GetInt("currentEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastEnergyTime = StringToDate(PlayerPrefs.GetString("lastEnergyTime"));

    }

    private void Save()
    {
        PlayerPrefs.SetInt("currentEnergy", currentEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastEnergyTime", lastEnergyTime.ToString());
    }

    public int getEnergy()
    {
        return currentEnergy;
    }

}
