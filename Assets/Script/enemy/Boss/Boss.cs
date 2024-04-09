using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;

public class Boss : enemy
{
    [Header("Range Stats")]
    [SerializeField] protected GameObject Bulletprefab;
    [SerializeField] private GameObject[] firePoint;
    [SerializeField] private GameObject[] endPoint;
    [SerializeField] private GameObject HPBar;
    private GameObject _hpbar;
    private GameObject HPbarContainer;


    [SerializeField] private Vector3 maxZone = new Vector3(10f, 0.1f, 10f);

    [Header("Status")]
    private float fireCountdown = 0f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Vector3 reachPoint;
    [SerializeField] private float shootTime;



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
    void Update()
    {
        base.Update();

        if (fireCountdown <= 0f)
        {
            Anim.SetTrigger("shoot");

            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;

        if (base.HP <= 0)
        {
            base.gm.win();
        }


    }

    public void shoot()
    {
        //this function is to be called by animator and you can adjust launching timing
        Invoke(nameof(launchBullet), shootTime);
    }

    void launchBullet()
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint[i].transform.position, transform.rotation);
            BossBullet _bullet = bulletGO.GetComponent<BossBullet>();

            if (_bullet != null)
            {
                //_bullet.seek(endPoint[i].transform);
            }
            else
            {
                Debug.Log("null");
            }
        }
    }

    public void HPupdate()
    {
        //healthBar.UpdateHealthBar(HP, MaxHP);
        Debug.Log("Boss took damage");
        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
    }

    public override void Damage(float _dmg)
    {
        DamageNumber damageNumber = base.numberPrefab.Spawn(transform.position, _dmg);
        HP -= _dmg;

        HPupdate();


    }
}
