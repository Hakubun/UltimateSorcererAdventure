using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFood : MonoBehaviour
{
    [SerializeField] private float HealAmount;
    [SerializeField] private Vector3 _rotation = new Vector3(0f, 100f, 0f);
    [SerializeField] AudioSource ItemCollectSound;

    private void Start()
    {
        HealAmount = 10;
    }

    private void FixedUpdate()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // ItemCollectSound.GetComponent<AudioSource>().Play();
            AudioSource.PlayClipAtPoint(ItemCollectSound.clip, transform.position);
            other.GetComponent<player>().Heal(HealAmount);
            Destroy(gameObject);
        }

    }
}
