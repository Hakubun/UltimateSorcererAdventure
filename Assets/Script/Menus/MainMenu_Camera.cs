using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Camera : MonoBehaviour
{
    private Vector3 targetPos;
    [SerializeField] private Vector3 defaultPos;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private bool turnaround;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = defaultPos;
        turnaround = false;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        while (turnaround)
        {
            transform.Rotate(0f,1f * Time.deltaTime,0f);
        }


    }

    public void MoveTo(Vector3 position)
    {
        targetPos = position;
    }

    public void lookback()
    {
        turnaround = true;
    }
}
