using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Define a class to represent a skill, including its cooldown.
[System.Serializable]
public class Skill
{
    public string skillName;
    public float baseCooldown; // Base cooldown time
    public float cooldownTimer; // To track the remaining cooldown time
    public float upgradedCooldown = 1; // Cooldown time after upgrade
}

// This script manages the player's skills, their upgrades, and skill levels.
public class SkillManager : MonoBehaviour
{

    [SerializeField] player PlayerStatus;
    public int PlayerLevel;
    [SerializeField] public SkillPanel skillPanel; // Add this line to hold a reference to the SkillPanel script
    [SerializeField] public SkillButtons Skillbutton;
    [SerializeField] public LightingStrike lighting;
    [SerializeField] public MeteorShower meteor;
    [SerializeField] public KnifeRain Rain;
    [SerializeField] public IceSpike Spike;
    [SerializeField] public FlowerSlash Slash;

    // Skill levels
    [Header("Skills")]
    public int KnifeRainLvl;
    public int LightningLvl;
    public int MeteorLvl;
    public int IceLvl;
    public int FlowerLvl;

    //Variables to turn off skills when they reached max lvl
    [Header("MaxSkill")]
    public bool MeteorMax = false;
    public bool LightningMax = false;
    public bool KnifeRainMax = false;
    public bool IceSpikeMax = false;
    public bool FlowerSlashMax = false;

    [Header("Skill Cooldowns")]
    public List<Skill> skills;  // List of skills with cooldown information

    // Static instance property
    public static SkillManager instance;

    void Awake()
    {
        // Set the instance on Awake
        instance = this;

        // Existing Awake code...
    }


    // Start is called before the first frame update
    void Start()
    {
        // Get player status and initialize skill levels.
        PlayerStatus = GameObject.FindWithTag("Player").GetComponent<player>();
        PlayerLevel = PlayerStatus.getLevel();

        KnifeRainLvl = 0;
        LightningLvl = 0;
        MeteorLvl = 0;
        IceLvl = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic goes here if needed
    }

    public void UpgradeSkill(string skillName)
    {
        // Find the skill in the list
        Skill skill = skills.Find(s => s.skillName == skillName);

        // Upgrade the corresponding skill
        switch (skillName)
        {
            case "Lightning":
                lighting.UpgradeDmg();
                skill.baseCooldown = 5;
                skill.upgradedCooldown = 1f;
                skill.baseCooldown -= skill.upgradedCooldown;
                LightningLvl++;

                if (LightningLvl >= 4 && !LightningMax)
                {
                    LightningMax = true;

                    skillPanel.RemoveSkillByName("Lightning Strike");
                }

                Button lightningButton = FindButtonBySkillName("Lightning");
                if (lightningButton != null)
                {
                    ActivateSkillProgressForButton(lightningButton);
                }

                break;
            case "Meteor":
                meteor.UpgradeDmg();
                skill.baseCooldown = 5;
                skill.upgradedCooldown = 1f;
                skill.baseCooldown -= skill.upgradedCooldown;
                MeteorLvl++;

                if (MeteorLvl >= 4 && !MeteorMax)
                {
                    MeteorMax = true;

                    skillPanel.RemoveSkillByName("Meteor Strike");
                }

                Button meteorButton = FindButtonBySkillName("Meteor");
                if (meteorButton != null)
                {
                    ActivateSkillProgressForButton(meteorButton);
                }

                break;
            case "KnifeRain":
                Rain.UpgradeDmg(); 
                skill.baseCooldown = 5;
                skill.upgradedCooldown = 1f;
                skill.baseCooldown -= skill.upgradedCooldown;
                KnifeRainLvl++;

                if (KnifeRainLvl >= 4 && !KnifeRainMax)
                {
                    KnifeRainMax = true;

                    skillPanel.RemoveSkillByName("Knife Rain");
                }

                Button knifeButton = FindButtonBySkillName("KnifeRain");
                if (knifeButton != null)
                {
                    ActivateSkillProgressForButton(knifeButton);
                }


                break;
            case "Ice":
                Spike.UpgradeDmg();
                skill.baseCooldown = 30;
                skill.upgradedCooldown = 2f;
                skill.baseCooldown -= skill.upgradedCooldown;
                IceLvl++;
                
                if (IceLvl >= 4 && !IceSpikeMax)
                {
                    IceSpikeMax = true;

                    skillPanel.RemoveSkillByName("Ice Lotus");
                }

                Button iceButton = FindButtonBySkillName("Ice");
                if (iceButton != null)
                {
                    ActivateSkillProgressForButton(iceButton);
                }

                break;
            case "Flower":
                Slash.UpgradeDmg();
                skill.baseCooldown = 30;
                skill.upgradedCooldown = 2;
                skill.baseCooldown -= skill.upgradedCooldown;
                FlowerLvl++;

                if (FlowerLvl >= 4 && !FlowerSlashMax)
                {
                    FlowerSlashMax = true;
                    skillPanel.RemoveSkillByName("Flower Slash");
                }

                Button flowerButton = FindButtonBySkillName("Flower");
                if (flowerButton != null)
                {
                    ActivateSkillProgressForButton(flowerButton);
                }
                break;

                // Add more cases if you have more skills
        }
    }

