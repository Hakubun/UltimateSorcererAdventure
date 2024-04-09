using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float initialTime = 300f; // Initial countdown time in seconds
    private float currentTime; // Current countdown time

    private float bossSpawnInterval = 360f; // How long before a boss spawns
    private string currentLevel;


    private TextMeshProUGUI timerText;
    private bool spawning = true; // Control variable for enemy spawning
    [SerializeField] private enemyspawner SpawnManager; // Reference to the SpawnManager
    [SerializeField] private PlayerStatus Status;



    void Start()
    {
        // Get the current level name
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        timerText = GetComponent<TextMeshProUGUI>();

        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found. Please attach it to the same GameObject.");
        }

        // Set the initial time
        currentTime = initialTime;

        // Start the countdown routine
        StartCoroutine(CountdownRoutine());
    }

    void Update()
    {
        // Update the TextMeshPro text with the formatted time
        UpdateTimerText();
    }

    private IEnumerator CountdownRoutine()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            if (currentLevel == "Level_ChallengeMode")
            {
                // Decrease the current time
                currentTime -= 1f;

                if ((initialTime - currentTime) >= bossSpawnInterval)
                {
                    SpawnManager.SpawnBossFromArray();
                    // Reset the current time for the next boss spawn
                    initialTime = currentTime;
                    currentTime = initialTime;
                    UpdateTimerText(); // Update timer
                }
                if (currentTime <= 0)
                {
                    SpawnManager.SpawnBossFromArray();
                    initialTime = currentTime;
                    currentTime = initialTime;
                    UpdateTimerText(); // Update one last time to display 00:00
                    gameObject.SetActive(false); // Turn off the GameObject
                }

            }
            else
            {

                // Decrease the current time
                currentTime -= 1f;

                // Check if the countdown has reached 0
                if (currentTime <= 0f)
                {
                    // Spawn the boss and reset the timer
                    SpawnManager.SpawnBoss();
                    currentTime = initialTime;
                    UpdateTimerText(); // Update one last time to display 00:00
                    gameObject.SetActive(false); // Turn off the GameObject
                }
            }
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Format the remaining time as minutes and seconds
            string formattedTime = FormatTime(currentTime);

            // Update the TextMeshPro text
            timerText.text = formattedTime;

            //Status.UpdateTimer((currentTime), initialTime);
            //Debug.Log((initialTime - currentTime) / initialTime);
        }
    }

    string FormatTime(float timeInSeconds)
    {
        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        // Format the time as MM:SS
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        return formattedTime;
    }

}
