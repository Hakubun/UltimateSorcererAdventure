using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

public static class SaveSystem
{


    #region UserLogin
    public static void SaveLogIn(string _username, string _pw)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/LogIn.info";
        FileStream stream = new FileStream(path, FileMode.Create);

        UserLogIn data = new UserLogIn(_username, _pw);

        formatter.Serialize(stream, data);

        stream.Close();

        //CloudSaveScript.instance.SaveUserName(_username);
    }

    public static UserLogIn LoadLogIn()
    {
        string path = Application.persistentDataPath + "/LogIn.info";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UserLogIn data = formatter.Deserialize(stream) as UserLogIn;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion
    #region HP
    public static void SavePlayerHP(float hp)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playerHP.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerHp data = new PlayerHp(hp);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SavePlayerHP(hp);
    }

    public static async Task<float> LoadPlayerHP()
    {
        string path = Application.persistentDataPath + "/playerHP.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerHp data = formatter.Deserialize(stream) as PlayerHp;
            stream.Close();
            return data.Health;
        }
        else
        {
            float _hp = await CloudSaveScript.instance.LoadPlayerHP();

            SavePlayerHP(_hp);
            
            return _hp;
        }
    }

    #endregion
    #region Damage
    public static void SavePlayerDmg(float dmg)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playerDmg.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDmg data = new PlayerDmg(dmg);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SavePlayerDamage(dmg);
    }

    public static async Task<float> LoadPlayerDmg()
    {
        string path = Application.persistentDataPath + "/playerDmg.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerDmg data = formatter.Deserialize(stream) as PlayerDmg;
            stream.Close();
            return data.Damage;
        }
        else
        {
            float _dmg = await CloudSaveScript.instance.LoadPlayerDamage();
            SavePlayerDmg(_dmg);
            
            return _dmg;
        }
    }
    #endregion
    #region Armor
    public static void SavePlayerArmor(float Armor)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playerArmor.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerArmor data = new PlayerArmor(Armor);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SavePlayerArmor(Armor);
    }

    public static async Task<float> LoadPlayerArmor()
    {
        string path = Application.persistentDataPath + "/playerArmor.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerArmor data = formatter.Deserialize(stream) as PlayerArmor;
            stream.Close();
            return data.Armor;
        }
        else
        {
            float _armor = await CloudSaveScript.instance.LoadPlayerArmor();
            SavePlayerArmor(_armor);
            
            return _armor;
        }
    }

    #endregion
    #region ExtraLife
    public static void SaveExtraLife(int count)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/LifeCount.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ExtraLifeCount data = new ExtraLifeCount(count);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SaveExtraLife(count);
    }

    public static async Task<int> LoadExtraLife()
    {
        string path = Application.persistentDataPath + "/LifeCount.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ExtraLifeCount data = formatter.Deserialize(stream) as ExtraLifeCount;
            stream.Close();
            return data.lifeCount;
        }
        else
        {
            int _count = await CloudSaveScript.instance.LoadExtraLife();
            SaveExtraLife(_count);
            
            return _count;
        }
    }

    #endregion
    #region CoinMultiplyer
    public static void SaveGoldMultiplier(int count)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/GoldMultiplierCount.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        GoldMultiplier data = new GoldMultiplier(count);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SaveMultiplier(count);
    }

    public static async Task<int> LoadGoldMultiplier()
    {
        string path = Application.persistentDataPath + "/GoldMultiplierCount.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GoldMultiplier data = formatter.Deserialize(stream) as GoldMultiplier;
            stream.Close();
            return data.Count;
        }
        else
        {
            int _count = await CloudSaveScript.instance.LoadMultiplier();
            SaveGoldMultiplier(_count);
            return _count;
        }
    }

    #endregion
    #region UserEXP
    public static void SavePlayerEXP(int _exp, int _lvl, int _req)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/PlayerEXP.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerEXP data = new PlayerEXP(_exp, _lvl, _req);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static async Task<PlayerEXP> LoadPlayerEXP()
    {
        string path = Application.persistentDataPath + "/PlayerEXP.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerEXP data = formatter.Deserialize(stream) as PlayerEXP;
            stream.Close();
            return data;
        }
        else
        {
            PlayerEXP data = await CloudSaveScript.instance.LoadUser();
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);

            stream.Close();

            return data;
        }
    }
    #endregion
    #region Coin
    public static void SaveCoin(int coinNum)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/coin.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        CoinData data = new CoinData(coinNum);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SaveUserCoin(coinNum);
    }

    public static async Task<int> LoadCoin()
    {
        string path = Application.persistentDataPath + "/coin.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CoinData data = formatter.Deserialize(stream) as CoinData;
            stream.Close();
            return data.coins;
        }
        else
        {
            int coin = await CloudSaveScript.instance.LoadUserCoin();
            SaveCoin(coin);
            return coin;
        }
    }
    #endregion
    #region Gem
    public static void SaveGem(int gemNum)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/gem.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        GemData data = new GemData(gemNum);

        formatter.Serialize(stream, data);

        stream.Close();

        CloudSaveScript.instance.SaveUserGem(gemNum);
    }

    public static async Task<int> LoadGem()
    {
        string path = Application.persistentDataPath + "/gem.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GemData data = formatter.Deserialize(stream) as GemData;
            stream.Close();
            return data.gems;
        }
        else
        {
            int gem = await CloudSaveScript.instance.LoadUserGem();
            SaveGem(gem);
            return gem;
        }
    }
    #endregion
    #region Skin
    public static void SavePlayerCustom(playerCustom player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/playerCustom.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerCustomData data = new PlayerCustomData(player);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerCustomData LoadPlayerCustom()
    {
        string path = Application.persistentDataPath + "/playerCustom.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerCustomData data = formatter.Deserialize(stream) as PlayerCustomData;
            stream.Close();
            return data;
        }
        else
        {
            playerCustom player = GameObject.FindWithTag("Player").GetComponent<playerCustom>();
            SavePlayerCustom(player);
            PlayerCustomData data = new PlayerCustomData(player);
            return data;
        }
    }
    #endregion
}
