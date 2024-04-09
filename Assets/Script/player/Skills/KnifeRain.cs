using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// This script controls the behavior of a knife rain ability in the game.
public class KnifeRain : MonoBehaviour
{
    // SerializedFields allow private variables to be visible in the Unity Editor for easy tweaking.
    [SerializeField] private GameObject KnifeRainPrefab; // Reference to the Knife prefab.
    [SerializeField] private GameObject _self; // Reference to the GameObject itself.
    [SerializeField] private SkillCooldownBar _SkillCooldownBar; // Reference to the skill cooldown bar.
    [SerializeField] private float KnifeRainRadius = 10.0f; // Radius that the skills position will randomly be around the player.
    private GameObject player; // Reference to the player GameObject.
    private GameObject nearestEnemy;

    // Gets the Particle Collision script.
    private HS_ParticleCollisionInstance particleInstance;

    [SerializeField] private bool Initialized; // Flag to track whether the ability has been initialized.

    private void Awake()
    {
        // Find the player GameObject with the "Player" tag and set the initial position.
        player = GameObject.FindWithTag("Player");
        transform.position = player.transform.position;
        _self = this.gameObject; // Set the reference to the current GameObject.
        Initialized = false; // Set the initialization flag to false.
    }

    void Start()
    {
        // Get the Particle Collision script component.
        particleInstance = GetComponent<HS_ParticleCollisionInstance>();
    }

    private void Update()
    {

    }

    public void ActivateSkill()
    {
        // Check for player input to trigger the Knife Rain.
        if (SkillManager.instance.CanUseSkill("KnifeRain"))
        {
            //Find enemies with the "Enemy" tag
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;


            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance && enemy.GetComponent<enemy>().getHP() > 0)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            if (enemies.Length > 0)
            {
                // Choose a random enemy from the array


                // Spawn the lightning strike at the position of the random enemy
                CreateKnifeRain(nearestEnemy.transform.position + new Vector3(0, 20.0f, 0));
            }
            else if (enemies.Length == 0)
            {
                // Create a lightning strike at a random point by getting the radius and the players position.
                Vector3 strikePosition = GetRandomPointInRadius(player.transform.position, KnifeRainRadius); // This allows the skill to spawn around the player within a certain radius.
                CreateKnifeRain(strikePosition);
            }
            // Trigger the cooldown through the SkillManager
            SkillManager.instance.TriggerCooldown("KnifeRain");
        }
    }

    // Function to get a random point within a specified radius around a given center
    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        // Generate random angles for spherical coordinates
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomHeight = Random.Range(-1f, 1f);

        // Convert spherical coordinates to Cartesian coordinates
        float x = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Cos(randomAngle);
        float z = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Sin(randomAngle);
        //float y = randomHeight * radius;

        // Apply offsets based on the center of the sphere
        Vector3 randomPoint = new Vector3(x, 10.0f, z) + center;

        return randomPoint;
    }

    // Method to create a Knife Rain at a specified position.
    private void CreateKnifeRain(Vector3 position)
    {
        // Instantiate the Knife Rain prefab at the specified position with rotation to face the ground.
        GameObject KnifeRa = Instantiate(KnifeRainPrefab, position, Quaternion.Euler(90f, 0f, 0f));
        KnifeRa.GetComponent<HS_ParticleCollisionInstance>().TargetSetUp(nearestEnemy);
        // Destroy the Knife Rain after 4 seconds.
        Destroy(KnifeRa, 4.0f);
    }

    // Method to upgrade the damage and cooldown of the knife rain ability.
    public void UpgradeDmg()
    {
        if (!Initialized)
        {
            _self.SetActive(true); // Activate the GameObject.
            Initialized = true; // Set the initialization flag to true.
        }
        else
        {

        }
    }
}
