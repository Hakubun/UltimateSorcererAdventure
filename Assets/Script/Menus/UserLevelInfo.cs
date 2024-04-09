using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserLevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI username;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI lvlProgressText;
    [SerializeField] private Slider lvlSlider;
    [SerializeField] private AccountManager am;
    // Start is called before the first frame update
    async void Start()
    {
        string Playername;
        Playername = SaveSystem.LoadLogIn().username;
        CloudSaveScript.instance.SaveUserName(Playername);
        username.text = Playername;
        PlayerEXP data = await SaveSystem.LoadPlayerEXP();
        int _exp = data.exp;
        int _lvl = data.lvl;
        int _req = data.req;
        lvlSlider.value = (float)_exp / (float)_req;
        lvlText.text = _lvl.ToString();
        lvlProgressText.text = _exp + "/" + _req;
    }

}
