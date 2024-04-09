using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Image HP;
    [SerializeField] private Slider EXP;
    [SerializeField] private Slider wave;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI KillCount;
    [SerializeField] private TextMeshProUGUI CoinCount;
    //[SerializeField] private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        //getHP
        //GetEXP
        //GetKills
    }

    // Update is called once per frame

    public void UpdateHP(float min, float max)
    {
        HP.fillAmount = min / max;

    }

    public void UpdateEXP(float min, float max)
    {
        EXP.value = min / max;
    }



    public void UpdateLvlText(int lvl)
    {
        lvlText.text = lvl.ToString();
    }

    public void UpdateLevelProgress(float currentwave, int totalwave)
    {
        if (totalwave == 0)
        {
            wave.value = 1;
            waveText.text = "\u221E";
        }
        else
        {

            wave.value = currentwave / totalwave;
            waveText.text = currentwave.ToString() + "/" + totalwave.ToString();
        }

    }

    public void UpdateLevelProgressInfo(string text)
    {
        waveText.text = text;
    }

    public void UpdateKill(int num)
    {
        KillCount.text = num.ToString();
    }

    public void UpdateCoin(int ammount)
    {
        CoinCount.text = ammount.ToString();
    }
}
