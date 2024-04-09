using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private AudioSource SkillButtonClickSound; //Drag and drop audio source in each level
    [SerializeField] private PlayerSkillControlButton[] ControlButtons;


    [SerializeField] private player player;
    [SerializeField] public enemyspawner spawnmanager;
    [SerializeField] private SkillManager sm;
    [SerializeField] private AccountManager AM;
    //[SerializeField] private TextMeshProUGUI KillCount; // need to change this to progress bar later
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject SkillPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private PlayerStatus PlayerStatus;
    [SerializeField] Button ReviveButton;
    [SerializeField] public int totalWaves;
    [SerializeField] int currentWaves = 0;
    [SerializeField] Transform enemyContainerTransform;
    public int containerCount;
    [SerializeField] int currentWavesEnemyCount;
    [SerializeField] public int spawnAmount;
    [SerializeField] public int spawnIncrement;
    [SerializeField] int rareSpawnAmount;
    private float kills = 0;
    [SerializeField] private int reward_exp;
    [SerializeField] private int coins;
    public bool pauseGame;
    [SerializeField] private int lvl;
    [SerializeField] private int currentlvl;
    [SerializeField] private float Timer;
    //[SerializeField] private int waves;
    [SerializeField] private int currentwaveCount;
    private bool gameover;
    private bool adWatched;

    //Boss Killing tracker - Challenge Level.
    private bool bossSpawned = false;
    private int bossSpawnCounter = 0;
    private float bossHealth = 0f;
    private bool gameWon = false;
    private string currentLevel;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentlvl = player.getLevel();

        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
        PausePanel.SetActive(false);
        gameover = false;
        pauseGame = true;
        //PlayerStatus.UpdateTimer(0, Timer);
        PlayerStatus.UpdateLvlText(currentlvl);
        PlayerStatus.UpdateCoin(coins);
        PlayerStatus.UpdateKill((int)kills);
        SkillPanel.SetActive(false);
        adWatched = false;

        //Activate Player input setup menu

        //user input of initial spawn amounts: 10/20/30/40/50

        //user input of spawnAmount increment: 5/10/15/20

        //user input of total wave: 5/10/15/20


        //todo move this to a different function


        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;


    }

    void Update()
    {
        if (!gameWon)
        {

            // if (kills >= killRequirment && !bossSpawned)
            // {
            //     spawnmanager.SpawnBoss();
            //     bossSpawned = true;
            // }
        }
        if (pauseGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    private void FixedUpdate()
    {
        // Update the player's level and handle UI based on game state
        lvl = player.getLevel();
        if (lvl > currentlvl && !gameover)
        {

            pauseGame = true;
            SkillPanel.SetActive(true);
            currentlvl = lvl;
            PlayerStatus.UpdateLvlText(currentlvl);
        }
    }

    public void BossDefeated()
    {
        bossSpawned = false;
        bossSpawnCounter++;
        //spawnmanager.spawning = true;
        Debug.Log(bossSpawnCounter);
        if (bossSpawnCounter >= 5)
        {
            win();
        }
        else
        {
            currentWavesEnemyCount = spawnmanager.SpawnWave(spawnAmount, rareSpawnAmount);
            currentWaves++;
            spawnAmount += 10;
            rareSpawnAmount += 5;
            PlayerStatus.UpdateLevelProgress(currentWaves, totalWaves);
        }
    }


    public void addKill(float exp)
    {
        // Increment kill count and coins
        kills += 1;
        currentWavesEnemyCount--;
        containerCount = enemyContainerTransform.childCount;
        coins += 1;
        reward_exp += (int)exp;
        PlayerStatus.UpdateCoin(coins);
        PlayerStatus.UpdateKill((int)kills);
        if (currentWavesEnemyCount == 0)
        {
            if (currentLevel == "Level_ChallengeMode")
            {
                if (currentWaves % 10 == 0)
                {
                    spawnmanager.SpawnBossFromArray();
                    PlayerStatus.UpdateLevelProgressInfo("Boss Fight!");
                }
                else
                {
                    currentWavesEnemyCount = spawnmanager.SpawnWave(spawnAmount, rareSpawnAmount);
                    currentWaves++;
                    //spawnAmount += 10;
                    //rareSpawnAmount += 5;
                    PlayerStatus.UpdateLevelProgress(currentWaves, totalWaves);
                }
            }
            else
            {
                if (currentWaves == totalWaves)
                {
                    //spawnBoss
                    spawnmanager.SpawnBoss();
                    PlayerStatus.UpdateLevelProgressInfo("Boss Fight!");
                }
                else
                {
                    currentWavesEnemyCount = spawnmanager.SpawnWave(spawnAmount, rareSpawnAmount);
                    currentWaves++;
                    spawnAmount += spawnIncrement;
                    rareSpawnAmount += 5;
                    PlayerStatus.UpdateLevelProgress(currentWaves, totalWaves);
                }
            }
        }
    }

    public void addCoin(int _amount)
    {
        coins += _amount;
        PlayerStatus.UpdateCoin(coins);

    }

    public void PauseGame()
    {
        // Toggle game pause status
        PausePanel.SetActive(true);
        PausePanel.GetComponent<PauseMenu>().showStatus(kills, coins);
        pauseGame = true;
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
        pauseGame = false;
    }


    public void UpgradeControl(int _skillID)
    {
        upgradeSkills(_skillID);

        // Debug.Log("This is the UpgradeControl Function ID" + _skillID);

        pauseGame = false;

        SkillPanel.SetActive(false);
    }

    void upgradeSkills(int _id)
    {
        // Apply skill upgrades based on the selected skill ID
        switch (_id)
        {
            case 0:
                player.HPboost();
                break;
            case 1:
                player.IncreaseRange();
                break;
            case 2: // Meteor skill
                sm.UpgradeSkill("Meteor");
                break;
            case 3: // Lightning skill
                sm.UpgradeSkill("Lightning");
                break;
            case 4: // KnifeRain skill
                sm.UpgradeSkill("KnifeRain");
                break;
            case 5: // Ice skill
                sm.UpgradeSkill("Ice");
                break;
            case 6: // Flower skill
                sm.UpgradeSkill("Flower");
                break;
            default:
                break;
        }

        SkillButtonClickSound.Play();
    }


    public async void gameOver()
    {
        // Handle game over state
        gameover = true;
        pauseGame = true;
        GameOverPanel.SetActive(true);
        GameOverPanel.GetComponent<GameOverMenu>().showStatus(kills, coins);
        if (currentLevel == "Level_ChallengeMode")
        {
            int count = await SaveSystem.LoadCoin();
            count += coins;
            SaveSystem.SaveCoin(count);
        }

        if (!adWatched)
        {
            ReviveButton.GetComponent<RewardedAdsButton>().LoadAd();
        }
    }

    public int Gold()
    {
        // Retrieve the current coin count
        return coins;
    }

    public int GetExp()
    {
        return reward_exp;
    }

    public void win()
    {
        // Handle the win state
        Invoke(nameof(FinishGame), 5f);
    }

    async public void FinishGame()
    {
        pauseGame = true;
        gameover = true;
        PausePanel.SetActive(false);
        WinPanel.SetActive(true);
        int count = await SaveSystem.LoadCoin();
        count += coins;
        SaveSystem.SaveCoin(count);
        //probably need to get rid of

    }

    public void returnToMenu()
    {
        // Return to the main menu scene
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        // Restart the current game scene
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void RevivePlayer()
    {
        // Revive the player and resume the game
        player.Revive();
        pauseGame = false;
        gameover = false;
        GameOverPanel.SetActive(false);
        adWatched = true;
    }

    public void SetUpWave()
    {
        currentWavesEnemyCount = spawnmanager.SpawnWave(spawnAmount, rareSpawnAmount);
        spawnAmount += spawnIncrement;
        rareSpawnAmount += 5;
        currentWaves += 1;
        PlayerStatus.UpdateLevelProgress(currentWaves, totalWaves);
        pauseGame = false;
    }



}
