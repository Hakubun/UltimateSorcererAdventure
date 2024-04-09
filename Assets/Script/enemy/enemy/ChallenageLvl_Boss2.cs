using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class ChallenageLvl_Boss2 : enemy
{
    [Header("Boss Setup: Drag In Required")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MinionsPrefab;

    private GameObject _hpbar;
    private GameObject HPbarContainer;


    [Header("Range Stats")]
    //By changing this variable. The bullets speed will either increase or decrease.

    [SerializeField] private float AttackTimer;
    [SerializeField] private float shotDuration;
    [SerializeField] private float shotDurationTimer;

    [SerializeField] private float BulletSpeed;


    //This variable effects the time between each wave of bullets.
    [SerializeField] private float TimeBetweenAttack;


    [SerializeField] private int phase;
    [SerializeField] private bool isShooting;
    [SerializeField] private int meleeRange;
    [SerializeField] private int teleportRange;

    // Private Variables
    private bool Notified = false;

    // Reference to the GameManager
    private GameManager gameManager;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        HPbarContainer = GameObject.Find("BossHPContainer");
        
        _hpbar = Instantiate(HPBar);
        _hpbar.transform.parent = HPbarContainer.transform;
        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
        phase = 1;

        //RangeAttack();

    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, teleportRange);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make the enemy face the player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

        // Update the shoot timer

        if (Vector3.Distance(transform.position, player.transform.position) >= teleportRange && AttackTimer >= TimeBetweenAttack)
        {
            Anim.SetBool("move", true);
            AttackTimer = 0f;
        }
        else
        {
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= TimeBetweenAttack && Vector3.Distance(transform.position, player.transform.position) < teleportRange)
            {
                isShooting = true;
                switch (phase)
                {
                    case 1:

                        Anim.SetBool("shoot", true);
                        break;
                    case 2:
                        Anim.SetBool("shoot", true);
                        break;
                    case 3:
                        Anim.SetBool("spray", true);
                        break;
                    case 4:
                        Anim.SetBool("spray", true);
                        break;
                }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < meleeRange && AttackTimer >= TimeBetweenAttack)
            {
                Anim.SetTrigger("melee");
                AttackTimer = 0f;
            }
        }
        // Shoot bullets every 5 seconds

    }

    public override void FixedUpdate()
    {
        if (isShooting)
        {
            shotDurationTimer += Time.fixedDeltaTime;

            if (shotDurationTimer >= shotDuration)
            {

                switch (phase)
                {
                    case 1:
                        Anim.SetBool("shoot", false);
                        isShooting = false;
                        AttackTimer = 0f;// Reset the timer
                        break;
                    case 2:
                        Anim.SetBool("shoot", false);
                        isShooting = false;
                        AttackTimer = 0f;
                        break;
                    case 3:
                        Anim.SetBool("spray", false);
                        isShooting = false;
                        AttackTimer = 0f;
                        break;
                    case 4:
                        Anim.SetBool("spray", false);
                        isShooting = false;
                        AttackTimer = 0f;
                        
                        break;
                }
            }
        }
        else
        {
            shotDurationTimer = 0f;
        }



    }

    public void HPupdate()
    {

        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
    }

    public override void Damage(float _dmg)
    {
        DamageNumber damageNumber = base.numberPrefab.Spawn(transform.position, _dmg);
        base.HP -= _dmg;

        HPupdate();

        // Determine the boss phase based on its HP

        if (base.HP > 1500)
        {
            phase = 1;
        }
        if (base.HP <= 1500)
        {
            Anim.SetBool("shoot", false); //in case the enemy stuck in first phase shooting animation
            TimeBetweenAttack = 2f;
            phase = 2;

        }
        if (base.HP <= 1000)
        {
            TimeBetweenAttack = 4f;
            phase = 3;
        }
        if (base.HP <= 500)
        {
            TimeBetweenAttack = 2f;
            phase = 4;
            StartSpawningMinions();
        }


        if (base.HP <= 0)
        {
            base.Anim.SetBool("death", true);
         
            //After boss is dead delete self game object.
            Destroy(gameObject);

            _hpbar.SetActive(false);
            
            //Call the function in game manager to continue the game.
            base.gm.BossDefeated();

            //base.gm.win();
        }
    }


    public void LaunchBullet()
    {
        GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint.transform.position, firePoint.transform.rotation);
        bulletGO.GetComponent<ChallenageLvl_Boss2_Bullet>().setup(player, attackPwd);
    }

    private void SpawnMinions()
    {
        Anim.SetBool("spawn", true);

    }

    public void Spawn()
    {
        // Define the number of minions to spawn
        int numberOfMinions = Random.Range(2, 5); // Adjust the range as needed

        // Define the radius of the circular formation
        float radius = 10f;

        for (int i = 0; i < numberOfMinions; i++)
        {
            // Calculate a random angle for minion placement
            float angle = Random.Range(0f, 360f);
            // Calculate the position based on the angle and radius
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0f, angle, 0f) * (Vector3.forward * radius);

            // Instantiate a minion at the calculated position
            GameObject minionGO = Instantiate(MinionsPrefab, spawnPosition, Quaternion.identity);

            // Optional: You may want to set the minions' target or adjust other properties here
        }
    }

    private void StartSpawningMinions()
    {
        InvokeRepeating("SpawnMinions", 0f, 20f); // Spawn minions every 10 seconds
    }

    public void MoveNearPlayer()
    {

        if (player != null)
        {
            // Get a random point on the NavMesh within a radius of 10 units around the player
            Vector3 randomOffset = Random.insideUnitSphere * 10f;
            randomOffset.y = 0; // Ensure the teleportation is at the same height as the player (assuming a 2D plane)

            // Calculate the new position by adding the random offset to the player's position
            Vector3 newPosition = player.transform.position + randomOffset;

            // Check if the new position is on the NavMesh
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(newPosition, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                // Set the position to the valid NavMesh position
                transform.position = hit.position;
            }
            else
            {
                Debug.LogWarning("Could not find a valid position on the NavMesh.");
            }
        }
        else
        {
            Debug.LogWarning("Player is not assigned.");
        }

        Anim.SetBool("move", false);
    }

    public void doneSpawn()
    {
        Anim.SetBool("spawn", false);
    }
}
