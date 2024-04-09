using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class ForestBoss : enemy
{
    // Start is called before the first frame update
    [Header("Boss Setup: Drag In Required")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MinionsPrefab;



    [Header("Boss SetUp: Fill in numbers")]
    [SerializeField] private float LowHealth;

    [SerializeField] private float shootTimer = 0f;
    [SerializeField] private float timeBetweenShots = 5f;

    #region Variables auto setup in Start Function
    private GameObject _hpbar;
    private GameObject HPbarContainer;
    #endregion


    protected void Start()
    {
        base.Start();
        HPbarContainer = GameObject.Find("BossHPContainer");
        _hpbar = Instantiate(HPBar);
        _hpbar.transform.parent = HPbarContainer.transform;

        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);


    }

    public override void Update()
    {
        navMeshAgent.SetDestination(player.transform.position);

        // Control animations based on movement
        Anim.SetFloat("move", navMeshAgent.velocity.magnitude);

        // Make the enemy face the player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

        // Update the shoot timer
        shootTimer += Time.deltaTime;

        // Shoot bullets every 5 seconds
        if (shootTimer >= timeBetweenShots && Vector3.Distance(transform.position, player.transform.position) >= 8)
        {
            Anim.SetTrigger("shoot");
            shootTimer = 0f; // Reset the timer
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

        if (base.HP <= 0)
        {
            base.Anim.SetBool("death", true);
            base.gm.win();
        }
        else if (base.HP <= LowHealth)
        {
            StartSpawningMinions();
        }
    }

    private void SpawnMinions()
    {
        Anim.SetTrigger("spawn");

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

    public void launchBullets()
    {
        int numberOfBullets = Random.Range(1, 4); // Random number between 3 and 6 (inclusive)

        for (int i = 0; i < numberOfBullets; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-10f, 10f));

            // Add small random offsets to x and y positions
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);

            GameObject bulletGO = Instantiate(Bulletprefab, firePoint.transform.position + randomOffset, firePoint.transform.rotation * randomRotation);
            bulletGO.GetComponent<BossBullet>().setup(player, attackPwd);

            // Optionally, you can add additional customization for each bullet here
            // For example, changing the direction or speed of each bullet.
        }
    }
}
