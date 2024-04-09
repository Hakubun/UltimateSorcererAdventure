using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject Bulletprefab;
    [SerializeField] private GameObject Boss;
    [SerializeField] CactusMelee CactusMelee;
    [SerializeField] private Animator Anim;
    public Transform firePoint;
    private GameObject nearestEnemy = null;

    [Header("Attributes")]

    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    //public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //repeat looking for enemy (function, starttime, rate)
        InvokeRepeating("UpdateTarget", 0f, 1f);
        firePoint = this.transform;
        Boss = null;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (fireCountdown <= 0f && target != null)
        {
            //shoot();
            Anim.SetTrigger("attack");
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

    }

    private void FixedUpdate() {
        Boss = GameObject.FindWithTag("Boss");
        if ( Boss != null){
            
            float distanceToBoss = Vector3.Distance (transform.position, Boss.transform.position);
            
            if (distanceToBoss <= range){
                target = Boss.transform;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

   void UpdateTarget ()
   {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Cactus");
        float shortestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance && enemy.GetComponent<enemy>().getHP() > 0)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            foreach (GameObject CactusMelee in enemies2)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, CactusMelee.transform.position);

                if (distanceToEnemy < shortestDistance && CactusMelee.GetComponent<CactusMelee>().getHP() > 0)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = CactusMelee;
                    break;
                }
            }
       

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        } 
        else 
        {
            target = null;
        }
   }



   public void IncreaseRange(){
        range += 1f;
   }
}
