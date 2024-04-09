using UnityEngine;

public class enemybullet : MonoBehaviour
{
    private Vector3 target;
    public GameObject hiteffect;
    public float speed = 15f;
    public float dmg;
    public float Lifetime; // Lifetime of the bullet in seconds

    [SerializeField] private HS_ParticleCollisionInstance particals;
    private ParticleSystem ps;

    // Update is called once per frame
    private void Start() {
        ps = GetComponent<ParticleSystem>();
        ps.Emit(100);
    }
    void Update()
    {
        // Destroy the bullet after its lifetime expires
        Lifetime -= Time.deltaTime;
        //Debug.Log(Lifetime);

        // Adjusted the condition to check if the bullet is close to the target position
        if (Lifetime <= 0)
        {
            destroyObject();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<player>().Damage(dmg);
            destroyObject();
        }
        else if (Lifetime == 0)
        {
            destroyObject();
        }
    }

    public void GoToPlayer()
    {
        //Logic to go to player

    }

    void destroyObject()
    {
        Destroy(gameObject);
    }


}
