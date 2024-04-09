using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class tutorialEnemy : enemy
{
    [SerializeField] tutorialGM tutorialgm;

    // Start is called before the first frame update
    public override void Start()
    {
        // Find and initialize references
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        Anim = gameObject.GetComponentInChildren<Animator>(); // Get the Animator component
        tutorialgm = GameObject.Find("GameManager").GetComponent<tutorialGM>();

        navMeshAgent.speed = speed; // Initialize the enemy's speed
        AttackRange = navMeshAgent.stoppingDistance;
    }

    // Update is called once per frame





    // Function to apply damage to the enemy
    public override void Damage(float _dmg)
    {
        HP -= _dmg;

        if (HP <= 0)
        {
            Anim.SetBool("death", true); // Play the death animation
            // Stop the NavMeshAgent
            navMeshAgent.isStopped = true;
            // rb.constraints = RigidbodyConstraints.FreezeAll;
            if (!Dropped)
            {
                DropXP();
                tutorialgm.addKill(1);
                DeathPopUp.Spawn(new Vector3(transform.position.x, 0.5f, transform.position.z));
                DamageNumber damageNumber = numberPrefab.Spawn(new Vector3(transform.position.x, 0.5f, transform.position.z), _dmg);
            }
        }
        else
        {
            DamageNumber damageNumber = numberPrefab.Spawn(new Vector3(transform.position.x, 0.5f, transform.position.z), _dmg);
        }
    }

    // Function to drop XP when the enemy dies
    public override void DropXP()
    {
        Dropped = true;
    }




    // Handle collisions with the player (assumes player has a "player" tag)









}
