using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class ChallenageLvl_Boss3 : enemy
{
    [Header("Boss Script Setup: Drag In Required")]
    [SerializeField] protected GameObject smallBulletprefab;
    [SerializeField] protected GameObject largeBulletprefab;
    [SerializeField] private GameObject[] firePoint_s;
    [SerializeField] private GameObject firePoint_l;
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MinionsPrefab;

    private GameObject _hpbar;
    private GameObject HPbarContainer;

    private float distanceToPlayer;

    [Header("Boss SetUp: Fill in numbers")]
    [SerializeField] private float LowHealth;
    [SerializeField] private float shootRange;
    [SerializeField] private float backoffDistance;
    [SerializeField] private float shootTimer = 0f;
    [SerializeField] private float timeBetweenShots = 8f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        HPbarContainer = GameObject.Find("BossHPContainer");

        _hpbar = Instantiate(HPBar);
        _hpbar.transform.parent = HPbarContainer.transform;
        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);

        shootRange = navMeshAgent.stoppingDistance;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, shootRange);

    }

    // Update is called once per frame
    public override void Update()
    {
        // Make the enemy face the player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        navMeshAgent.SetDestination(player.transform.position);

        // Control animations based on movement
        Anim.SetFloat("move", navMeshAgent.velocity.magnitude);

        shootTimer += Time.deltaTime;
        if (distanceToPlayer >= shootRange)
        {


            // Shoot bullets every 5 seconds
            if (shootTimer >= timeBetweenShots)
            {
                Anim.SetTrigger("shoot");
                shootTimer = 0f; // Reset the timer
            }
        }
        else if (distanceToPlayer < shootRange)
        {
            if (shootTimer >= timeBetweenShots)
            {
                Anim.SetTrigger("launch");
                shootTimer = 0f; // Reset the timer
            }
            
        }
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

    public void HPupdate()
    {

        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
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

    public void SmallArrow(int pos)
    {
        GameObject bulletGO = (GameObject)Instantiate(smallBulletprefab, firePoint_s[pos].transform.position, firePoint_s[pos].transform.rotation);
        bulletGO.GetComponent<ChallenageLvlBoss3Arrow>().setup(player, attackPwd / 2);
    }
    public void LargeArrow() 
    {
        GameObject bulletGO = (GameObject)Instantiate(largeBulletprefab, firePoint_l.transform.position, firePoint_l.transform.rotation);
        bulletGO.GetComponent<ChallenageLvlBoss3Arrow>().setup(player, attackPwd);
        Debug.Log("LargeArrow!");
    }
}
