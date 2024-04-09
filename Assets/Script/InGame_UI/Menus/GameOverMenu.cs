using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KillCount;
    [SerializeField] private TextMeshProUGUI CoinCount;
    [SerializeField] private Button gemContinueBtn;
    [SerializeField] private Button itemContinueBtn;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private AudioSource GameOverSound;


    public void showStatus(float kill, int coins)
    {
        KillCount.text = kill.ToString();
        CoinCount.text = coins.ToString();
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
        GameOverSound.Play();
        //SaveSystem.SaveExtraLife(10);
        int count = await SaveSystem.LoadExtraLife();
        itemCountText.text = count.ToString();
        //Debug.Log("enabled");
        if (gemContinueBtn.GetComponent<ContinueButton>().revived)
        {
            gemContinueBtn.interactable = false;
        }
        else
        {
            int gem = await SaveSystem.LoadGem();
            if (gem >= 10)
            {
                gemContinueBtn.interactable = true;
            }
            else
            {
                gemContinueBtn.interactable = false;
            }
        }

        if (itemContinueBtn.GetComponent<ContinueButton>().revived)
        {
            itemContinueBtn.interactable = false;
        }
        else
        {
            int extralife = await SaveSystem.LoadExtraLife();
            if (extralife >= 1)
            {
                itemContinueBtn.interactable = true;
            }
            else
            {
                itemContinueBtn.interactable = false;
            }
        }

    }
}
