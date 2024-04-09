using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSkillControlButton : MonoBehaviour
{

    [Header("Status")]
    [SerializeField] private bool Activated;
    [SerializeField] public bool SkillReady;
    [SerializeField] public float CD;
    [SerializeField] public float CDCurrent;
    [SerializeField] private int lvl;
    [Header("Drag In GameObject")]
    public SkillManager manager; //Reference to the Skill Manager

    public Image SkillImage_FG;
    public Image SkillImage_BG;
    public GameObject[] ProGressRing;
    public Sprite[] _skills;

    public int Skill_ID;

    // Start is called before the first frame update
    void Start()
    {
        CDCurrent = CD;
        this.gameObject.GetComponent<Button>().interactable = false;
        SkillImage_FG.gameObject.SetActive(false);
        SkillImage_BG.gameObject.SetActive(false);
        lvl = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!SkillReady)
        {
            SkillImage_FG.fillAmount = CDCurrent / CD;
            CDCurrent += 1 * Time.deltaTime;

            if (CDCurrent >= CD)
            {
                SkillImage_FG.fillAmount = 1;
                SkillReady = true;
            }
        }

    }

    public void SetUpButton(int _id)
    {
        SkillImage_FG.gameObject.SetActive(true);
        SkillImage_BG.gameObject.SetActive(true);
        SkillImage_FG.sprite = _skills[_id];
        SkillImage_BG.sprite = _skills[_id];
        ProGressRing[0].SetActive(true);
        this.gameObject.GetComponent<Button>().interactable = true;
        Skill_ID = _id;
    }

    public void ActivateSkill()
    {
        Debug.Log("Pressed");
        //switch statement
        SkillReady = false;

        SkillImage_FG.fillAmount = 0.0f;
        CDCurrent = 0.0f;

        switch (Skill_ID)
        {
            case 1:
                manager.Rain.ActivateSkill();
                break;
            case 2:
                manager.lighting.ActivateSkill();
                break;
            case 3:
                manager.meteor.ActivateSkill();
                break;
            case 4:
                manager.Spike.ActivateSkill();
                break;

            default:
                break;
        }
    }

    public void LevelUp() {
        lvl++;
        for (int i = 0; i < lvl; i++) {
            ProGressRing[i].SetActive(true);
        }
    }
}
