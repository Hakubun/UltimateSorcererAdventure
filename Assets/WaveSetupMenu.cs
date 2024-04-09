using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSetupMenu : MonoBehaviour
{

    [SerializeField] Toggle[] InitialEnemySpawnAmount;
    [SerializeField] Toggle[] SpawnAmountIncrement;
    [SerializeField] Slider TotalWaveAmount_Slider;
    [SerializeField] TextMeshProUGUI TotalWaveAmount_Text;
    [SerializeField] Toggle CurrentInitialSPawnAmount;
    [SerializeField] Toggle CurrentSpawnIncrement;

    int InitialSpawnAmount;
    int EnemyIncrement;

    // Start is called before the first frame update
    void OnEnable()
    {
        // foreach (Toggle toggles in InitialEnemySpawnAmount)
        // {
        //     toggles.isOn = false;
        // }

        // foreach (Toggle toggles in SpawnAmountIncrement)
        // {
        //     toggles.isOn = false;
        // }

        TotalWaveAmount_Text.text = TotalWaveAmount_Slider.value.ToString();
    }

    public void InitialEnemySpawnSetup(int amount)
    {
        //pass the amount to GameManeger
        InitialSpawnAmount = amount;

    }

    public void EnemySpawnIncrementSetup(int amount)
    {
        //pass the amount to GameManeger

        EnemyIncrement = amount;
    }

    public void SetUpWave()
    {
        //float value = TotalWaveAmount_Slider.value;
        TotalWaveAmount_Text.text = TotalWaveAmount_Slider.value.ToString();

    }

    public void StartGame()
    {
        GameManager.instance.spawnAmount = InitialSpawnAmount;
        GameManager.instance.spawnIncrement = EnemyIncrement;
        GameManager.instance.totalWaves = (int)TotalWaveAmount_Slider.value;
        //call setup in gamemanager
        GameManager.instance.SetUpWave();
        //disable panel
        this.gameObject.SetActive(false);

    }
}
