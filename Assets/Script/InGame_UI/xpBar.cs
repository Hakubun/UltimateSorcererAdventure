using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xpBar : MonoBehaviour
{
    [SerializeField] private Slider playerXP;
    //[SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    //[SerializeField] private Vector3 offset;

    void Start() {
        //camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //enemyHP = gameObject.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        // transform.rotation = camera.transform.rotation;
        // transform.position = target.position + offset;
    }

    public void UpdateXPbar(float xp, float maxXP){ 

        playerXP.value = xp / maxXP;

    }
}
