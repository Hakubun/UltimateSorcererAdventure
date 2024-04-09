using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    [SerializeField] private TextMeshProUGUI text;

    //[SerializeField] private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHP(float Hp, float maxHp)
    {
        if (Hp <= 0) {
            Hp = 0;
        }

        bar.value = Hp / maxHp;

        text.text = Hp.ToString() + "/" + maxHp.ToString();
    }
}
