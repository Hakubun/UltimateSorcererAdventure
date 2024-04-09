using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class SimpleBoss : enemy
{
    [Header("Boss Setup: Drag In Required")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject HPBar;
    // Start is called before the first frame update

    [Header("Boss SetUp: Fill in numbers")]

    [SerializeField] private float shootTimer = 0f;
    [SerializeField] private float timeBetweenShots = 5f;

    #region Variables auto setup in Start Function
    private GameObject _hpbar;
    private GameObject HPbarContainer;
    #endregion
    void Start()
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

        // Update the shoot timer
        shootTimer += Time.deltaTime;

        // Shoot bullets every 5 seconds
        if (shootTimer >= timeBetweenShots && Vector3.Distance(transform.position, player.transform.position) >= 8)
        {
            Anim.SetTrigger("shoot");
            shootTimer = 0f; // Reset the timer
        }
        else if (shootTimer >= timeBetweenShots && Vector3.Distance(transform.position, player.transform.position) < 8)
        {
            Anim.SetTrigger("melee");
            shootTimer = 0f;
        }
    }

    public void launchBullet()
    {
        GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint.transform.position, firePoint.transform.rotation);
        bulletGO.GetComponent<BossBullet>().setup(player, attackPwd);
        //SandBossBullet _bullet = bulletGO.GetComponent<SandBossBullet>();  Quaternion.LookRotation(player.transform.position - transform.position)
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
}
