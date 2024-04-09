using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : Boss
{
    [Header("Range Stats")]
    //By changing this variable. The bullets speed will either increase or decrease.
    [SerializeField] private float BulletSpeed;

    //Changing this variable will do nothing, but without this variable, the boss will not work.
    //This will allow the boss the fire as many bullets that we set "ProjectilePerBurst" to. 
    //If we set "ProjectilePerBurst" to 100 it will fire 100 bullets.
    [SerializeField] private int BurstCount; 
   
    //How many bullets the boss will fire.
    [SerializeField] private int ProjectilePerBurst;
   
    //This will affect the spread of bullets.
    [SerializeField][Range(0, 359)] private float AngleSpread;
   
    //The bullets starting distance from the boss.
    [SerializeField] private float StartingDistance = 1.0f; // Adjusted for 3D
   
    //This variable effects the time between each wave of bullets.
    [SerializeField] private float TimeBetweenBurst;
   
    //This bool variable will effect the bullet. The bullet will shoot directly at the player.
    [SerializeField] private bool Stagger;
   
    //This bool variable will effect the bullet. The bullet will shoot around the boss, in tandem with the AngleSpread variable.
    [SerializeField] private bool Oscillate;
    
    // Private Variables
    private bool IsShooting = false;
    private bool PhaseOne = false;
    private bool Notified = false;

    // Reference to the GameManager
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        // Find the GameManager in the scene
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    { 
       //Calls the parent update method.
       base.Update();
       //Checks to see if the boss is dead or not.
       if (!base.dead)
       {
            // Determine the boss phase based on its HP
            if (PhaseOne == false)
            {
                if (base.HP >= 1750)
                {
                    Stagger = true;
                    Oscillate = false;
                    ProjectilePerBurst = 20;
                    RangeAttack();
                }
                if (base.HP <= 1500)
                {
                    Stagger = false;
                    Oscillate = true;
                    ProjectilePerBurst = 40;
                    RangeAttack();
                }
                if (base.HP <= 1000)
                {
                    Stagger = true;
                    Oscillate = true;
                    ProjectilePerBurst = 60;
                    RangeAttack();
                }
                if (base.HP <= 500)
                {
                    Stagger = false;
                    Oscillate = true;
                    ProjectilePerBurst = 100;
                    BulletSpeed = 10;
                    RangeAttack();
                }
            }
       }
       else
       {
            // Notify the GameManager when the boss is defeated 
            if (!Notified)
            {
                NotifyGameManager();
            }
       }
    }

    // Notify GameManager about the boss defeat
    private void NotifyGameManager()
    {
        if (gameManager != null)
        {
            gameManager.BossDefeated();
            Notified = true;
        }
    }

#region RangeAttackBullets
    // Initiates the range attack if not already shooting
    public void RangeAttack()
    {
        if (!IsShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    // Coroutine to handle the boss's shooting behavior
    private IEnumerator ShootRoutine()
    {
        IsShooting = true;

        float StartAngle, CurrentAngle, AngleStep, EndAngle;
        float TimeBetweenProjectiles = Stagger ? TimeBetweenBurst / ProjectilePerBurst : 0.0f;

       for (int i = 0; i < BurstCount; i++)
       {
            // Determine the cone of influence based on boss behavior
            if (Oscillate)
            {
                TargetConeOfInfluence360(out StartAngle, out CurrentAngle, out AngleStep, out EndAngle);
            }
            else
            {
                TargetConeOfInfluence(out StartAngle, out CurrentAngle, out AngleStep, out EndAngle);
            }

            for (int j = 0; j < ProjectilePerBurst; j++)
            {
                // Spawn bullet and set its properties
                Vector3 pos = FindBulletSpawnPos(CurrentAngle);

                GameObject newBullet = Instantiate(base.Bulletprefab, pos, Quaternion.identity);

                // Set the bullet's forward direction
                Vector3 bulletDirection;

                if (Oscillate)
                {
                    // For oscillation, set the bullet direction based on the current angle
                    bulletDirection = new Vector3(Mathf.Cos(CurrentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(CurrentAngle * Mathf.Deg2Rad)).normalized;
                }
                else
                {
                    // For stagger or other cases, point the bullet towards the player
                    bulletDirection = (player.transform.position - pos).normalized;
                }

                enemybullet bulletScript = newBullet.GetComponent<enemybullet>();

                // Ensure there's a bullet script attached
                if (bulletScript != null)
                {
                    // Set the bullet's velocity to move it
                    Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
                    if (bulletRigidbody != null)
                    {
                        bulletRigidbody.velocity = bulletDirection * BulletSpeed;
                    }

                    // Set the bullet's lifetime
                    bulletScript.Lifetime = 5f;
                }
                yield return new WaitForSeconds(TimeBetweenProjectiles);
                CurrentAngle += AngleStep;
            }

            yield return new WaitForSeconds(TimeBetweenBurst);
       }
        IsShooting = false;
    }

    // Determine the cone of influence based on player position
    private void TargetConeOfInfluence(out float StartAngle, out float CurrentAngle, out float AngleStep, out float EndAngle)
    {
        Vector3 targetDirection = player.transform.position - transform.position;
        float TargetAngle = Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
        StartAngle = TargetAngle - AngleSpread / 2f;
        EndAngle = TargetAngle + AngleSpread / 2f;
        CurrentAngle = StartAngle;
        AngleStep = AngleSpread / ProjectilePerBurst;
    }

    // Determine the cone of influence in a 360-degree range
    private void TargetConeOfInfluence360(out float StartAngle, out float CurrentAngle, out float AngleStep, out float EndAngle)
    {
        StartAngle = 0;
        EndAngle = 360;
        CurrentAngle = StartAngle;
        AngleStep = 360 / ProjectilePerBurst;
    }

    // Find the spawn position of a bullet based on the current angle
    private Vector3 FindBulletSpawnPos(float CurrentAngle)
    {
        float x = transform.position.x + StartingDistance * Mathf.Cos(CurrentAngle * Mathf.Deg2Rad);
        float y = transform.position.y;
        float z = transform.position.z + StartingDistance * Mathf.Sin(CurrentAngle * Mathf.Deg2Rad);

        return new Vector3(x, y, z);
    }

#endregion

}