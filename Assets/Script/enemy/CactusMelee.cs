using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CactusMelee : MonoBehaviour
{
    //For local variables
    [SerializeField] public Animator CactusAnim;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject[] exps;

    //For global variables and Game objects
    public float MaxHP = 10f;
    public float HP = 10f;
    private float dmg = 10;
    public int xp = 5;
    public bool Dropped = false;
    public float speed = 4f;
    //temp var
    public bool onetime = false;

    public GameObject player;

    public Transform playerTransform;
    public bool playerInDetectionRange = true;

    [SerializeField] enemyHealthBar hpBar;
    [SerializeField] GameObject Bulletprefab;

    [Header("Cooldown to Attack")]
    [SerializeField] public bool AttackIsBlock;
    [SerializeField] public float AttackDelay = 0.3f;
   
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        CactusAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Gamemanager").GetComponent<GameManager>();
        hpBar = GetComponentInChildren<enemyHealthBar>();
        HP = MaxHP;
        navMeshAgent.speed = speed;
        Run();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > 0)
        {
            navMeshAgent.SetDestination(player.transform.position); // Move to player

            //Look at the player
            Vector3 lookPos = player.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);
        }

        if (HP <= 0)
        {
            navMeshAgent.isStopped = true;
            if (!onetime)
            {
                //GameObject.Find("Gamemanager").GetComponent<GameManager>().addKill();
                onetime = true;
            }
            if (Dropped == false)
            {
                CactusAnim.SetTrigger("Die");
                DropXP();
            }
            Destroy(gameObject, 1f);
        }
    }

    private void FixedUpdate()
    {
        navMeshAgent.speed = speed;
    }

    public void Run()
    {
        CactusAnim.SetTrigger("Run");
        //navMeshAgent.transform.LookAt(playerTransform); // Look at the player
        //navMeshAgent.SetDestination(player.transform.position); // Move to player
        speed = 12f;
    }

    public void Walk()
    {
        CactusAnim.SetTrigger("Walk");
        //navMeshAgent.transform.LookAt(playerTransform); // Look at the player
        //navMeshAgent.SetDestination(player.transform.position); // Move to player
        speed = 8f;
    }

    //Start of the attacking funcitonality
    public void Attack()
    {
        //Checking to see if the bool is false so it can skip this function.
        if (AttackIsBlock) return;


        CactusAnim.SetTrigger("Attack");
        player.GetComponent<player>().Damage(dmg);
       
        
        //Delays the attack so it's not spamming it.
        AttackIsBlock = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(AttackDelay);
        AttackIsBlock = false;
    }

    public void Damage(float _dmg)
    {
        HP -= _dmg;
    }
    
    public float getHP()
    {
        return HP;
    }

    public void DropXP()
    {
        //exp.GetComponent<xpDrop>().setXP(xps);
        GameObject exp = exps[Random.Range(0, exps.Length)];
        GameObject xp = Instantiate(exp, transform.position, transform.rotation);
        Dropped = true;
    }
}
