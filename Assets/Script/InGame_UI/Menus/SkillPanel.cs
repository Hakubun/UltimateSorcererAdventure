using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPanel : MonoBehaviour
{

    [SerializeField] public List<ButtonScriptableObject> Upgrades;
    [SerializeField] Button[] Buttons;
    [SerializeField] GameManager gm;
    [SerializeField] GameObject ButtonPrefab;
    [SerializeField] Transform[] Pos;
    List<GameObject> generatedButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SetupBtns()
    {
        // Reset the skill ID when setting up the buttons

        List<ButtonScriptableObject> selectedButtons = _getRandomTypes();

        int buttonCount = (Upgrades.Count != 2) ? 3 : 2; // Adjust the number of buttons based on Upgrades count

        for (int i = 0; i < buttonCount; i++)
        {
            GameObject btn = Instantiate(ButtonPrefab, Pos[i]);
            btn.transform.localPosition = Vector3.zero;
            generatedButtons.Add(btn);
            btn.GetComponent<upgradeButton>().SetupBtn(selectedButtons[i]);

            int currentSkillID = selectedButtons[i]._skillID;
            btn.GetComponent<Button>().onClick.AddListener(() => gm.UpgradeControl(currentSkillID));
        }
    }

    private List<ButtonScriptableObject> _getRandomTypes()
    {
        List<ButtonScriptableObject> types = new List<ButtonScriptableObject>(Upgrades);
        List<ButtonScriptableObject> selectedButtons = new List<ButtonScriptableObject>();

        for (int i = 0; i < 3 && types.Count > 0; i++)
        {
            int choose = Random.Range(0, types.Count);
            selectedButtons.Add(types[choose]);
            types.RemoveAt(choose);
        }

        return selectedButtons;
    }

    public void ResetSkillButton()
    {
        if (generatedButtons.Count > 0)
        {
            foreach (GameObject button in generatedButtons)
            {
                Destroy(button);
            }

            generatedButtons.Clear();
        }

    }

    private void OnDisable()
    {
        ResetSkillButton();
    }

    private void OnEnable()
    {
        SetupBtns();
    }

    // Assuming ButtonScriptableObject has a property named SkillName that stores the name of the skill
    public void RemoveSkillByName(string skillName)
    {
        Upgrades.RemoveAll(button => button._name == skillName);
    }
}
