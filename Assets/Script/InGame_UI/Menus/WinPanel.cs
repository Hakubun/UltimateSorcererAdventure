using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private AudioSource WinSound;
    [SerializeField] private TextMeshProUGUI CoinCount;
    [SerializeField] private TextMeshProUGUI ExpCount;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI lvlProgressText;
    [SerializeField] private Button goldMultiplierBtn;
    [SerializeField] private RewardedAdsButton AdsMultiplierBtn;
    [SerializeField] private Slider lvlSlider;
    [SerializeField] private GameManager gm;
    [SerializeField] private AccountManager am;
    private int coin;

    private void Awake() {
        AdsMultiplierBtn.LoadAd();
    }
    public void RestartGame()
    {
        // Restart the current game scene
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void returnToMenu()
    {
        // Return to the main menu scene
        SceneManager.LoadScene(1);


    }

    private async void OnEnable()
    {

        WinSound.Play();
        coin = gm.Gold();
        CoinCount.text = gm.Gold().ToString();
        ExpCount.text = gm.GetExp().ToString();
        PlayerEXP data = await SaveSystem.LoadPlayerEXP();
        int _exp = data.exp;
        int _lvl = data.lvl;
        int _req = data.req;

        _exp += gm.GetExp();

        while (_exp >= _req)
        {
            _lvl += 1;
            _exp -= _req;
            _req += 100;
            am.UpdateLvl(_exp, _lvl, _req);
            Debug.Log((float)_exp / (float)_req);
            lvlSlider.value = (float)_exp / (float)_req;
            lvlText.text = _lvl.ToString();
            lvlProgressText.text = _exp.ToString() + "/" + _req.ToString();
        }
        if (_exp < _req)
        {
            am.UpdateLvl(_exp, _lvl, _req);
            lvlSlider.value = (float)_exp / (float)_req;
            lvlText.text = _lvl.ToString();
            lvlProgressText.text = _exp + "/" + _req;
        }

        int goldMultiply = await SaveSystem.LoadGoldMultiplier();
        Debug.Log(goldMultiply);
        if (goldMultiply > 0)
        {
            goldMultiplierBtn.interactable = true;
        }
        else
        {
            goldMultiplierBtn.interactable = false;
        }

    }

    public async void SaveCoin()
    {
        // Save the player's coin count
        int count = await SaveSystem.LoadCoin();
        Debug.Log(count);
        coin += count;
        Debug.Log(coin);
        SaveSystem.SaveCoin(coin);
    }

    public async void Multiply()
    {
        Debug.Log("Multiply()");
        int multiplyCount = await SaveSystem.LoadGoldMultiplier();
        multiplyCount -= 1;
        SaveSystem.SaveGoldMultiplier(multiplyCount);
        SaveCoin();
        SceneManager.LoadScene(1);

    }

    public async void AdsMultiply()
    {
        Debug.Log("AdsMultiply()");
      
        SaveCoin();
        SceneManager.LoadScene(1);
    }

    public async void finishGame()
    {
        
        SceneManager.LoadScene(1);
    }
}
