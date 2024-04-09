using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItemScript : MonoBehaviour
{
    [SerializeField] private int Amount;
    [SerializeField] private Vector3 _rotation = new Vector3(0f, 100f, 0f);
    [SerializeField] AudioSource ItemCollectSound;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(ItemCollectSound.clip, transform.position);
            GameManager.instance.addCoin(Amount);
            Destroy(gameObject);
        }

    }
}
