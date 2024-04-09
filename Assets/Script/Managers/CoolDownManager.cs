using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{
    [Header("Cooldown Bools")]
    [SerializeField] public bool isLightningCooldown = false;
    [SerializeField] public bool islaserCooldown = false;
    [SerializeField] public bool isMeteorCooldown = false;
    [SerializeField] public bool isKnifeCooldown = false;


    [Header("Cooldown Timers")]
    [SerializeField] private float LightningcooldownTimer = 0.0f;
    [SerializeField] private float LasercooldownTimer = 0.0f;
    [SerializeField] private float MeteorcooldownTimer = 0.0f;



    // Update is called once per frame
    void Update()
    {
        if (isLightningCooldown)
        {
            LightningcooldownTimer -= Time.deltaTime;
            if (LightningcooldownTimer <= 0)
            {
                isLightningCooldown = false;
                // We can trigger some event here to indicate that the cooldown has completed.
            }
        }
        if (islaserCooldown)
        {
            LasercooldownTimer -= Time.deltaTime;
            if (LasercooldownTimer <= 0)
            {
                islaserCooldown = false;
                // We can trigger some event here to indicate that the cooldown has completed.
            }
        }
        if (isMeteorCooldown)
        {
            MeteorcooldownTimer -= Time.deltaTime;
            if (MeteorcooldownTimer <= 0)
            {
                isMeteorCooldown = false;
                // We can trigger some event here to indicate that the cooldown has completed.
            }
        }
    }

    //Below here is where we do the funcitons for each ability cooldown.
    public void StartLightningCooldown(float cooldownTime)
    {
        isLightningCooldown = true;
        LightningcooldownTimer = cooldownTime;
    }

    //Get rid of tomorrow
    public void StartLaserCooldown(float cooldownTime)
    {
        islaserCooldown = true;
        LasercooldownTimer = cooldownTime;
    }

    public void StartMeteorCooldown(float cooldownTime)
    {
        isMeteorCooldown = true;
        MeteorcooldownTimer = cooldownTime;
    }
}
