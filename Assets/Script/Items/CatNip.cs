using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNip : MonoBehaviour
{
    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);

    private void FixedUpdate()
    {
        // Rotate the item
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player playercomp = other.GetComponent<player>();
            if (playercomp != null)
            {
                playercomp.Boost(speedBoostDuration);
            }
            // Optionally, play a sound or particle effect when collected.
            Destroy(gameObject);
        }
    }



}
