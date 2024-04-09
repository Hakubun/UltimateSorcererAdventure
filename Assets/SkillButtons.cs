using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButtons : MonoBehaviour
{
    public Button[] skillButtons; //Button array to hold all the skill buttons.
    
    public SkillPanel skillPanel; // Reference to the SkillPanel script
    public SkillManager manager; //Reference to the Skill Manager

    private int chosenSkillsCount = 0; // Counter for the number of chosen skills

    public Sprite defaultBGImage; // Default image for the BG (background) child
    public Sprite defaultFGImage; // Default image for the FG (foreground) child
    
    // Gets all the skills levels
    private int knifeLevel;
    private int lightningLevel;
    private int meteorLevel;
    private int iceLevel;
    private int flowerLevel;

    //To get the Cooldown
    private Image bgImage;
    private Image fgImage;
    private float CoolDownTimer;
    private float CDCurrent;

    // Dictionary to store the mapping between skill names and images
    private Dictionary<string, Image> skillFgImages = new Dictionary<string, Image>();
    private Dictionary<string, Coroutine> cooldownCoroutines = new Dictionary<string, Coroutine>();
    private Dictionary<Button, string> buttonSkillMapping = new Dictionary<Button, string>();
    private Dictionary<string, int> skillLevels = new Dictionary<string, int>();

    private List<string> selectedSkills = new List<string>(); // List to store selected skills dynamically                                                       
    private List<string> availableSkills = new List<string>(); // Define a list to hold the available skills
    private List<(string skillName, Sprite sprite)> skillImagesList = new List<(string, Sprite)>();
   
    //Enum to store our skills, rather then hardcoding strings
    private enum SkillName
    {
        KnifeRain,
        Lightning,
        Meteor,
        Ice,
        Flower
    }

    void Start()
    {
        // Change the image based on push alarm state
        AddSkillImage(SkillName.Meteor, "UI/SkillsIcons/Skill3");
        AddSkillImage(SkillName.Lightning, "UI/SkillsIcons/Skill4");
        AddSkillImage(SkillName.KnifeRain, "UI/SkillsIcons/Skill5");
        AddSkillImage(SkillName.Ice, "UI/SkillsIcons/Skill6");
        AddSkillImage(SkillName.Flower, "UI/SkillsIcons/Skill7");

        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i; // Capture the index in a local variable to avoid closure issues
            skillButtons[i].onClick.AddListener(() => OnSkillButtonClick(index));
        }
    }

    // Helper method to add skill images to the list
    private void AddSkillImage(SkillName skillName, string imagePath)
    {
        string fullPath = imagePath;  // No need to add "UI/SkillsIcons/" since it's already provided in the Start method
        Sprite sprite = Resources.Load<Sprite>(fullPath);

        if (sprite != null)
        {
            skillImagesList.Add((skillName.ToString(), sprite));
        }
        else
        {
            Debug.LogError("Failed to load sprite for skill: " + skillName.ToString());
        }
    }

    void Update()
    {
        // Get all skills levels
        knifeLevel = manager.KnifeRainLvl;
        lightningLevel = manager.LightningLvl;
        meteorLevel = manager.MeteorLvl;
        iceLevel = manager.IceLvl;
        flowerLevel = manager.FlowerLvl;

        // Check if each skill level is greater than 0 and the maximum number of chosen skills has not been reached
        if (knifeLevel > 0 && chosenSkillsCount < 3 && !selectedSkills.Contains(SkillName.KnifeRain.ToString()))
        {
            AssignSkillToButton(skillButtons[chosenSkillsCount], SkillName.KnifeRain.ToString(), knifeLevel);
            availableSkills.Add(SkillName.KnifeRain.ToString());
            chosenSkillsCount++;
        }
        if (lightningLevel > 0 && chosenSkillsCount < 3 && !selectedSkills.Contains(SkillName.Lightning.ToString()))
        {
            AssignSkillToButton(skillButtons[chosenSkillsCount], SkillName.Lightning.ToString(), lightningLevel);
            availableSkills.Add(SkillName.Lightning.ToString());
            chosenSkillsCount++;
        }
        if (meteorLevel > 0 && chosenSkillsCount < 3 && !selectedSkills.Contains(SkillName.Meteor.ToString()))
        {
            AssignSkillToButton(skillButtons[chosenSkillsCount], SkillName.Meteor.ToString(), meteorLevel);
            availableSkills.Add(SkillName.Meteor.ToString());
            chosenSkillsCount++;
        }
        if (iceLevel > 0 && chosenSkillsCount < 3 && !selectedSkills.Contains(SkillName.Ice.ToString()))
        {
            AssignSkillToButton(skillButtons[chosenSkillsCount], SkillName.Ice.ToString(), iceLevel);
            availableSkills.Add(SkillName.Ice.ToString());
            chosenSkillsCount++;
        }
        if (flowerLevel > 0 && chosenSkillsCount < 3 && !selectedSkills.Contains(SkillName.Flower.ToString()))
        {
            AssignSkillToButton(skillButtons[chosenSkillsCount], SkillName.Flower.ToString(), flowerLevel);
            availableSkills.Add(SkillName.Flower.ToString());
            chosenSkillsCount++;
        }

        if (chosenSkillsCount == 3)
        {
            // Remove the skills not selected by the player
            List<SkillName> skillsToRemove = new List<SkillName>();
            foreach (SkillName skillEnum in Enum.GetValues(typeof(SkillName)))
            {
                string skillName = skillEnum.ToString();

                if (!selectedSkills.Contains(skillName) && !availableSkills.Contains(skillName))
                {
                    skillsToRemove.Add(skillEnum);
                }
            }

            // Remove the marked skills from the skill panel
            foreach (var skillEnum in skillsToRemove)
            {
                string correspondingSkillString = GetCorrespondingSkillString(skillEnum);
                //Debug.Log(correspondingSkillString);
                if (!string.IsNullOrEmpty(correspondingSkillString))
                {
                    skillPanel.RemoveSkillByName(correspondingSkillString);
                }
            }
        }
        UpdateSkillLevels();
    }

    private void UpdateSkillLevels()
    {
        skillLevels[SkillName.KnifeRain.ToString()] = knifeLevel;
        skillLevels[SkillName.Lightning.ToString()] = lightningLevel;
        skillLevels[SkillName.Meteor.ToString()] = meteorLevel;
        skillLevels[SkillName.Ice.ToString()] = iceLevel;
        skillLevels[SkillName.Flower.ToString()] = flowerLevel;
    }

    private string GetCorrespondingSkillString(SkillName skillEnum)
    {
        switch (skillEnum)
        {
            case SkillName.KnifeRain:
                return "Knife Rain";
            case SkillName.Lightning:
                return "Lightning Strike";
            case SkillName.Meteor:
                return "Meteor Strike";
            case SkillName.Ice:
                return "Ice Lotus";
            case SkillName.Flower:
                return "Flower Slash";
            // Add cases for other enum values

            default:
                return string.Empty;
        }
    }

    private void AssignButtonTag(Button button, string skillName)
    {
        button.tag = skillName;
        //Debug.Log("Button tagged: " + button.name + " with skill: " + skillName);
    }

    void AssignSkillToButton(Button button, string skillName, int skillLevel)
    {
       //Debug.Log("INSIDE ASSIGNSKILLTOBUTTON");
       //Debug.Log(skillName);

        // Get the BG and FG child game objects
        Transform bgTransform = button.transform.Find("BG");
        Transform fgTransform = button.transform.Find("FG");
        // Find the sprite based on the skill name
        var skillTuple = skillImagesList.Find(tuple => tuple.skillName == skillName);

        if (skillTuple != default)
        {
            // Set the BG and FG images based on the skill name
            bgImage = bgTransform.GetComponent<Image>();
            fgImage = fgTransform.GetComponent<Image>();
            bgImage.sprite = skillTuple.sprite;
            fgImage.sprite = skillTuple.sprite;

            // Store the fgImage reference in the dictionary
            skillFgImages[skillName] = fgImage;
        }
        else
        {
            Debug.LogError("Sprite not found for skill: " + skillName);
        }

        // Call the new function to activate skill progress based on skill level
        ActivateSkillProgress(button, skillName, skillLevel);
        // Call the new function to assign a tag to the button
        AssignButtonTag(button, skillName);
        // Add the skill to the selectedSkills list
        selectedSkills.Add(skillName);
    }

    public void OnSkillButtonClick(int selectedSkillIndex)
    {
        if (selectedSkillIndex < selectedSkills.Count)
        {
            string selectedSkill = selectedSkills[selectedSkillIndex];

            if (cooldownCoroutines.ContainsKey(selectedSkill))
            {
                StopCoroutine(cooldownCoroutines[selectedSkill]);
                cooldownCoroutines.Remove(selectedSkill);
            }

            if (manager.CanUseSkill(selectedSkill))
            {
                Button clickedButton = skillButtons[selectedSkillIndex];

                if (!buttonSkillMapping.ContainsKey(clickedButton))
                {
                    // Map the button to the selected skill
                    buttonSkillMapping.Add(clickedButton, selectedSkill);

                    // Disable the button
                    clickedButton.interactable = false;

                    Coroutine cooldownCoroutine = StartCoroutine(CooldownCoroutine(selectedSkill, clickedButton));
                    cooldownCoroutines[selectedSkill] = cooldownCoroutine;

                    switch (selectedSkill)
                    {
                        case "KnifeRain":
                            manager.Rain.ActivateSkill();
                            break;
                        case "Lightning":
                            manager.lighting.ActivateSkill();
                            break;
                        case "Meteor":
                            manager.meteor.ActivateSkill();
                            break;
                        case "Ice":
                            manager.Spike.ActivateSkill();
                            break;
                        case "Flower":
                            manager.Slash.ActivateSkill();
                            break;
                    }
                }
            }
        }
    }

    private IEnumerator CooldownCoroutine(string selectedSkill, Button button)
    {
        Skill skill = manager.skills.Find(s => s.skillName == selectedSkill);

        if (skill != null && skillFgImages.TryGetValue(selectedSkill, out Image fgImage))
        {
            float cooldownTime = skill.baseCooldown;
            float startTime = Time.time;

            while (Time.time < startTime + cooldownTime)
            {
                float elapsedTime = Time.time - startTime;
                fgImage.fillAmount = 0 + (elapsedTime / cooldownTime);
                yield return null;
            }

            fgImage.fillAmount = 1;

            // Enable the button after cooldown is completed
            button.interactable = true;

            // Remove the button from the mapping
            buttonSkillMapping.Remove(button);

            manager.CanUseSkill(selectedSkill);

            cooldownCoroutines.Remove(selectedSkill);
        }
    }

    public void ActivateSkillProgress(Button button, string skillName, int skillLevel)
    {
        if (skillLevels.TryGetValue(skillName, out int previousSkillLevel))
        {
            if (skillLevel > previousSkillLevel)
            {
                // Find the "PrgBg" child of the button
                Transform prgBgTransform = button.transform.Find("PrgBg");

                if (prgBgTransform != null)
                {
                    // Iterate through each child of "PrgBg" and activate based on skill level
                    for (int i = 0; i < prgBgTransform.childCount; i++)
                    {
                        Transform child = prgBgTransform.GetChild(i);

                        // Assuming that the child game objects are named "Progress1", "Progress2", ..., "ProgressN"
                        string progressObjectName = "Progress" + (i + 1);

                        // Activate the child based on skill level
                        bool activateChild = (i < skillLevel);
                        child.gameObject.SetActive(activateChild);
                    }
                }
                else
                {
                    Debug.LogError("PrgBg transform not found for skill: " + skillName);
                }
            }
        }
        else
        {
            Debug.LogError("Skill level not found for skill: " + skillName);
        }
    }

   

}