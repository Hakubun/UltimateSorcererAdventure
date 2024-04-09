using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberCyclone : MonoBehaviour
{
    //Serialize Variables
    [SerializeField] private UnityEngine.AI.NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject Player;
    [SerializeField] private enemyHealthBar healthBar;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private int BurstCount;
    [SerializeField] private int ProjectilePerBurst;
    [SerializeField] [Range(0, 359)] private float AngleSpread;
    [SerializeField] private float StartingDistance = 1.0f; //Adjusted for 3d
    [SerializeField] private float TimeBetweenBurst;
    [SerializeField] private float RestTime = 1.0f;
    [SerializeField] private bool Stagger;
    [SerializeField] private bool Oscillate;

    //Private Variables
    private bool IsShooting = false;
    private bool IsMelee = false;
    private float distanceToPlayer;
    private float Radius = 1.0f;

    //Public Variables.
    private float MaxHP = 1000.0f;
    private float HP = 1000.0f;

    //Melee
    [SerializeField] private float range = 20f;
    [SerializeField] private float meleeDamage = 20.0f;
    [SerializeField] private float meleeCooldown = 2.0f;
   
    //This will fire before start
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        healthBar = GetComponentInChildren<enemyHealthBar>();
        healthBar.UpdateHealthBar(HP, MaxHP);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > 0)
        {
            navMeshAgent.SetDestination(Player.transform.position); // Move to player

            //Look at the player
            Vector3 lookPos = Player.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);
        }

        distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer <= range)
        {
            if (!IsMelee)
            {
                // Perform AOE melee attack here
                MeleeAttack();
            }
        }
        else if (distanceToPlayer >= range)
        {
            RangeAttack();
        }

        if (HP <= 0)
        {
            Destroy(gameObject, 1f);
            //GameObject.Find("Gamemanager").GetComponent<GameManager>().win();
        }
    }

    public void Damage(float _dmg)
    {
        HP -= _dmg;
        healthBar.UpdateHealthBar(HP, MaxHP);
    }


#region RangeAttack
    //Range attack of boss
    public void RangeAttack()
    {
        if (!IsShooting) 
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        IsShooting = true;

        float StartAngle, CurrentAngle, AngleStep, EndAngle;
        float TimeBetweenProjectiles = 0.0f;

        TargetConeOfInfluence(out StartAngle, out CurrentAngle, out AngleStep, out EndAngle); 

        if (Stagger) { TimeBetweenProjectiles = TimeBetweenBurst / ProjectilePerBurst; }

        for (int i = 0; i < BurstCount; i++) 
        {
            if (!Oscillate)
            {
                TargetConeOfInfluence(out StartAngle, out CurrentAngle, out AngleStep, out EndAngle);
            }
            if(Oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out StartAngle, out CurrentAngle, out AngleStep, out EndAngle);

            }
            else if (Oscillate)
            {
                CurrentAngle = EndAngle;
                EndAngle = StartAngle;
                StartAngle = CurrentAngle;
                AngleStep *= -1;
            }

            for (int j = 0; j < ProjectilePerBurst; j++)
            {
                Vector3 pos = FindBulletSpawnPos(CurrentAngle);

                GameObject newBullet = Instantiate(BulletPrefab, pos, Quaternion.identity);
                //newBullet.transform.position = newBullet.transform.position - transform.position;
                newBullet.transform.forward = (Player.transform.position - pos).normalized; // Adjusted for 3D


                if (newBullet.TryGetComponent(out enemybullet bullet))
                {
                    //bullet.UpdateBulletSpeed(BulletSpeed);
                }

                CurrentAngle += AngleStep;

                //Gets the bullet and targets the player
                enemybullet _bullet = newBullet.GetComponent<enemybullet>();
                if (_bullet != null)
                {
                    _bullet.Lifetime = 5f;
                }
                else
                {
                    Debug.Log("null");
                }

                if(Stagger ) { yield return new WaitForSeconds(TimeBetweenProjectiles); }
            }

            CurrentAngle = StartAngle;

            if (!Stagger) { yield return new WaitForSeconds(TimeBetweenBurst); }
        }

        yield return new WaitForSeconds(RestTime);
        IsShooting = false;
    }

    private void TargetConeOfInfluence(out float StartAngle, out float CurrentAngle, out float AngleStep, out float EndAngle)
    {
        Vector3 targetDirection = Player.transform.position - transform.position;
        float TargetAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
        StartAngle = TargetAngle;
        EndAngle = TargetAngle;
        CurrentAngle = TargetAngle;
        float HalfAngleSpread = 0f;
        AngleStep = 0;

        if (AngleSpread != 0)
        {
            AngleStep = AngleSpread / ProjectilePerBurst;
            HalfAngleSpread = AngleSpread / 2f;
            StartAngle = TargetAngle - HalfAngleSpread;
            EndAngle = TargetAngle + HalfAngleSpread;
            CurrentAngle = StartAngle;
        }
    }

    private Vector3 FindBulletSpawnPos(float CurrentAngle)
    {
        float x = transform.position.x + StartingDistance * Mathf.Cos(CurrentAngle * Mathf.Deg2Rad);
        float y = transform.position.y;
        float z = transform.position.z + StartingDistance * Mathf.Sin(CurrentAngle * Mathf.Deg2Rad);

        Vector3 Pos = new Vector3(x, y, z);

        return Pos;
    }
    #endregion

#region MeleeAttack

    private void OnDrawGizmos()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.transform.position) <= range)
        {
            Gizmos.color = Color.red;
            float maxAlpha = 0.5f;
            float alpha = Mathf.PingPong(Time.time, maxAlpha) / maxAlpha; // Fading effect
            Gizmos.DrawWireSphere(transform.position, range); // Adjust the range multiplier as needed
            Gizmos.color = new Color(1f, 0f, 0f, alpha); // Set the color with fading alpha
            Gizmos.DrawSphere(transform.position, range); // Adjust the range multiplier as needed
        }
    }

    private void MeleeAttack()
    {
        // Play melee attack animation or effects here

        // Damage the player (you can modify this logic based on your game)
        Player.GetComponent<player>().Damage(meleeDamage);

        // Set IsMelee to true to prevent continuous melee attacks
        IsMelee = true;

        // Wait for a cooldown or animation duration before allowing another melee attack
        StartCoroutine(MeleeCooldown());
    }

    IEnumerator MeleeCooldown()
    {
        // Adjust the cooldown duration as needed
        yield return new WaitForSeconds(meleeCooldown);
        IsMelee = false;
    }
    #endregion

}
