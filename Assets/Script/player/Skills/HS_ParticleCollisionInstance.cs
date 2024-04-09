/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

// This script manages the behavior of particle collisions, triggering effects and applying damage.
public class HS_ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision; // Array of effects to be instantiated on particle collision.
    public float DestroyTimeDelay = 5;  // Time delay before destroying the instantiated effects.
    public bool UseWorldSpacePosition;  // Option to use world space position for instantiated effects.
    public float Offset = 0; // Offset for the position of instantiated effects.
    public Vector3 rotationOffset = new Vector3(0, 0, 0); // Rotation offset for instantiated effects.
    public bool useOnlyRotationOffset = true; // Option to use only rotation offset without additional rotation.
    public bool UseFirePointRotation; // Option to use the rotation of the fire point.
    public bool DestroyMainEffect = false; // Option to destroy the main effect after collision
    private ParticleSystem part;   // Particle system component reference.
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>(); // List to store collision events.
    private ParticleSystem ps; // Reference to the Particle System.
    public float Damage;  // Damage value to be applied on collision.
    private GameObject _Target;

    void Start()
    {
        // Get the Particle System component.
        part = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (_Target != null)
        {
            if (this.gameObject.name == "KniveRain(Clone)")
            {
                this.gameObject.transform.position = _Target.transform.position + new Vector3(0f, 10.0f, 0.0f);
            }
            else
            {
                this.gameObject.transform.position = _Target.transform.position;
            }
        }
    }

    // Called when a particle collides with another GameObject.
    void OnParticleCollision(GameObject other)
    {
        // Get the collision events.
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        // Iterate through each collision event.
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Instantiate effects on collision.
            foreach (var effect in EffectsOnCollision)
            {
                var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;

                // Set the parent of the effect based on UseWorldSpacePosition option.
                if (!UseWorldSpacePosition)
                {
                    instance.transform.parent = transform;
                }

                // Set the rotation of the effect based on different options.
                if (UseFirePointRotation)
                {
                    instance.transform.LookAt(transform.position);
                }
                else if (rotationOffset != Vector3.zero && useOnlyRotationOffset)
                {
                    instance.transform.rotation = Quaternion.Euler(rotationOffset);
                }
                else
                {
                    instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                }

                // Destroy the instantiated effect after a delay.
                Destroy(instance, DestroyTimeDelay);

                //Debug.Log("Collides inside");

                // If the collision is with an object tagged as "Enemy," apply damage to the enemy.
                if (other.CompareTag("Enemy"))
                {
                    // Debug.Log("COLLIDES enemy");
                    // Apply damage to the enemy.
                    enemy EnemyType = other.GetComponent<enemy>();
                    if (EnemyType != null)
                    {
                        EnemyType.Damage(Damage); // Adjust the damage as needed.
                    }
                }
            }
        }

        // Destroy the main effect if the DestroyMainEffect option is true.
        if (DestroyMainEffect == true)
        {
            Destroy(gameObject, DestroyTimeDelay + 0.5f);
        }
    }

    public void TargetSetUp(GameObject target)
    {
        _Target = target;
    }
}
