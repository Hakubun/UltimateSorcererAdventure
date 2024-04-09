using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class enemy : MonoBehaviour
{
    [Header("Drag In Required")]
    public NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component
    //public Rigidbody rb;
    public DamageNumber numberPrefab;
    public DamageNumber DeathPopUp;

    [SerializeField] protected Animator Anim; // Animator component for animations
    public GameObject AttackPrefab;
    public Transform AttackPoint;
    public AudioSource attackSound;
    public GameObject[] exps; // Array of XP prefabs

    [Header("Auto Setup, Debug purpose")]
    public GameObject player; // Reference to the player GameObject
    public GameManager gm; // Reference to the game manager

    [SerializeField] private enemyspawner spawner; // Reference to the enemy spawner

    [SerializeField] private GameObject EXP_Container; // Container for managing XP objects



    //Protected Variables to allow Child scripts to derived from Parent script.



    [Header("Status")]
    public int expWorth;
    public float AttackRange;
    private float OriginSpeed = 4; // Original speed of the enemy
    public float speed = 4; // Current speed of the enemy
    public float MaxHP; // Maximum HP of the enemy
    public float HP; // Current HP of the enemy
    public float attackPwd; // Attack power of the enemy
    public bool Dropped = false; // Flag to track if XP has been dropped
    public bool dead = false;

    // Temporary variables
    private bool hasAddedKill = false; // Flag to track if the kill has already been added

    //Position variables 
    private bool isPoisoned = false;
    private float poisonRate;
    private float poisonDuration = 3.0f; // Adjust the duration as needed
    private WaitForSeconds poisonTick = new WaitForSeconds(1f); // Adjust the tick interval as needed

    //Check the Xp container to see if any XP has been picked up.
    private float checkXPTimer = 0f; // Timer to track when to call CheckAndDestroyXP
    private float checkXPInterval = 60f; // Interval to call CheckAndDestroyXP (1 minute)


    // Start is called before the first frame update
    public virtual void Start()
    {
        // Find and initialize references
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        Anim = gameObject.GetComponentInChildren<Animator>(); // Get the Animator component
        gm = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get the game manager
        spawner = GameObject.Find("EnemySpawner").GetComponent<enemyspawner>(); // Get the enemy spawner
        //EXP_Container = GameObject.Find("EXP Container"); // Find the XP container
        navMeshAgent.speed = speed; // Initialize the enemy's speed
        AttackRange = navMeshAgent.stoppingDistance;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // If the enemy is not dead, update its behavior
        if (HP > 0)
        {
            // Set the destination of the NavMeshAgent to the player's position
            navMeshAgent.SetDestination(player.transform.position);

            // Control animations based on movement
            Anim.SetFloat("move", navMeshAgent.velocity.magnitude);

            // Make the enemy face the player
            Vector3 lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);

            
        }



    }

    public float attackCooldown = 2.0f; // Cooldown duration in seconds
    public float currentCooldown = 0.0f; // Time left until the next attack can occur

    public virtual void FixedUpdate()
    {

        navMeshAgent.speed = speed; // Update the enemy's speed
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (navMeshAgent.velocity.magnitude == 0 && distanceToPlayer <= AttackRange)
        {
            //Debug.Log("Attack!");
            if (currentCooldown <= 0.0f)
            {
                // Perform the attack when the cooldown is over
                Anim.SetTrigger("attack");

                //attackSound.Play();

                // Reset the cooldown timer
                currentCooldown = attackCooldown;
            }
        }

        // Update the cooldown timer
        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.fixedDeltaTime;
        }

    }

    public float getHP()
    {
        return HP;
    }

    public float getMaxHP()
    {
        return MaxHP;
    }
    public float getAttackPWD()
    {
        return attackPwd; // pass over the enemy's damage
    }

    // Function to apply damage to the enemy
    public virtual void Damage(float _dmg)
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
                gm.addKill(1);
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
    public virtual void DropXP()
    {
        GameObject exp = exps[Random.Range(0, exps.Length)]; // Choose a random XP prefab
        exp.GetComponent<xpDrop>().SetUpXP(expWorth);
        GameObject xp = Instantiate(exp, new Vector3(transform.position.x, 0.5f, transform.position.z), transform.rotation); // Spawn the XP
        //xp.transform.parent = EXP_Container.transform; // Set the XP object as a child of the XP container
        Dropped = true;
    }




    // Handle collisions with the player (assumes player has a "player" tag)
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Attack by " + this.gameObject);
            other.gameObject.GetComponent<player>().Damage(1f);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Attack by " + this.gameObject);
            other.gameObject.GetComponent<player>().Damage(1f);
            // Debug.Log("Dealing Damage");
        }
    }

    public void Attack()
    {
        GameObject AttackGO = Instantiate(AttackPrefab, AttackPoint.position, Quaternion.identity);
        AttackGO.GetComponent<EnemyMeleeAttackLogic>().SetupDMG(attackPwd);
        attackSound.Play();
    }

    public void DestroyObject()
    {
        // Destroy the game object
        Destroy(gameObject);
        //Debug.Log(this.gameObject.transform.parent.childCount);
    }

    public void Expload()
    {
        GameObject AttackGO = Instantiate(AttackPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }

    public void ResumeMovement()
    {
        navMeshAgent.isStopped = false;
    }

    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
    }

}
