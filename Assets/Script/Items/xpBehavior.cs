using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xpBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation = new Vector3(0f, 100f, 0f);

    [SerializeField] private bool collected = false;
    [SerializeField] private float _speed;
    [SerializeField] AudioSource ItemCollectSound;

    private GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void FixedUpdate()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
        {
            //move to player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collect();
            AudioSource.PlayClipAtPoint(ItemCollectSound.clip, transform.position);
            // ItemCollectSound.GetComponent<AudioSource>().Play();
        }
    }

    public void collect()
    {
        collected = true;
    }
}
