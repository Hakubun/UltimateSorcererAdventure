using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallenageLvlBoss3Arrow : MonoBehaviour
{
    [SerializeField] private float dmg;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 RotationRate;
    [SerializeField] private float DestroyTime;
    [SerializeField] private bool followPlayer;
    [SerializeField] private GameObject hitEffectPrefab;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestorySelf", DestroyTime);
        //change to invoke function?
    }

    // Update is called once per frame
    void Update()
    {



        float distanceThisFrame = speed * Time.deltaTime;
        transform.Rotate(RotationRate);

        // if (dir.magnitude <= distanceThisFrame)
        // {
        //     //Destroy(gameObject);
        //     //GameObject effectInst = (GameObject)Instantiate(hiteffect, transform.position, transform.rotation);
        // }
        if (followPlayer)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);

            // Adjust rotation to face the movement direction
            transform.rotation = Quaternion.LookRotation(dir);

        }
        else
        {

            Vector3 dir = transform.forward;
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<player>().Damage(dmg);
            DestorySelf();
            //GameObject effectInst = (GameObject)Instantiate(hiteffect, transform.position, transform.rotation);



        }
        //GameObject effectInst = (GameObject)Instantiate(hiteffect, transform.position, transform.rotation);



    }

    public void setup(GameObject _target, float damage)
    {
        dmg = damage;
        target = _target;
    }

    private void DestorySelf()
    {
        Destroy(gameObject);
        GameObject hiteffect = (GameObject)Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        Destroy(hiteffect, 1f);
    }

    //

}
