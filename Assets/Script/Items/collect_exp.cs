using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collect_exp : MonoBehaviour
{
    [SerializeField] private GameObject expContainer;
    [SerializeField] private Vector3 _rotation = new Vector3(0f, 100f, 0f);
    [SerializeField] AudioSource ItemCollectSound;
    //[SerializeField] GameObject ItemCollectSound;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(ItemCollectSound.clip, transform.position);
            //ItemCollectSound.GetComponent<AudioSource>().Play();
            expContainer = GameObject.FindWithTag("EXPContainer");

            foreach (Transform exp in expContainer.transform)
            {
                exp.GetComponent<xpBehavior>().collect();
            }

            Destroy(gameObject);
        }
    }
}
