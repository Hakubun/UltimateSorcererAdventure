using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine.UI;
using Unity.Services.Core;
using TMPro;

public class CloudSaveScript : MonoBehaviour
{
    public static CloudSaveScript instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public Text status;
    public InputField input;


    public async void Start()
    {
        await UnityServices.InitializeAsync();
    }


    #region Testing DO NOT USE!!!
    public async void SaveData()
    {
        var data = new Dictionary<string, object> { { "name", input.text } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async void LoadData()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "name" });

        if (serverData.ContainsKey("name"))
        {
            input.text = serverData["name"];
        }
        else
        {
            // Debug.Log("not found");
        }

    }

    public async void SavePlayerFiles()
    {
        byte[] file = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/playerHP.save");
        await CloudSaveService.Instance.Files.Player.SaveAsync("hp", file);
    }
    #endregion


    #region PlayerEXP
    public async void SaveUserEXP(int exp, int lvl, int req)
    {
        var data = new Dictionary<string, object> { { "exp", exp }, { "Level", lvl }, { "Requirement", req } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }


    public async Task<PlayerEXP> LoadUser()
    {
        // Specify the keys to load in the HashSet
        HashSet<string> keysToLoad = new HashSet<string> { "exp", "Level", "Requirement" };

        // Load data from the cloud using the specified keys
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(keysToLoad);

        // Retrieve the values for each key, or set to default values if the key is not present
        int exp = serverData.ContainsKey("exp") ? int.Parse(serverData["exp"]) : 0;
        int level = serverData.ContainsKey("Level") ? int.Parse(serverData["Level"]) : 1;
        int requirement = serverData.ContainsKey("Requirement") ? int.Parse(serverData["Requirement"]) : 500;


        return new PlayerEXP(exp, level, requirement);
    }

    #endregion
    #region UserName
    public async void SaveUserName(string name){
        var data = new Dictionary<string, object> { { "username", name } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    #endregion
    #region Player Coin
    public async void SaveUserCoin(int coin)
    {
        var data = new Dictionary<string, object> { { "coin", coin } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<int> LoadUserCoin()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "coin" });

        int coin = serverData.ContainsKey("coin") ? int.Parse(serverData["coin"]) : 0;

        return coin;
    }
    #endregion

    #region Player Gem
    public async void SaveUserGem(int gem)
    {
        var data = new Dictionary<string, object> { { "gem", gem } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<int> LoadUserGem()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "gem" });

        int gem = serverData.ContainsKey("gem") ? int.Parse(serverData["gem"]) : 0;

        return gem;
    }
    #endregion

    #region Player HP
    public async void SavePlayerHP(float hp)
    {
        var data = new Dictionary<string, object> { { "hp", hp } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<float> LoadPlayerHP()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "hp" });

        float hp = serverData.ContainsKey("hp") ? float.Parse(serverData["hp"]) : 100f;

        return hp;
    }

    #endregion

    #region Player Damage
    public async void SavePlayerDamage(float dmg)
    {
        var data = new Dictionary<string, object> { { "Damage", dmg } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<float> LoadPlayerDamage()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "Damage" });

        float dmg = serverData.ContainsKey("Damage") ? float.Parse(serverData["Damage"]) : 10f;

        return dmg;
    }

    #endregion

    #region Player Armor
    public async void SavePlayerArmor(float armor)
    {
        var data = new Dictionary<string, object> { { "Armor", armor } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<float> LoadPlayerArmor()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "Armor" });

        float amr = serverData.ContainsKey("Armor") ? float.Parse(serverData["Armor"]) : 0f;

        return amr;
    }


    #endregion

    #region ExtraLife
    public async void SaveExtraLife(int num)
    {
        var data = new Dictionary<string, object> { { "ExtraLife", num } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<int> LoadExtraLife()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "ExtraLife" });

        int num = serverData.ContainsKey("ExtraLife") ? int.Parse(serverData["ExtraLife"]) : 0;

        return num;
    }
    #endregion
    #region GoldMultiplier
    public async void SaveMultiplier(int num)
    {
        var data = new Dictionary<string, object> { { "CoinMultiplier", num } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }

    public async Task<int> LoadMultiplier()
    {
        Dictionary<string, string> serverData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "CoinMultiplier" });

        int num = serverData.ContainsKey("CoinMultiplier") ? int.Parse(serverData["CoinMultiplier"]) : 0;

        return num;
    }

    #endregion
}
