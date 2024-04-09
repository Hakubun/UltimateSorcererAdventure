using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialGM : MonoBehaviour
{
    public static tutorialGM Instance { get; private set; } // Singleton instance
    [SerializeField] private GameObject[] MeleePrefabs;
    [SerializeField] private GameObject exp;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SkillManager sm;

    public int kills = 0;
    [SerializeField] private int reward_exp;
    [SerializeField] private int coins;
    [SerializeField] private PlayerStatus PlayerStatus;
    public GameObject TutorialPanel;

    public GameObject PausePanel;
    public bool pauseGame;
    public int enemycount;

    public GameObject SkillPanel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: Keep the instance alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicates
        }
    }
    void Start()
    {
        PlayerStatus.UpdateLevelProgress(0, 4);
    }


    private void Update()
    {
        if (pauseGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SpawnEnemy()
    {
        if (MeleePrefabs == null || MeleePrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not set!");
            return;
        }

        // Randomly select an enemy prefab
        int index = Random.Range(0, MeleePrefabs.Length);
        GameObject enemyPrefab = MeleePrefabs[index];

        // Determine a spawn position near the player
        Vector3 spawnOffset = Random.insideUnitSphere * 5f; // Spawn within a 5 unit radius from the player
        spawnOffset.y = 0; // Assuming you want to spawn on the same ground level as the player
        Vector3 spawnPosition = playerTransform.position + spawnOffset;

        // Instantiate the enemy at the chosen position and default rotation
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void LevelUp()
    {
        PlayerStatus.UpdateLevelProgress(2, 4);
        TutorialPanel.SetActive(true);
        TutorialPanel.GetComponent<TutorialManager>().pause = true;
        TutorialPanel.GetComponent<TutorialManager>().SetText("Yes! You have defeated the first enemy, now pick up the exp gem so you can level up");
        ItemDrop();
    }

    public void ItemDrop()
    {
        Vector3 spawnOffset = Random.insideUnitSphere * 5f; // Spawn within a 5 unit radius from the player
        spawnOffset.y = 0; // Assuming you want to spawn on the same ground level as the player
        Vector3 spawnPosition = playerTransform.position + spawnOffset;

        // Instantiate the enemy at the chosen position and default rotation
        Instantiate(exp, spawnPosition, Quaternion.identity);
    }

    public void MovementComplete()
    {
        PlayerStatus.UpdateLevelProgress(1, 4);
        TutorialPanel.SetActive(true);
        TutorialPanel.GetComponent<TutorialManager>().pause = true;
        TutorialPanel.GetComponent<TutorialManager>().SetText("Nice! now u know how to move you character, try press dash button and see what it does, also, here is an enemy");

        SpawnEnemy();
    }

    public void addKill(int exp)
    {
        // Increment kill count and coins
        kills += 1;

        coins += 1;
        reward_exp += exp;
        PlayerStatus.UpdateCoin(coins);
        PlayerStatus.UpdateKill((int)kills);

        if (kills == 1)
        {
            LevelUp();
        }
        else if (kills >= 6)
        {
            PlayerStatus.UpdateLevelProgress(4, 4);
            TutorialPanel.SetActive(true);
            TutorialPanel.GetComponent<TutorialManager>().pause = true;
            TutorialPanel.GetComponent<TutorialManager>().SetText("Alright! You have completed the tutorial! Nice! go back and check out the info panel for more information");
            //go back to main menu
            TutorialPanel.GetComponent<TutorialManager>().finished = true;
        }
    }

    public void Skill()
    {
        //add skill to menu
        PlayerStatus.UpdateLevelProgress(3, 4);
        TutorialPanel.SetActive(true);
        TutorialPanel.GetComponent<TutorialManager>().pause = true;
        TutorialPanel.GetComponent<TutorialManager>().SetText("Alright! Let's try out the skill! I'm giving you a new skill to activate, and here's some enemies, good luck!");
        sm.UpgradeSkill("Meteor");
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy();
            enemycount++;
        }
    }

    public void PauseGame()
    {
        // Toggle game pause status
        pauseGame = !pauseGame;
        PausePanel.GetComponent<PauseMenu>().showStatus(kills, coins);
        PausePanel.SetActive(pauseGame);
    }

}
