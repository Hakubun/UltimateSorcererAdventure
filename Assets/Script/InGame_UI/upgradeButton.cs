using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class upgradeButton : MonoBehaviour
{
    //public ButtonScriptableObject BtnSO;

    public TMP_Text ButtonName;
    public Image ButtonIcon;
    public TMP_Text SkillDes;
    SkillManager sm;

    // Start is called before the first frame update
    void Start()
    {
        //SetupBtn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupBtn(ButtonScriptableObject BtnSO)
    {

        ButtonName.text = BtnSO.name;

        ButtonIcon.sprite = BtnSO.Icon;

        //SkillDes.text = BtnSO.Description;

        switch (BtnSO._skillID)
        {
            case 0:
                SkillDes.text = BtnSO.Description;
                break;
            case 1:
                SkillDes.text = BtnSO.Description;
                break;
            case 2: // Meteor skill
                if (GameObject.Find("SkillManager").GetComponent<SkillManager>().MeteorLvl > 0)
                {
                    SkillDes.text = "Skill Cooldown -1 sec";
                }
                else
                {
                    SkillDes.text = BtnSO.Description;
                }
                break;
            case 3: // Lightning skill
                if (GameObject.Find("SkillManager").GetComponent<SkillManager>().LightningLvl > 0)
                {
                    SkillDes.text = "Skill Cooldown -1 sec";
                }
                else
                {
                    SkillDes.text = BtnSO.Description;
                }
                break;
            case 4: // KnifeRain skill
                if (GameObject.Find("SkillManager").GetComponent<SkillManager>().KnifeRainLvl > 0)
                {
                    SkillDes.text = "Skill Cooldown -1 sec";
                }
                else
                {
                    SkillDes.text = BtnSO.Description;
                }
                break;
            case 5: // Ice skill
                if (GameObject.Find("SkillManager").GetComponent<SkillManager>().IceLvl > 0)
                {
                    SkillDes.text = "Skill Cooldown -2 sec";
                }
                else
                {
                    SkillDes.text = BtnSO.Description;
                }
                break;
            case 6: // Flower skill
                if (GameObject.Find("SkillManager").GetComponent<SkillManager>().FlowerLvl > 0)
                {
                    SkillDes.text = "Skill Cooldown -2 sec";
                }
                else
                {
                    SkillDes.text = BtnSO.Description;
                }
                break;
            default:
                break;
        }

    }


}
