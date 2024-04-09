using System.Collections;
using UnityEngine;
using DamageNumbersPro;

public class RangeEnemy : enemy
{
    [Header("Range Stats")]

    private float distanceToPlayer;

    private bool lowHealth;


    void Start()
    {

        // Call base.Start() after initializing desiredDistance
        base.Start();
    }

    public override void Update()
    {


        // Control animations based on movement
        Anim.SetFloat("move", navMeshAgent.velocity.magnitude);

        // Make the enemy face the player
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);



        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        navMeshAgent.SetDestination(player.transform.position);



        if (currentCooldown <= 0.0f)
        {
            // Perform the attack when the cooldown is over
            Anim.SetTrigger("shoot");

            // Reset the cooldown timer
            currentCooldown = attackCooldown;
        }


        // Update the cooldown timer
        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.fixedDeltaTime;
        }

    }

    public override void Damage(float _dmg)
    {
        HP -= _dmg;

        if (HP <= 0)
        {
            Anim.SetBool("death", true);

            if (!Dropped)
            {
                DropXP();
                gm.addKill(expWorth);
                DeathPopUp.Spawn(new Vector3(transform.position.x, 0.8f, transform.position.z));
                DamageNumber damageNumber = numberPrefab.Spawn(new Vector3(transform.position.x, 0.8f, transform.position.z), _dmg);
            }

        }
        else
        {
            DamageNumber damageNumber = numberPrefab.Spawn(new Vector3(transform.position.x, 0.8f, transform.position.z), _dmg);


        }
    }

    #region RangeAttack

    public void LaunchBullet()
    {
        GameObject AttackGO = Instantiate(AttackPrefab, AttackPoint.position, AttackPoint.rotation);
        AttackGO.GetComponent<rangeEnemyBullet>().setup(player, attackPwd);
    }

    #endregion

}
