using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider HP;
    //[SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    void Start() {
        // camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        // enemyHP = gameObject.GetComponent<Slider>();
        HP = this.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        // transform.rotation = camera.transform.rotation;
        // transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float hp, float maxhp){ 

        HP.value = hp / maxhp;

    }
}
