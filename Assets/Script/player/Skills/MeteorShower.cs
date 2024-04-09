using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script controls the behavior of a meteor shower ability in the game.
public class MeteorShower : MonoBehaviour
{
    // SerializedFields allow private variables to be visible in the Unity Editor for easy tweaking.
    [SerializeField] private GameObject MeteorShowerPrefab; // Reference to the Meteor strike prefab
    [SerializeField] private GameObject _self; // Reference to the GameObject itself.
    [SerializeField] private SkillCooldownBar _SkillCooldownBar; // Reference to the skill cooldown bar.
    [SerializeField] private float meteorStrikeRadius = 10.0f; // Radius that the skills position will randomly be around the player.
    private GameObject player;  // Reference to the player GameObject.
    private GameObject nearestEnemy;
    //Gets the Particle collision script
    private HS_ParticleCollisionInstance particleInstance;
    [SerializeField] private bool Initialized;  // Flag to track whether the ability has been initialized.

    private void Awake()
    {
        // Find the player GameObject with the "Player" tag and set the initial position.
        player = GameObject.FindWithTag("Player");
        transform.position = player.transform.position;
        _self = this.gameObject;  // Set the reference to the current GameObject.
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
        // Check if the skill is ready through the SkillManager
        if (SkillManager.instance.CanUseSkill("Meteor"))
        {
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
                CreateMeteorStrike(nearestEnemy.transform.position);
            }
            else if (enemies.Length == 0)
            {
                Vector3 strikePosition = GetRandomPointInRadius(player.transform.position, 10.0f);
                CreateMeteorStrike(strikePosition);
            }

            // Trigger the cooldown through the SkillManager
            SkillManager.instance.TriggerCooldown("Meteor");
        }
    }

    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        // Generate random angles for spherical coordinates
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomHeight = Random.Range(-1f, 1f);

        // Convert spherical coordinates to Cartesian coordinates
        float x = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Cos(randomAngle);
        float z = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Sin(randomAngle);
        float y = randomHeight * radius;

        // Apply offsets based on the center of the sphere
        Vector3 randomPoint = new Vector3(x, y, z) + center;

        return randomPoint;
    }

    // Method to create a Meteor strike at a specified position.
    private void CreateMeteorStrike(Vector3 position)
    {
        // Instantiate the Meteor strike prefab at the specified position
        GameObject MeteorStrike = Instantiate(MeteorShowerPrefab, position, Quaternion.identity);
        MeteorStrike.GetComponent<HS_ParticleCollisionInstance>().TargetSetUp(nearestEnemy);
        // Destroy the Meteor strike after 4 seconds
        Destroy(MeteorStrike, 4.0f);
    }

    // Method to upgrade the damage and cooldown of the meteor shower ability.
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
