using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] private float dmg;
    [SerializeField] private float dealDamageTiming; 
    public float delay = 2.0f; // Time in seconds before destroying the object

    private bool damaged;

    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {

            Invoke(nameof(DamagePlayer), dealDamageTiming);
        }
    }

    void DamagePlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        player.GetComponent<player>().Damage(dmg);
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
