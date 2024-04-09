using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KillCount;
    [SerializeField] private TextMeshProUGUI CoinCount;

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

    public void ResuemGame()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().ResumeGame();
    }
}
