using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_SwitchMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ChestPacksMenu;
    [SerializeField] private GameObject GoldPacksMenu;
    [SerializeField] private GameObject GemPacksMenu;
    [SerializeField] private GameObject GearPacksMenu;

    public void enableChest()
    {
        ChestPacksMenu.SetActive(true);
    }

    public void enableGold()
    {
        GoldPacksMenu.SetActive(true);
    }

    public void enableGem()
    {
        GemPacksMenu.SetActive(true);
    }

    public void enableGear()
    {
        GearPacksMenu.SetActive(true);
    }

    public void disableChest()
    {
        ChestPacksMenu.SetActive(false);
    }

    public void disableGold()
    {
        GoldPacksMenu.SetActive(false);
    }

    public void disableGem()
    {
        GemPacksMenu.SetActive(false);
    }

    public void disableGear()
    {
        GearPacksMenu.SetActive(false);
    }
}
