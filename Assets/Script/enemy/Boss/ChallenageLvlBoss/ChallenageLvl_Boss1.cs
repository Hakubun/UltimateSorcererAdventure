using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class ChallenageLvl_Boss1 : enemy
{
    [Header("Boss Script Setup: Drag In Required")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MinionsPrefab;

    private GameObject _hpbar;
    private GameObject HPbarContainer;


    private float distanceToPlayer;

    [Header("Boss SetUp: Fill in numbers")]
    [SerializeField] private float LowHealth;

    [SerializeField] private float shootTimer = 0f;
    [SerializeField] private float timeBetweenShots = 8f;

    void Start()
    {
        base.Start();

        HPbarContainer = GameObject.Find("BossHPContainer");

        _hpbar = Instantiate(HPBar);
        _hpbar.transform.parent = HPbarContainer.transform;
        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        shootTimer += Time.deltaTime;

        if (shootTimer >= timeBetweenShots && Vector3.Distance(transform.position, player.transform.position) >= 10)
        {
            Anim.SetTrigger("shoot");
            shootTimer = 0f; // Reset the timer
        }

    }

    public void launchBullet()
    {
        GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint.transform.position, firePoint.transform.rotation);

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
         
            //After boss is dead delete self game object.
            Destroy(gameObject);

            _hpbar.SetActive(false);

            //Call the function in game manager to continue the game.
            base.gm.BossDefeated();
        }
        else if (base.HP <= LowHealth)
        {
            StartSpawningMinions();
        }

        //Condition:


    }

    private void SpawnMinions()
    {
        Anim.SetTrigger("spawn");

    }

    public void Spawn()
    {
        // Define the number of minions to spawn
        int numberOfMinions = Random.Range(5, 10); // Adjust the range as needed

        // Define the radius of the circular formation
        float radius = 10f;

        for (int i = 0; i < numberOfMinions; i++)
        {
            // Calculate a random angle for minion placement
            float angle = Random.Range(0f, 180f);
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
}
