using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] List<UpgradeScriptableObject> Upgrades;
    [SerializeField] private StatusBarUI StatusBar;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button CoinRerollButton;
    [SerializeField] private TextMeshProUGUI CoinButtonText;
    [SerializeField] private int CoinRerollRequiredAmount;
    [SerializeField] private Button GemRerollButton;
    [SerializeField] private TextMeshProUGUI GemButtonText;
    [SerializeField] private int GemRerollRequiredAmount;
    [SerializeField] private int loadTimeDuration;
    private bool Rolled;
    private DateTime FinishLoadTime;


    void Start()
    {
        //SetupBtns();
        //checkRerollAvaliable();
        GemButtonText.text = GemRerollRequiredAmount.ToString();
        CoinButtonText.text = CoinRerollRequiredAmount.ToString();

    }

    private void OnEnable()
    {
        FinishLoadTime = DateTime.Now.AddSeconds(loadTimeDuration);
        Rolled = false;
        // Debug.Log(FinishLoadTime);
        checkRerollAvaliable();
    }

    private void OnDisable()
    {
        //remove all listener from button
        foreach(Button btn in buttons){
            btn.onClick.RemoveAllListeners();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (DateTime.Now < FinishLoadTime)
        {
            //Write a function that only setup button visually
            RollingButton();
        }
        else if (DateTime.Now >= FinishLoadTime && !Rolled)
        {
            SetupBtns();
            Rolled = true;
        }


    }

    public void SetupBtns()
    {
        // Reset the skill ID when setting up the buttons

        List<UpgradeScriptableObject> selectedButtons = _getRandomUpgrade();

        for (int i = 0; i < 3; i++)
        {
            buttons[i].GetComponent<UpgradeButton__Upgrade_Panel>().SetupBtn(selectedButtons[i]);


            //int currentSkillID = selectedButtons[i]._skillID;
            //buttons[i].onClick.AddListener(() => )
            //btn.GetComponent<Button>().onClick.AddListener(() => );//gm.UpgradeControl(currentSkillID)
            //buttons[i].enabled = true;
            UpgradeButton__Upgrade_Panel ButtonFuction = buttons[i].GetComponent<UpgradeButton__Upgrade_Panel>();
            buttons[i].GetComponent<Button>().onClick.AddListener(() => ButtonFuction.OnClick());
            // Debug.Log("FInished setup");
        }
    }

    public void RollingButton()//this function is for visual purpose only
    {
        List<UpgradeScriptableObject> selectedButtons = _getRandomUpgrade();

        for (int i = 0; i < 3; i++)
        {
            buttons[i].GetComponent<UpgradeButton__Upgrade_Panel>().SetupBtn(selectedButtons[i]);


            //int currentSkillID = selectedButtons[i]._skillID;
            //buttons[i].onClick.AddListener(() => )
            //btn.GetComponent<Button>().onClick.AddListener(() => );//gm.UpgradeControl(currentSkillID)
            //buttons[i].enabled = false;
        }
    }

    private List<UpgradeScriptableObject> _getRandomUpgrade()
    {
        List<UpgradeScriptableObject> types = new List<UpgradeScriptableObject>(Upgrades);
        List<UpgradeScriptableObject> selectedButtons = new List<UpgradeScriptableObject>();

        for (int i = 0; i < 3 && types.Count > 0; i++)
        {
            int choose = Random.Range(0, types.Count);
            selectedButtons.Add(types[choose]);
            types.RemoveAt(choose);
        }

        return selectedButtons;
    }

    public void reroll()
    {
        FinishLoadTime = DateTime.Now.AddSeconds(loadTimeDuration);
        Rolled = false;
    }

    public async void rerollGem()
    {
        int gemCount = await SaveSystem.LoadGem();
        FinishLoadTime = DateTime.Now.AddSeconds(loadTimeDuration);
        gemCount -= GemRerollRequiredAmount;
        SaveSystem.SaveGem(gemCount);
        StatusBar.UpdateGem();
        checkRerollAvaliable();
    }

    public async void rerollCoin()
    {
        int coinCount = await SaveSystem.LoadCoin();
        FinishLoadTime = DateTime.Now.AddSeconds(loadTimeDuration);
        coinCount -= CoinRerollRequiredAmount;
        SaveSystem.SaveCoin(coinCount);
        StatusBar.UpdateCoin();
        checkRerollAvaliable();
    }

    public async void checkRerollAvaliable()
    {

        int gemCount = await SaveSystem.LoadGem();
        int coinCount = await SaveSystem.LoadCoin();

        if (gemCount < GemRerollRequiredAmount)
        {
            GemRerollButton.interactable = false;
        }
        else
        {
            GemRerollButton.interactable = true;
        }

        if (coinCount < CoinRerollRequiredAmount)
        {
            CoinRerollButton.interactable = false;
        }
        else
        {
            CoinRerollButton.interactable = true;
        }

    }
}
