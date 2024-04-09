using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalSpikesSkill : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy"))
        {
            //DealDamage
        }
    }
}
