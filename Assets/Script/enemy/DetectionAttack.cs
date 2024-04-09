using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectionAttack : MonoBehaviour
{
     //Global variables
    public CactusMelee cactus;

    public bool playerInDetectionFight = false;
    public DateTime nextDamage;
    public float fightAfterTime;


    // Awake is called before the first frame update
    void Awake()
    {
        nextDamage = DateTime.Now;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (playerInDetectionFight == true)
        {
            FightInDetectionFight();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInDetectionFight = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInDetectionFight = false;
        }
        if (playerInDetectionFight == false)
        {
            cactus.CactusAnim.SetTrigger("Walk");
        }
    }

    public void FightInDetectionFight()
    {
        if (nextDamage <= DateTime.Now)
        {
            cactus.Attack();
            nextDamage = DateTime.Now.AddSeconds(System.Convert.ToDouble(fightAfterTime));
        }
    }

}
