using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject HomeMenu;
    [SerializeField] GameObject OptionMenu;
    [SerializeField] GameObject LevelSelection;
    [SerializeField] GameObject ShopMenu;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject SkillTree;
    [SerializeField] GameObject IAPMenu;
    [SerializeField] private loadingscene loading;
    [SerializeField] private Vector3 HomeMenuPosition;
    [SerializeField] private Vector3 SkillTreePosition;
    [SerializeField] private Vector3 InventoryPosition;
    [SerializeField] private Vector3 LevelSelectionPosition;
    [SerializeField] private Vector3 ShopPosition;
    [SerializeField] private MainMenu_Camera _camera;

    // Start is called before the first frame update
    private void Awake()
    {
        HomeMenu.SetActive(true);
        OptionMenu.SetActive(false);
        LevelSelection.SetActive(false);
        ShopMenu.SetActive(false);
        Inventory.SetActive(false);
        SkillTree.SetActive(false);
        IAPMenu.SetActive(false);

        Time.timeScale = 1;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void StartLevel(int level)
    {
        //SceneManager.LoadScene(level);
        loading.LoadScene(level);
    }

    public void SelectLevel()
    {
        _camera.MoveTo(LevelSelectionPosition);
        HomeMenu.SetActive(false);
        LevelSelection.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        _camera.MoveTo(HomeMenuPosition);
        LevelSelection.SetActive(false);
        HomeMenu.SetActive(true);
    }


    public void closeOption()
    {
        OptionMenu.SetActive(false);
    }

    public void openOption()
    {
        OptionMenu.SetActive(true);
    }

    public void openShop()
    {

        _camera.MoveTo(ShopPosition);
        HomeMenu.SetActive(false);
        ShopMenu.SetActive(true);
        OptionMenu.SetActive(false);
        LevelSelection.SetActive(false);
        Inventory.SetActive(false);
        SkillTree.SetActive(false);
        IAPMenu.SetActive(false);
    }

    public void openIAP()
    {
        _camera.MoveTo(ShopPosition);
        HomeMenu.SetActive(false);
        ShopMenu.SetActive(false);
        OptionMenu.SetActive(false);
        LevelSelection.SetActive(false);
        Inventory.SetActive(false);
        SkillTree.SetActive(false);
        IAPMenu.SetActive(true);
        ShopMenu.SetActive(false);

    }
    public void closeIAP()
    {
        _camera.MoveTo(HomeMenuPosition);
        IAPMenu.SetActive(false);
        HomeMenu.SetActive(true);
    }

    public void closeShop()
    {
        _camera.MoveTo(HomeMenuPosition);
        ShopMenu.SetActive(false);
        HomeMenu.SetActive(true);
    }

    public void openInventory()
    {

        _camera.MoveTo(InventoryPosition);
        HomeMenu.SetActive(false);
        Inventory.SetActive(true);
        //Inventory.SetActive(true);
    }

    public void closeInventory()
    {
        Inventory.SetActive(false);
        _camera.MoveTo(HomeMenuPosition);
        HomeMenu.SetActive(true);
    }

    public void OpenSkillTree()
    {
        _camera.MoveTo(SkillTreePosition);
        HomeMenu.SetActive(false);
        SkillTree.SetActive(true);
    }

    public void CloseSkillTree()
    {
        SkillTree.SetActive(false);
        _camera.MoveTo(HomeMenuPosition);
        HomeMenu.SetActive(true);
    }
}
