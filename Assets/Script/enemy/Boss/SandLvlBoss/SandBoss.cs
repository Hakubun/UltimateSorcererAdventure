using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class SandBoss : enemy
{
    [Header("Drag In Required")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject HPBar;

    private GameObject _hpbar;
    private GameObject HPbarContainer;



    [Header("Charging Skill")]
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeCooldown = 5f;
    private bool isCharging = false;
    private float lastChargeTime;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
        HPbarContainer = GameObject.Find("BossHPContainer");
        _hpbar = Instantiate(HPBar);
        _hpbar.transform.parent = HPbarContainer.transform;

        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
    }

    // Update is called once per frame
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


        // Check if it's time to perform an action
        if (Time.time - lastChargeTime >= chargeCooldown)
        {
            float randomValue = Random.value;

            // Adjust the probabilities based on your preference
            if (randomValue < 0.2f)
            {
                StartCoroutine(ChargeAtPlayer());
                base.Anim.SetBool("charge", isCharging);
            }
            else
            {
                base.Anim.SetTrigger("shoot");
            }

            lastChargeTime = Time.time;
        }
    }

    IEnumerator ChargeAtPlayer()
    {
        if (!isCharging)
        {
            isCharging = true;

            // Stop the NavMeshAgent while charging
            navMeshAgent.isStopped = true;

            // Play charging animation or any visual/audio cues

            yield return new WaitForSeconds(2.0f); // Adjust this based on your needs

            // Calculate the direction to charge
            Vector3 chargeDirection = (player.transform.position - transform.position).normalized;

            // Move the boss towards the player at a higher speed
            Vector3 chargeDestination = player.transform.position;
            while (Vector3.Distance(transform.position, chargeDestination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, chargeDestination, chargeSpeed * Time.deltaTime);
                yield return null;
            }

            // Resume normal behavior
            navMeshAgent.isStopped = false;

            // Reset charging state
            isCharging = false;
            base.Anim.SetBool("charge", isCharging);

        }
    }

    public void launchBullet()
    {
        GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint.transform.position, firePoint.transform.rotation);
        bulletGO.GetComponent<BossBullet>().setup(player, attackPwd);
        //SandBossBullet _bullet = bulletGO.GetComponent<SandBossBullet>();
        // if (_bullet != null)
        // {
        //     _bullet.seek(player.transform);
        // }
        // else
        // {
        //     Debug.Log("null");
        // }
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
