using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton__Upgrade_Panel : MonoBehaviour
{
    [SerializeField] private AudioSource AcceptSound;
    [SerializeField] private AudioSource ErrorSound;
    [SerializeField] private GameObject ErrorMessage;
    [SerializeField] private TextMeshProUGUI ErrorText;
    public TMP_Text ButtonInfo;
    public TMP_Text Price;
    public Image ButtonIcon;
    public Image ButtonFrame;
    public int ButtonID;
    public float ButtonMultiplier;
    public GameObject Cover;

    private int _price;
    [SerializeField] private StatusBarUI StatusBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupBtn(UpgradeScriptableObject BtnSO)
    {

        ButtonInfo.text = BtnSO.info;
        Price.text = BtnSO.price.ToString();
        _price = BtnSO.price;
        ButtonIcon.sprite = BtnSO.Icon;
        ButtonFrame.sprite = BtnSO.Frame;
        ButtonID = BtnSO.ID;
        ButtonMultiplier = BtnSO.multiplier;
        this.gameObject.GetComponent<Button>().interactable = true;
        Cover.SetActive(false);

    }

    public async void OnClick()
    {
        int coinCount = await SaveSystem.LoadCoin();
        if (coinCount >= _price)
        {
            switch (ButtonID)
            {
                case 0:
                    //hp
                    float hp = await SaveSystem.LoadPlayerHP();
                    Debug.Log("Original hp = " + hp);
                    hp *= ButtonMultiplier;
                    SaveSystem.SavePlayerHP(hp);
                    Debug.Log("Updated hp = " + hp);
                    Clicked(_price);
                    break;
                case 1:
                    //Attack
                    float dmg = await SaveSystem.LoadPlayerDmg();
                    Debug.Log("Original dmg = " + dmg);
                    dmg *= ButtonMultiplier;
                    SaveSystem.SavePlayerDmg(dmg);
                    Debug.Log("Updated Dmg = " + dmg);
                    Clicked(_price);
                    break;
                case 2:
                    //armor
                    float armor = await SaveSystem.LoadPlayerArmor();
                    Debug.Log("Original armor = " + armor);
                    armor += ButtonMultiplier;
                    SaveSystem.SavePlayerArmor(armor);
                    Debug.Log("Updated Dmg = " + armor);
                    Clicked(_price);
                    break;
                case 3:
                    //gold multiplier
                    int MultiplierCount = await SaveSystem.LoadGoldMultiplier();
                    Debug.Log("Original MultiplierCount = " + MultiplierCount);
                    MultiplierCount += (int)ButtonMultiplier;
                    SaveSystem.SaveGoldMultiplier(MultiplierCount);
                    Debug.Log("Updated MultiplierCount = " + MultiplierCount);
                    Clicked(_price);
                    break;
                case 4:
                    //Extra life
                    int LifeCount = await SaveSystem.LoadExtraLife();
                    Debug.Log("Original lifeCount = " + LifeCount);
                    LifeCount += (int)ButtonMultiplier;
                    SaveSystem.SaveExtraLife(LifeCount);
                    Debug.Log("Updated LifeCount = " + LifeCount);
                    Clicked(_price);

                    break;
            }
            Debug.Log(ButtonID + " pressed");
            AcceptSound.Play();
        }
        else
        {
            ErrorText.text = "Not Enough Coin!";
            ErrorMessage.SetActive(true);
            ErrorSound.Play();
        }
    }

    async void Clicked(int _price)
    {
        int coinCount = await SaveSystem.LoadCoin();
        coinCount -= _price;
        SaveSystem.SaveCoin(coinCount);
        StatusBar.UpdateCoin();
        this.gameObject.GetComponent<Button>().interactable = false;
        //this.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        Cover.SetActive(true);



    }
}
