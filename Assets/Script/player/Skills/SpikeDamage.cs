using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        // Handle trigger collision event if needed.
        if (other != null)
        {
           // Debug.Log("IceSpike triggered with " + other.name); // Log the name of the colliding object.


            //Debug.Log("IceSpike triggered with " + other.name);
            // You can put your custom logic here when the IceSpike triggers with another collider.
            // If the collision is with an object tagged as "Enemy," apply damage to the enemy.
            if (other.CompareTag("Enemy"))
            {
                // Debug.Log("COLLIDES enemy");
                // Apply damage to the enemy.
                enemy EnemyType = other.GetComponent<enemy>();
                if (EnemyType != null)
                {
                    EnemyType.Damage(damage); // Adjust the damage as needed.
                }
            }
        }
        else
        {
            Debug.LogWarning("OnTriggerEnter: Collided with null object.");
        }
    }

}
