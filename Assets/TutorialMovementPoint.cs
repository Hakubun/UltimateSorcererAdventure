using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovementPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            tutorialGM.Instance.MovementComplete();
        }
    }
}
