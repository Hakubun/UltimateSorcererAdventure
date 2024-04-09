using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int levelIndex;
    public GameObject lockIcon;
    public GameObject lvlText;
    [SerializeField] private bool locked;
    [SerializeField] private StatusBarUI energyScript;
    [SerializeField] private int lvlRequirment;

    [SerializeField] private int eneryRequirment;
    [SerializeField] private Button _self;
    [SerializeField] private loadingscene loading;

    private void Start()
    {
        _self = this.gameObject.GetComponent<Button>();
    }

    private async void OnEnable()
    {
        PlayerEXP data = await SaveSystem.LoadPlayerEXP();
        int playerLvl = data.lvl;
        if (playerLvl >= lvlRequirment)
        {
            lockIcon.SetActive(false);
            lvlText.SetActive(true);
            locked = false;
        }
        else
        {
            locked = true;
            lockIcon.SetActive(true);
            lvlText.SetActive(false);
        }

        if (energyScript.getEnergy() >= eneryRequirment && !locked)
        {
            _self.enabled = true;
        }
        else
        {
            // if (levelIndex == 2)
            // {
            //     _self.interactable = true;
            // }
            // else
            // {

            // }
            _self.enabled = false;
        }


    }


    public void MoveToScene()
    {
        //SceneManager.LoadScene(levelIndex);
        loading.LoadScene(levelIndex);
    }
}
