using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class ShopLogic : MonoBehaviour
{


    [SerializeField] private int coinNum;
    [SerializeField] private int gemNum;
    [SerializeField] private StatusBarUI StatusBar;
    [SerializeField] private AudioSource LootBoxSound;
    [SerializeField] private GameObject topBar; // Assign this in the inspector
    private Button backButton; // Button reference

    [Header("LootBox Related")]

    [SerializeField] private GameObject OpenBoxPopUp;
    [SerializeField] private GameObject[] Loots;
    [SerializeField] private Image BoxImg;
    [SerializeField] private Image BoxOpenImg;
    [SerializeField] private Sprite[] BoxImgs;
    [SerializeField] private Sprite[] BoxOpenImgs;


    [Header("Loots ScriptablesList")]
    [SerializeField] List<LootScriptableObject> Brown;
    [SerializeField] List<LootScriptableObject> Green;
    [SerializeField] List<LootScriptableObject> Blue;
    [SerializeField] List<LootScriptableObject> Purple;
    [SerializeField] List<LootScriptableObject> Red;
    [SerializeField] List<LootScriptableObject> Gold;
    [SerializeField] List<LootScriptableObject> Glow;


    async void Start()
    {
        coinNum = await SaveSystem.LoadCoin();
        gemNum = await SaveSystem.LoadGem();
        OpenBoxPopUp.SetActive(false);

        if (topBar != null)
        {
            Transform backButtonTransform = topBar.transform.Find("Button_Back");
            if (backButtonTransform != null)
            {
                backButton = backButtonTransform.GetComponent<Button>();
            }
            else
            {
                Debug.LogError("Button_Back not found in TopBar");
            }
        }
        else
        {
            Debug.LogError("TopBar is not assigned in the inspector");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpendGold(int ID)
    {
        //ID: 0 = Tiny of Gem, 1 = Small Potion, 2 = Medium Potion, 3 = Large Potion, 4 = Sliver Chest
        //need better function

        switch (ID)
        {
            case 0:
                //Need to figure out what to do here
                giveGem(10);
                break;
            case 1:
                StatusBar.increaseEnergy(20);
                break;
            case 2:
                StatusBar.increaseEnergy(30);
                break;
            case 3:
                StatusBar.increaseEnergy(60);
                break;
            case 4:
                OpenSilverChest();
                break;
        }

        // Debug.Log(price + " Amount of gold spent");


    }

    public void SpendGem(int ID)
    {

        //ID: 0 = Mega Potion, 1 = Gold Chest, 2 = Epic Chest, 3 = Legendary Chest


        switch (ID)
        {
            case 0:
                //Need to figure out what to do here
                StatusBar.increaseEnergy(100); ;
                break;
            case 1:
                //Need chest function
                //golden chest
                OpenGoldenLootBox();
                //Debug.Log();

                break;
            case 2:
                OpenEpicLootBox();
                break;
            case 3:
                OpenLegendaryLootBox();
                break;

        }

        // Debug.Log(price + " Amount of gold spent");


    }

    public void giveGold(int amount)
    {
        coinNum += amount;
        SaveSystem.SaveCoin(coinNum);
        StatusBar.UpdateCoin();
    }

    public void giveGem(int amount)
    {
        gemNum += amount;
        SaveSystem.SaveGem(gemNum);
        StatusBar.UpdateGem();
    }


    ///////LOOT BOX SYSTEM////////
    void SetUpBox(int ID)
    {//ID 0: wooden, ID 1: Silver, ID 2: Golden, ID 3: Epic, ID 4: Legendary

        BoxImg.sprite = BoxImgs[ID];
        BoxOpenImg.sprite = BoxOpenImgs[ID];
        OpenBoxPopUp.SetActive(true);
        LootBoxSound.Play();
    }
    // Function to open the loot box
    public void OpenWoodenChest()
    {
        List<LootScriptableObject>[] categories = { Blue, Green, Brown };
        float[] probabilities = { 0.2f, 0.3f, 0.5f };

        // Number of objects to pick
        int numberOfObjectsToPick = 3;
        List<LootScriptableObject> selectedObjects = new List<LootScriptableObject>();

        for (int pickIndex = 0; pickIndex < numberOfObjectsToPick; pickIndex++)
        {
            // Use Random.Range to generate a random number between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Determine the selected category based on probabilities
            float cumulativeProbability = 0f;
            for (int i = 0; i < categories.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    // Disable the Button_Back when this function is called
                    backButton.gameObject.SetActive(false);

                    // Randomly pick one of the objects in the selected category
                    int randomIndex = Random.Range(0, categories[i].Count);
                    LootScriptableObject selectedObject = categories[i][randomIndex];
                    selectedObjects.Add(selectedObject);
                    break;
                }
            }
        }

        // Do something with the selected objects (e.g., display, use, etc.)
        foreach (var obj in selectedObjects)
        {
            Debug.Log(obj.ID);
            Debug.Log(obj.info);
            Debug.Log(obj.name);
        }
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().SetUpLoots(selectedObjects);
        SetUpBox(0);
    }

    public void OpenSilverChest()
    {
        List<LootScriptableObject>[] categories = { Green, Blue, Purple };
        float[] probabilities = { 0.2f, 0.3f, 0.5f };

        // Number of objects to pick
        int numberOfObjectsToPick = 3;
        List<LootScriptableObject> selectedObjects = new List<LootScriptableObject>();

        for (int pickIndex = 0; pickIndex < numberOfObjectsToPick; pickIndex++)
        {
            // Use Random.Range to generate a random number between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Determine the selected category based on probabilities
            float cumulativeProbability = 0f;
            for (int i = 0; i < categories.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    // Disable the Button_Back when this function is called
                    backButton.gameObject.SetActive(false);

                    // Randomly pick one of the objects in the selected category
                    int randomIndex = Random.Range(0, categories[i].Count);
                    LootScriptableObject selectedObject = categories[i][randomIndex];
                    selectedObjects.Add(selectedObject);
                    break;
                }
            }
        }

        // Do something with the selected objects (e.g., display, use, etc.)
        foreach (var obj in selectedObjects)
        {
            Debug.Log(obj.ID);
            Debug.Log(obj.info);
            Debug.Log(obj.name);
        }
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().SetUpLoots(selectedObjects);
        SetUpBox(1);
    }
    public void OpenGoldenLootBox()
    {
        List<LootScriptableObject>[] categories = { Red, Blue, Purple };
        float[] probabilities = { 0.2f, 0.3f, 0.5f };


        // Number of objects to pick
        int numberOfObjectsToPick = 3;
        List<LootScriptableObject> selectedObjects = new List<LootScriptableObject>();

        for (int pickIndex = 0; pickIndex < numberOfObjectsToPick; pickIndex++)
        {
            // Use Random.Range to generate a random number between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Determine the selected category based on probabilities
            float cumulativeProbability = 0f;
            for (int i = 0; i < categories.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    // Disable the Button_Back when this function is called
                    backButton.gameObject.SetActive(false);

                    // Randomly pick one of the objects in the selected category
                    int randomIndex = Random.Range(0, categories[i].Count);
                    LootScriptableObject selectedObject = categories[i][randomIndex];
                    selectedObjects.Add(selectedObject);
                    break;
                }
            }
        }

        // Do something with the selected objects (e.g., display, use, etc.)
        foreach (var obj in selectedObjects)
        {
            Debug.Log(obj.ID);
            Debug.Log(obj.info);
            Debug.Log(obj.name);
        }
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().SetUpLoots(selectedObjects);
        SetUpBox(2);
    }

    public void OpenEpicLootBox()
    {
        List<LootScriptableObject>[] categories = { Purple, Gold, Red };
        float[] probabilities = { 0.2f, 0.3f, 0.5f };


        // Number of objects to pick
        int numberOfObjectsToPick = 3;
        List<LootScriptableObject> selectedObjects = new List<LootScriptableObject>();

        for (int pickIndex = 0; pickIndex < numberOfObjectsToPick; pickIndex++)
        {
            // Use Random.Range to generate a random number between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Determine the selected category based on probabilities
            float cumulativeProbability = 0f;
            for (int i = 0; i < categories.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    // Disable the Button_Back when this function is called
                    backButton.gameObject.SetActive(false);

                    // Randomly pick one of the objects in the selected category
                    int randomIndex = Random.Range(0, categories[i].Count);
                    LootScriptableObject selectedObject = categories[i][randomIndex];
                    selectedObjects.Add(selectedObject);
                    break;
                }
            }
        }

        // Do something with the selected objects (e.g., display, use, etc.)
        foreach (var obj in selectedObjects)
        {
            Debug.Log(obj.ID);
            Debug.Log(obj.info);
            Debug.Log(obj.name);
        }
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().SetUpLoots(selectedObjects);
        SetUpBox(3);
    }

    public void OpenLegendaryLootBox()
    {
        List<LootScriptableObject>[] categories = { Glow, Red, Gold };
        float[] probabilities = { 0.2f, 0.3f, 0.5f };


        // Number of objects to pick
        int numberOfObjectsToPick = 3;
        List<LootScriptableObject> selectedObjects = new List<LootScriptableObject>();

        for (int pickIndex = 0; pickIndex < numberOfObjectsToPick; pickIndex++)
        {
            // Use Random.Range to generate a random number between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Determine the selected category based on probabilities
            float cumulativeProbability = 0f;
            for (int i = 0; i < categories.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    // Disable the Button_Back when this function is called
                    backButton.gameObject.SetActive(false);

                    // Randomly pick one of the objects in the selected category
                    int randomIndex = Random.Range(0, categories[i].Count);
                    LootScriptableObject selectedObject = categories[i][randomIndex];
                    selectedObjects.Add(selectedObject);
                    break;
                }
            }
        }

        // Do something with the selected objects (e.g., display, use, etc.)
        foreach (var obj in selectedObjects)
        {
            Debug.Log(obj.ID);
            Debug.Log(obj.info);
            Debug.Log(obj.name);
        }
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().SetUpLoots(selectedObjects);
        SetUpBox(4);
    }

    public void LootClicked()
    {
        OpenBoxPopUp.GetComponent<LootBoxPopUp>().ResetLootSlot();
        OpenBoxPopUp.SetActive(false);
        backButton.gameObject.SetActive(true);
        
    }
}
