using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class player : MonoBehaviour
{
    [Header("GameObjects")]
    public Rigidbody _rigidbody;
    public FloatingJoystick _joystick;
    public Animator Anim;
    [SerializeField] private GameObject DashButton;
    [SerializeField] private PlayerStatus PlayerStatus;
    public Image dashimg;


    [Header("XP Related")]
    [SerializeField] private float exp = 0;
    [SerializeField] private float maxexp = 100;
    [SerializeField] private int lvl = 1;

    [Header("HP Related")]
    [SerializeField] public float HP;
    [SerializeField] public float maxHP;

    [Header("Status Related")]
    [SerializeField] public float dmg;
    [SerializeField] private float Armor;
    [SerializeField] private float speedTime;
    [SerializeField] private Vector3 currentLocation;
    [SerializeField] public float _speed;
    [SerializeField] public float _originSpeed;
    [SerializeField] public bool SpeedBoost;
    [SerializeField] public float currentTime;

    [SerializeField] private float boostSpeed;
    [SerializeField] private float MaxboostSpeed = 2f;

    [Header("Attack")]
    public Transform target;
    [SerializeField] private GameObject Bulletprefab;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private float shootTime;
    public Transform firePoint;
    private GameObject nearestEnemy = null;
    public float range = 15f;
    public float fireCD = 3f;
    public float fireCount = 0f;

    [Header("Dash")]
    //Will house dashing variables
    [SerializeField] public bool DashReady;
    [SerializeField] public float DashCD = 2f;
    [SerializeField] public float DashCDCurrent = 0.0f;
    [SerializeField] private float DashDistance;

    //Particle Effects
    [Header("Particle Effects")]
    [SerializeField] private GameObject hpBoostVFX; // Assign the GameObject containing the Particle System in the Inspector
    [SerializeField] private GameObject dashBoostVFX; // Assign the GameObject containing the Particle System in the Inspector
    [SerializeField] private GameObject levelUpVFX; // Assign the GameObject containing the Particle System in the Inspector

    //tutorial


    // Start is called before the first frame update

    public async void Start()
    {

        //Setup player Status from save
        //HP, dmg, Armor
        maxHP = await SaveSystem.LoadPlayerHP();
        HP = maxHP;

        dmg = await SaveSystem.LoadPlayerDmg();
        Armor = await SaveSystem.LoadPlayerArmor();





        //Setup finding all the game objects
        PlayerStatus = GameObject.Find("PlayerStatus").GetComponent<PlayerStatus>();
        //catAnim = gameObject.GetComponentInChildren<Animator>();


        currentLocation = transform.position;

        //setup for speedboost
        SpeedBoost = false;
        _speed = 10;
        _originSpeed = _speed;
        boostSpeed = MaxboostSpeed;

        //setup for attack
        InvokeRepeating("UpdateTarget", 0f, 1f);
        Debug.Log("called updateTarget");
        //firePoint = this.transform;


        //Setup UI
        PlayerStatus.UpdateEXP(exp, maxexp);
        PlayerStatus.UpdateHP(HP, maxHP);
    }
    public virtual void Update()
    {



        Anim.SetFloat("move", _rigidbody.velocity.magnitude);

        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _speed, _rigidbody.velocity.y, _joystick.Vertical * _speed);



        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
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

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    //|||||||||||||||||Pass Over Data Functions||||||||||||||||||||||||
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

    public int getLevel()
    {
        return lvl;
    }

    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    //|||||||||||||||||Player Functions||||||||||||||||||||||||||||||||
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

    public void Damage(float _dmg)
    {

        float ActualDamage = _dmg * (100f / (100f + Armor));
        HP -= ActualDamage;
        PlayerStatus.UpdateHP(HP, maxHP);

        //Vibrate the phone
        VibrationManager.Vibrate();
        

        if (HP <= 0)
        {
            //play death animation
            GameObject.Find("GameManager").GetComponent<GameManager>().gameOver();
        }
        //hpBar.UpdateHealthBar(HP, maxHP);
        //Debug.Log(HP);
    }


    public void xpUp(int xps)
    {
        exp += xps;
        if (exp >= maxexp)
        {
            lvl += 1;
            maxexp += exp * 0.5f;
            exp = 0;
            //lvlText.text = lvl.ToString();

            GameObject LvlUpEffect = (GameObject)Instantiate(levelUpVFX, transform.position, transform.rotation);
            LvlUpEffect.transform.parent = this.transform;
            Destroy(LvlUpEffect, 1f);
            if (fireCD > 0.5f)
            {
                fireCD -= 0.1f;
            }
        }
        // else if (exp > maxexp){
        //     lvl += 1;
        //     exp -= maxexp;
        //     maxexp += exp * 0.5f;
        //     lvlText.text = lvl.ToString();
        // }
        //xpBar.GetComponent<xpBar>().UpdateXPbar(exp, maxexp);
        PlayerStatus.UpdateEXP(exp, maxexp);

    }


    //Dashing
    void IsDashing()
    {
        //This is a timer to delay the Dash ability
        if (DashCDCurrent >= DashCD)
        {
            DashReady = true;
            DashButton.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            DashReady = false;
            DashCDCurrent += Time.deltaTime;
            DashCDCurrent = Mathf.Clamp(DashCDCurrent, 0.0f, DashCD);
        }

    }

    //Need different method
    public void Dash()
    {
        if (DashReady)
        {
            Vector3 dashDirection;

            // Check if the joystick is pointing in a direction
            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                // Joystick is pointing in a direction
                dashDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
            }
            else
            {
                // Joystick is not pointing anywhere, dash forward
                dashDirection = transform.forward;
            }

            // Calculate the dash destination point
            Vector3 dashDestination = transform.position + dashDirection * DashDistance;

            // Teleport the player to the dash destination
            this.transform.position = dashDestination;

            DashCDCurrent = 0.0f;
            DashReady = false;
            dashimg.fillAmount = 0.0f;

            GameObject DashEffect = (GameObject)Instantiate(dashBoostVFX, transform.position, transform.rotation);
            Destroy(DashEffect, 1f);
        }
    }

    public void Heal(float _heal)
    {

        HP += _heal;
        //hpBar.UpdateHealthBar(HP, maxHP);
        PlayerStatus.UpdateHP(HP, maxHP);

        GameObject HealthEffect = (GameObject)Instantiate(hpBoostVFX, transform.position, transform.rotation);
        HealthEffect.transform.parent = this.transform;
        Destroy(HealthEffect, 1f);
    }

    public void Boost(float _time)
    {
        currentTime = Time.time;
        speedTime = _time;
        SpeedBoost = true;

        boostSpeed *= _speed;

        //Math so that the player "shouldn't" be able to become the flash when boosting.
        if (boostSpeed >= 50) { boostSpeed /= 3; }
        if (boostSpeed >= 100) { boostSpeed /= 4; }
        if (boostSpeed >= 150) { boostSpeed /= 5; }
        if (boostSpeed >= 200) { boostSpeed /= 6; }
    }

    public void HPboost()
    {
        maxHP += 30;
        HP += 30;
        //hpBar.UpdateHealthBar(HP, maxHP);
        PlayerStatus.UpdateHP(HP, maxHP);

        GameObject HealthEffect = (GameObject)Instantiate(hpBoostVFX, transform.position, transform.rotation);
        HealthEffect.transform.parent = this.transform;
        Destroy(HealthEffect, 1f);
    }

    public void SpeedIncrease()
    {
        _speed += 1;
        _originSpeed = _speed;
    }

    public void Revive()
    {
        HP = maxHP;
        //hpBar.UpdateHealthBar(HP, maxHP);
        PlayerStatus.UpdateHP(HP, maxHP);
    }



    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    //|||||||||||||||||Attack Functions||||||||||||||||||||||||||||||||
    //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && enemy.GetComponent<enemy>().getHP() > 0)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }


        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    public void launchBullet()
    {
        GameObject bulletGO = (GameObject)Instantiate(Bulletprefab, firePoint.position, Quaternion.identity);
        bullet _bullet = bulletGO.GetComponent<bullet>();
        //play audio
        attackSound.Play();

        if (_bullet != null)
        {
            _bullet.setup(target, dmg);

        }
    }

    public void IncreaseRange()
    {
        range += 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Damage(1f);
        }
    }




}
