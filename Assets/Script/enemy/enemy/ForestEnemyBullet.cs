using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestEnemyBullet : MonoBehaviour
{
    public float Damage = 10.0f;
    private Vector3 target;

    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        ps.Emit(1);
        // Optional: Instantiate flash effect
        // if (flash != null)
        // {
        //     var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
        //     flashInstance.transform.forward = gameObject.transform.forward;
        //     var flashPs = flashInstance.GetComponent<ParticleSystem>();
        //     if (flashPs != null) { Destroy(flashInstance, flashPs.main.duration); }
        //     else { Destroy(flashInstance, flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration); }
        // }

        // Destroy the projectile after 5 seconds
        Destroy(gameObject, 2);
    }

    void FixedUpdate()
    {
        if (speed != 0)
        {
            //rb.velocity = transform.forward  * speed;
            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<player>().Damage(Damage);
        }

        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) { Destroy(hitInstance, hitPs.main.duration); }
            else { Destroy(hitInstance, hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration); }
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }

        Destroy(gameObject);
    }
}
