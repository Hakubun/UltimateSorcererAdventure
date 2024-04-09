using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackLogic : MonoBehaviour
{
    private float _dmg;
    public float delay = 1.0f; // Time in seconds before destroying the object
    // Start is called before the first frame update
    public void SetupDMG(float dmg)
    {
        _dmg = dmg;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("deal melee damage");
            //Debug.Log("Attack by " + this.gameObject);
            other.gameObject.GetComponent<player>().Damage(_dmg);
            Destroy(this.gameObject);
        }
    }

    

    void Start()
    {
        // Invoke the DestroyObject method after the specified delay
        Invoke("DestroyObject", delay);
    }

    void DestroyObject()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}
