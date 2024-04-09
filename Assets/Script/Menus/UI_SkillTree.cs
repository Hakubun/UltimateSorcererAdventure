// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;


// public class UI_SkillTree : MonoBehaviour
// {
//     public GameObject Container_PlayerDamage;
//     public GameObject Container2_PlayerHealth;
//     public GameObject Container3_ExtraGold;
//     public GameObject goldCounter;

//     private Button[] damageButtons;
//     private Button[] healthButtons;
//     private Button[] goldButtons;

//     private int damageButtonIndex = 0;
//     private int healthButtonIndex = 0;
//     private int goldButtonIndex = 0;

//     //Sets the gold to what we want
//     [SerializeField] public TextMeshProUGUI CoinCount;
//     private int playerGold = 1000;
//     private int baseUpgradeCost = 100; // Initial cost for the first upgrade

//     private async void Awake()
//     {
//         //Sets the gold to what the player has.
//         playerGold = await SaveSystem.LoadCoin();
//         // Update the CoinCount text with the loaded gold value
//         CoinCount.text = playerGold.ToString();
       
//         damageButtons = Container_PlayerDamage.GetComponentsInChildren<Button>();
//         healthButtons = Container2_PlayerHealth.GetComponentsInChildren<Button>();
//         goldButtons = Container3_ExtraGold.GetComponentsInChildren<Button>();

//         //Initially, lock all buttons except the first one in each container
//         LockAllButtonsExceptFirst(damageButtons);
//         LockAllButtonsExceptFirst(healthButtons);
//         LockAllButtonsExceptFirst(goldButtons);

//         // Update the initial upgrade cost text after setting up indices
//         UpdateInitialUpgradeCostText();

//         UpdateGoldDisplay(); // Update the gold display on Awake
//     }

//     // A method to update the initial upgrade cost text on all buttons
//     private void UpdateInitialUpgradeCostText()
//     {
//         SetInitialUpgradeCostText(damageButtons);
//         SetInitialUpgradeCostText(healthButtons);
//         SetInitialUpgradeCostText(goldButtons);
//     }

//     // A method to lock all buttons except the first one
//     private void LockAllButtonsExceptFirst(Button[] buttons)
//     {
//         for (int i = 1; i < buttons.Length; i++)
//         {
//             buttons[i].interactable = false;
//             Image imageComponent = buttons[i].transform.Find("Image_Forest").GetComponent<Image>();
//             Color color = imageComponent.color;
//             color.a = 0.5f;
//             imageComponent.color = color;
//         }
//     }

//     // A method to handle button clicks
//     public void HandleButtonClick(Button button)
//     {
//         //cost of the upgrade
//         int cost = GetUpgradeCost(button);

//         if (playerGold >= cost)
//         {
//             playerGold -= cost;
//             UpdateGoldDisplay(); // Update the UI to show the new gold amount

//             if (ArrayContains(damageButtons, button))
//             {
//                 Debug.Log("Damage Button " + damageButtonIndex);
//                 UnlockNextButton(damageButtons, ref damageButtonIndex);
                
//             }
//             else if (ArrayContains(healthButtons, button))
//             {
//                 Debug.Log("Health Button " + healthButtonIndex);
//                 UnlockNextButton(healthButtons, ref healthButtonIndex);
//                 player.TestSkillTree();
//             }
//             else if (ArrayContains(goldButtons, button))
//             {
//                 Debug.Log("Extra Button " + goldButtonIndex);
//                 UnlockNextButton(goldButtons, ref goldButtonIndex);
//                 player.TestSkillTree();
//             }
//         }
//         else
//         {
//             // Player doesn't have enough gold, handle accordingly (e.g., show a message)
//             Debug.Log("Not enough gold to buy this upgrade!");
//         }
//     }

//     //Safety check to see if the array isn't Null
//     // A utility method to check if an array contains an element
//     private bool ArrayContains(Button[] array, Button element)
//     {
//         return System.Array.IndexOf(array, element) != -1;
//     }

//     // A method to unlock the next button in the array
//     private void UnlockNextButton(Button[] buttons, ref int index)
//     {
//         if (index < buttons.Length - 1)
//         {
//             buttons[index].interactable = false;
//             index++;
//             buttons[index].interactable = true;

//             //Change the other buttons alpha back to normal
//             Image imageComponent = buttons[index].transform.Find("Image_Forest").GetComponent<Image>();
//             Color color = imageComponent.color;
//             color.a = 1.0f;
//             imageComponent.color = color;
//         }
//     }

//     // A method to get the cost of the upgrade based on the button clicked
//     private int GetUpgradeCost(Button button)
//     {
//         int upgradeIndex = GetUpgradeIndex(button);
//         return baseUpgradeCost + (upgradeIndex * 100);
//     }

//     // A utility method to get the upgrade index based on the button clicked
//     private int GetUpgradeIndex(Button button)
//     {
//         if (ArrayContains(damageButtons, button))
//         {
//             return damageButtonIndex;
//         }
//         else if (ArrayContains(healthButtons, button))
//         {
//             return healthButtonIndex;
//         }
//         else if (ArrayContains(goldButtons, button))
//         {
//             return goldButtonIndex;
//         }

//         return 0; // Default to 0 if the button is not found in any array
//     }

//     // A method to set the initial upgrade cost text on all buttons GoldRequired 
//     private void SetInitialUpgradeCostText(Button[] buttons)
//     {
//         for (int i = 0; i < buttons.Length; i++)
//         {
//             // Find the GoldRequired Text component in the button
//             TextMeshProUGUI goldRequiredText = buttons[i].transform.Find("Gold_Required").GetComponent<TextMeshProUGUI>();

//             // Get the upgrade index for the current button
//             int upgradeIndex = i;  // Use the button index as the upgrade index for initialization

//             // Get the upgrade cost for the current button
//             int cost = baseUpgradeCost + (upgradeIndex * 100);

//             // Set the GoldRequired text with the correct upgrade cost
//             goldRequiredText.text = "Gold Required: " + cost.ToString();
//         }
//     }

//     private void UpdateGoldDisplay()
//     {
//         UpdateGoldText(); // Update the general gold display
//     }

//     // A method to update the general gold display
//     private void UpdateGoldText()
//     {
//         // Find the gold_counter GameObject in the scene
//         //GameObject goldCounter = GameObject.Find("Gold_Counter");

//         if (goldCounter != null)
//         {
//             // Assuming you have a reference to the Gold Text component
//             TextMeshProUGUI goldText = goldCounter.transform.Find("Text_Value").GetComponent<TextMeshProUGUI>();

//             // Update the gold text with the current playerGold value
//             goldText.text = playerGold.ToString();
//         }
//         else
//         {
//             Debug.LogError("Text_Value not found in the scene!");
//         }
//     }
// }