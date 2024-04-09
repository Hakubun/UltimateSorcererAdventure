using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusMenuLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HP;
    [SerializeField] private TextMeshProUGUI Attack;
    [SerializeField] private TextMeshProUGUI Armor;
    [SerializeField] private TextMeshProUGUI ExtraLife;
    [SerializeField] private TextMeshProUGUI GoldMultipler;

    // Start is called before the first frame update
    private async void Awake()
    {
        float _hp = await SaveSystem.LoadPlayerHP();
        HP.text = Mathf.RoundToInt(_hp).ToString();
        float _dmg = await SaveSystem.LoadPlayerDmg();
        Attack.text = Mathf.RoundToInt(_dmg).ToString();
        float _amr = await SaveSystem.LoadPlayerArmor();
        Armor.text = Mathf.RoundToInt(_amr).ToString();
        int _lifeCount = await SaveSystem.LoadExtraLife();
        ExtraLife.text = Mathf.RoundToInt(_lifeCount).ToString();
        int _multiplier = await SaveSystem.LoadGoldMultiplier();
        GoldMultipler.text = Mathf.RoundToInt(_multiplier).ToString();
    }

    private async void OnEnable()
    {
        float _hp = await SaveSystem.LoadPlayerHP();
        HP.text = Mathf.RoundToInt(_hp).ToString();
        float _dmg = await SaveSystem.LoadPlayerDmg();
        Attack.text = Mathf.RoundToInt(_dmg).ToString();
        float _amr = await SaveSystem.LoadPlayerArmor();
        Armor.text = Mathf.RoundToInt(_amr).ToString();
        int _lifeCount = await SaveSystem.LoadExtraLife();
        ExtraLife.text = Mathf.RoundToInt(_lifeCount).ToString();
        int _multiplier = await SaveSystem.LoadGoldMultiplier();
        GoldMultipler.text = Mathf.RoundToInt(_multiplier).ToString();
    }
}
