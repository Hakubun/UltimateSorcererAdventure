using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectionRadius : MonoBehaviour
{
    //Global variables
    public CactusMelee cactus;

    // Awake is called before the first frame update
    void Awake()
    {
      
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //If player is in range, turn the bool on.
            cactus.Walk();
            cactus.playerInDetectionRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //If player isn't in range, run at them
            cactus.Run();
            cactus.playerInDetectionRange = false;
        }
    }

   

}
