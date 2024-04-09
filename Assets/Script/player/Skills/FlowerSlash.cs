using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script controls the behavior of a lightning strike ability in the game.
public class FlowerSlash : MonoBehaviour
{
    // SerializedFields allow private variables to be visible in the Unity Editor for easy tweaking.
    [SerializeField] private GameObject FlowerSlashPrefab; // Reference to the lightning strike prefab.
    [SerializeField] private GameObject _self; // Reference to the GameObject itself.
    [SerializeField] private SkillCooldownBar _SkillCooldownBar; // Reference to the skill cooldown bar.
    private GameObject player; // Reference to the player GameObject.

    [SerializeField] private bool Initialized; // Flag to track whether the ability has been initialized.

    private void Awake()
    {
        // Find the player GameObject with the "Player" tag and set the initial position.
        player = GameObject.FindWithTag("Player");
        transform.position = player.transform.position;
        _self = this.gameObject; // Set the reference to the current GameObject.
        Initialized = false; // Set the initialization flag to false.
    }

    public void ActivateSkill()
    {
        // Check for player input to trigger the Ice Spike.
        if (SkillManager.instance.CanUseSkill("Flower"))
        {
            //Spawn on the player.
            // Spawn the Ice Spike on the player.
            CreateFlowerSlash(player.transform.position);

            // Trigger the cooldown through the SkillManager
            SkillManager.instance.TriggerCooldown("Flower");
        }
    }

    // Function to get a random point within a specified radius around a given center
    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        // Generate random angles for spherical coordinates
        float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        float randomHeight = UnityEngine.Random.Range(-1f, 1f);

        // Convert spherical coordinates to Cartesian coordinates
        float x = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Cos(randomAngle);
        float z = Mathf.Sqrt(1 - randomHeight * randomHeight) * radius * Mathf.Sin(randomAngle);
        float y = randomHeight * radius;

        // Apply offsets based on the center of the sphere
        Vector3 randomPoint = new Vector3(x, y, z) + center;

        return randomPoint;
    }

    // Method to create a Ice Spike at a specified position.
    private void CreateFlowerSlash(Vector3 position)
    {
        // Instantiate the Ice Spike prefab at the specified position.
        GameObject Flow = Instantiate(FlowerSlashPrefab, position, Quaternion.identity);

        // Destroy the Ice Spike after 3 seconds.
        Destroy(Flow, 2f);
    }
    // Method to upgrade the damage and cooldown of the Ice Spike ability.
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