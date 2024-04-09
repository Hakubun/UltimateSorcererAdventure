using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeLoadScreenBehavior : MonoBehaviour
{
    public GameObject[] panels;
    void OnEnable()
    {
        //randomly pick one of the panels to be active
        // Ensure there are panels to choose from
        if (panels.Length > 0)
        {
            // Deactivate all panels first
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }

            // Randomly pick one panel to activate
            int randomIndex = Random.Range(0, panels.Length);
            panels[randomIndex].SetActive(true);
        }
    }
}