    // Check if a skill is upgradable
    private bool CanUpgrade(string skillName)
    {
        switch (skillName)
        {
            case "Lightning":
                return LightningLvl < 4 && !LightningMax;
            case "Meteor":
                return MeteorLvl < 4 && !MeteorMax;
            case "KnifeRain":
                return KnifeRainLvl < 4 && !KnifeRainMax;
            case "Ice":
                return IceLvl < 4 && !IceSpikeMax;
            case "Flower":
                return FlowerLvl < 4 && !FlowerSlashMax;
            // Add more cases if you have more skills
            default:
                return false;
        }
    }

    // Coroutine for handling cooldown
    private IEnumerator CooldownCoroutine(Skill skill)
    {
        skill.cooldownTimer = skill.baseCooldown;

        while (skill.cooldownTimer > 0)
        {
            skill.cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        // Cooldown completed, set skillReady to true or handle other logic
    }

    // Method to trigger the cooldown for a skill
    public void TriggerCooldown(string skillName)
    {
        Skill skill = skills.Find(s => s.skillName == skillName);

        if (skill != null)
        {
            StartCoroutine(CooldownCoroutine(skill));
        }
    }

    // Check if a skill can be used
    public bool CanUseSkill(string skillName)
    {
        // Find the skill in the list
        Skill skill = skills.Find(s => s.skillName == skillName);

        // Check if the skill is not null and its cooldownTimer is less than or equal to 0
        return skill != null && skill.cooldownTimer <= 0;
    }

    // Add this method to call ActivateSkillProgress based on button/tag
    public void ActivateSkillProgressForButton(Button button)
    {
        string skillName = button.tag; // Assuming the button has the skill name as its tag
        int skillLevel = GetSkillLevel(skillName); // Get the skill level from your SkillManager

        // Call ActivateSkillProgress method
        Skillbutton.ActivateSkillProgress(button, skillName, skillLevel);
    }

    // Add this method to find the button based on skill name
    private Button FindButtonBySkillName(string skillName)
    {
        foreach (Button button in Skillbutton.skillButtons)
        {
            if (button.tag == skillName)
            {
                return button;
            }
        }

        return null; // Button not found
    }

    private int GetSkillLevel(string skillName)
    {
        switch (skillName)
        {
            case "KnifeRain":
                return KnifeRainLvl;
            case "Lightning":
                return LightningLvl;
            case "Meteor":
                return MeteorLvl;
            case "Ice":
                return IceLvl;
            case "Flower":
                return FlowerLvl;
            // Add more cases if you have more skills
            default:
                return 0; // or any default value
        }
    }

}
