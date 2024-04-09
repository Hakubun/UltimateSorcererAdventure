using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class PracticeLvlBoss : enemy
{
    [Header("Drag In Required")]
    private GameObject _hpbar;
    private GameObject HPbarContainer;
    [SerializeField] private GameObject HPBar;


    // Start is called before the first frame update
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

    public void HPupdate()
    {

        _hpbar.GetComponent<BossHpBar>().UpdateHP(base.HP, base.MaxHP);
    }


}
