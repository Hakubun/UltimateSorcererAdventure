using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallenageLvl_Boss2_Bullet : MonoBehaviour
{

    [SerializeField] private float dmg;
    [SerializeField] private float speed;

    private float destroyTime;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    private GameObject target;
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    void Start()
    {
        destroyTime = Random.Range(5f, 10f);
        // part = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        //Destroy(gameObject, destroyTime);
        Invoke("DestroyVFX", destroyTime);

    }

    void Update()
    {
        Vector3 dir = transform.forward;
        float distanceThisFrame = speed * Time.deltaTime;

        //Vector3 dir = target.transform.position - transform.position;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // Adjust rotation to face the movement direction
        







    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<player>().Damage(dmg);
            DestroyVFX();



        }
        else if (other.gameObject.CompareTag("Floor"))
        {
            DestroyVFX();
        }
        //GameObject effectInst = (GameObject)Instantiate(hiteffect, transform.position, transform.rotation);



    }

    public void setup(GameObject _target, float damage)
    {
        dmg = damage;
        target = _target;
    }

    private void DestroyVFX()
    {
        speed = 0;

        // Calculate explosion position
        Vector3 explosionPos = transform.position;

        // Instantiate the hit effect
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, explosionPos, Quaternion.identity);

            // Adjust the rotation of the hit effect based on conditions
            if (UseFirePointRotation)
            {
                hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
            }
            else if (rotationOffset != Vector3.zero)
            {
                hitInstance.transform.rotation = Quaternion.Euler(rotationOffset);
            }
            else
            {
                // You may want to set the rotation based on some logic if needed
            }

            // Get the ParticleSystem component from the hit prefab
            var hitPs = hitInstance.GetComponent<ParticleSystem>();

            // Destroy the hit prefab after the duration of its ParticleSystem
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                Destroy(hitInstance, hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
            }
        }

        // Additional code for explosion, if needed

        // Destroy the bullet object
        Destroy(gameObject);
    }




}
