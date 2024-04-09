using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warningMessage : MonoBehaviour
{
    [SerializeField] private float Duration;
    // Start is called before the first frame update
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Invoke("DisableSelf", Duration);
    }

    private void DisableSelf()
    {
        this.gameObject.SetActive(false);
    }
}
