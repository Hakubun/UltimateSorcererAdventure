using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ChallengeTimer : MonoBehaviour
{
    public float elapsedTime = 0f;
    private TextMeshProUGUI timerText;

    [SerializeField] private enemyspawner gameManager; // Reference to the GameManager

    private float six = 360f;
    private bool spawning = true; // Control variable for enemy spawning

    void Start()
    {
        // Assuming the TextMeshProUGUI component is attached to the same GameObject
        timerText = GetComponent<TextMeshProUGUI>();
        // Find the GameManager in the scene
        //gameManager = GameObject.FindObjectOfType<GameManager>();

        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found. Please attach it to the same GameObject.");
        }

        // Start the enemy spawning routine
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        // Update the TextMeshPro text with the formatted time
        UpdateTimerText();
    }

    private IEnumerator SpawnRoutine()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(six);
            gameManager.SpawnBoss();
            // Wait for a random time before spawning more enemies
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            // Format the elapsed time as days, hours, minutes, and seconds
            string formattedTime = FormatTime(elapsedTime);

            // Update the TextMeshPro text
            timerText.text = "Time Survived: " + formattedTime;
        }
    }

    string FormatTime(float timeInSeconds)
    {
        // Convert time to days, hours, minutes, and seconds
        int hours = Mathf.FloorToInt((timeInSeconds % 86400f) / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        // Format the time as DD:HH:MM:SS
        string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

        return formattedTime;
    }

    //Funciton to save the time the player has survived, so if we have a leaderboard it shows this with the kill count.
}
