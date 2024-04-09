using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionLogic : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Attack by " + this.gameObject);
            other.gameObject.GetComponent<player>().Damage(1f);
            
        }
    }
}
