using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private GameObject[] itemsToSpawn;
    [SerializeField] private GameObject[] rareItems;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 10f; // Adjust the radius as needed
    [SerializeField] private float minWaitTime = 5f;
    [SerializeField] private float maxWaitTime = 10f;
    [SerializeField] private int maxItemCount = 10;
    private bool spawning;

    void Start()
    {
        if (player == null)
        {
            FindPlayer();
        }

        spawning = true;
        StartCoroutine(SpawnRoutine());

    }

    void Update()
    {
        // No need to update the position; it's handled in SpawnRoutine
    }

    void FindPlayer()
    {

        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player reference not set in Inspector!");
        }

    }

    void SpawnItem()
    {
        Vector3 posToSpawn = SpawnLocation(spawnRadius);

        GameObject itemToSpawn;

        // 30% chance to select from rareItems, 70% chance to select from itemsToSpawn
        if (Random.value < 0.1f && rareItems.Length > 0)
        {
            itemToSpawn = rareItems[Random.Range(0, rareItems.Length)];
        }
        else
        {
            itemToSpawn = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
        }

        GameObject item = Instantiate(itemToSpawn, posToSpawn + new Vector3 (0f, 0.5f, 0f), Quaternion.identity);
        item.transform.parent = itemContainer.transform;
    }

    IEnumerator SpawnRoutine()
    {
        while (spawning)
        {
            SpawnItem();
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 SpawnLocation(float radius)
    {

        // Generate a random direction vector within a unit sphere
        Vector3 randomDirection = Random.onUnitSphere;

        // Scale the random direction vector by the desired radius
        randomDirection *= radius;

        // Offset the position around the player's current position
        Vector3 spawnPosition = player.transform.position + randomDirection;

        // Sample a valid NavMesh position around the spawnPosition
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            // Fallback to the original position if sampling fails
            return spawnPosition;
        }
    }
}
