using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrapSkillLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            NavMeshAgent enemyNavMeshAgent = other.GetComponent<NavMeshAgent>();
            if (enemyNavMeshAgent != null)
            {
                enemyNavMeshAgent.Stop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            NavMeshAgent enemyNavMeshAgent = other.GetComponent<NavMeshAgent>();
            if (enemyNavMeshAgent != null)
            {
                enemyNavMeshAgent.isStopped = false;
            }
        }
    }
}
