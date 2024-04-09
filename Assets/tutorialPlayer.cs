using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPlayer : player
{
    [SerializeField] bool pause;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        pause = true;
    }

    // Update is called once per frame
    public override void Update()
    {


        if (!pause)
        {

            Anim.SetFloat("move", _rigidbody.velocity.magnitude);

            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _speed, _rigidbody.velocity.y, _joystick.Vertical * _speed);



            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                Debug.Log("Move!");
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            }





            //attack logic
            if (fireCount >= fireCD && target != null)
            {
                launchBullet();
                fireCount = 0;
            }
            else
            {
                //Anim.SetBool("attack", false);
                fireCount += 1 * Time.deltaTime;
            }
            if (!DashReady)
            {
                dashimg.fillAmount = DashCDCurrent / DashCD;
                DashCDCurrent += 1 * Time.deltaTime;

                if (DashCDCurrent >= DashCD)
                {
                    dashimg.fillAmount = 1;
                    DashReady = true;
                }
            }
        }

    }
    public void UnpauseCharacter()
    {
        pause = false;
    }
}
