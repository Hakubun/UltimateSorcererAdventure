using System.Collections;
using System.Linq; // Add this using directive
using UnityEngine;
using UnityEngine.AI;

public class enemyspawner : MonoBehaviour
{
    [SerializeField] private GameObject[] MeleePrefabs;
    [SerializeField] private GameObject[] RangePrefabs;
    [SerializeField] private GameObject[] BossPrefabsChallenge; // Array of boss prefabs


    [SerializeField] public GameObject bossPrefab;


    [SerializeField] public GameObject enemyContainer;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 60f;
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private int meleeToRangeRatio = 5; // Adjust the ratio as needed

    public bool spawning = true;
    private NavMeshHit hit;
    private Transform enemyContainerTransform;
    private int meleeCounter = 0;
    private int rangeCounter = 0;
    private int currentEnemyCount;

    string currentLevel;


    private bool SpawnOnce = true;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        enemyContainerTransform = enemyContainer.transform;
        //StartCoroutine(SpawnRoutine());
    }

    public int getEnemyCount()
    {
        return currentEnemyCount;
    }

    private void SpawnEnemy(GameObject[] enemyPrefabArray)
    {

        // Get a random position on the NavMesh for spawning an enemy
        Vector3 spawnPosition = RandomNavMeshPoint();

        if (spawnPosition != Vector3.zero)
        {
            // Check for collisions at the spawn position, ignoring colliders on the "Ground" layer
            int groundLayer = LayerMask.NameToLayer("Ground");
            int layerMask = ~(1 << groundLayer); // Invert the ground layer mask

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 2f, layerMask);

            if (colliders.Length == 0)
            {
                // No colliders found at the spawn position, so it's safe to spawn the enemy
                GameObject enemyPrefab = enemyPrefabArray[UnityEngine.Random.Range(0, enemyPrefabArray.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.transform.parent = enemyContainerTransform; // Place the enemy under the enemy container

                // Update counts
                currentEnemyCount++;
            }
        }
    }

    private Vector3 RandomNavMeshPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * spawnRadius;
        randomDirection += player.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }


    public void SpawnBossFromArray()
    {
        if (BossPrefabsChallenge.Length > 0)
        {
            // Select a random index from the array
            int randomIndex = Random.Range(0, BossPrefabsChallenge.Length);

            // Select the boss prefab at the random index
            GameObject bossPrefab = BossPrefabsChallenge[randomIndex];

            // Spawn the selected boss prefab
            Vector3 spawnPosition = RandomNavMeshPoint();
            if (spawnPosition != Vector3.zero)
            {
                // Instantiate the boss with a unique name
                GameObject spawnedBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
                // Remove the spawned boss reference from the array
                BossPrefabsChallenge = BossPrefabsChallenge.Where((boss, index) => index != randomIndex).ToArray();

                if (enemyContainer != null && currentLevel != "Level_ChallengeMode")
                {
                    ClearExistingEnemies();
                    spawning = false;
                }
            }
        }
    }

    public void SpawnBoss()
    {
        Vector3 spawnPosition = RandomNavMeshPoint();

        if (spawnPosition != Vector3.zero)
        {
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            spawning = false;
            ClearExistingEnemies();
        }
    }

    private void ClearExistingEnemies()
    {
        for (int i = enemyContainerTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyContainerTransform.GetChild(i).gameObject);
        }
    }

    public int SpawnWave(int amount, int rareEnemyAmount)
    {
        currentEnemyCount = 0;
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy(MeleePrefabs);

        }
        for (int j = 0; j < rareEnemyAmount; j++)
        {
            SpawnEnemy(RangePrefabs);

        }

        return currentEnemyCount;
    }
}