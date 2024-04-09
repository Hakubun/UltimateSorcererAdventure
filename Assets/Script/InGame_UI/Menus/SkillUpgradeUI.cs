using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUpgradeUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public int maxButtons;
    public List<string> SkillTypes;

    public List<string> ChestTypes;

    private GameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void GenerateButtons(bool includeChest = false)
    {
        Debug.Log($"Generate Buttons, includeChest = {includeChest}");

        List<string> selectedTypes = _getRandomTypes(includeChest);

        foreach (string type in selectedTypes)
        {

            Debug.Log($"select skill {type}");
            GameObject newButton = Instantiate(buttonPrefab, transform);

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = type;

            Button btn = newButton.GetComponentInChildren<Button>();

            //btn.onClick.AddListener(() => gameManagerScript.SkillUpgradeButtonControl(type));
        }

    }

    private List<string> _getRandomTypes(bool includeChest = false)
    {
        List<string> types = new List<string>();

        List<string> selectedTypes = new List<string>();

        types.AddRange(SkillTypes);

        if (includeChest)
        {
            types.AddRange(ChestTypes);
        }

        Debug.Log(string.Join("\n,", types.ToArray()));

        for (int i = 0; i < maxButtons; i++)
        {
            int choose = Random.Range(0, types.Count);
            selectedTypes.Add(types[choose]);
            types.RemoveAt(choose);
        }

        return selectedTypes;

    }

}

