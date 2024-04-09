using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int exp;
    public int lvl;
    public int req;

    public string UserName;
    private void Awake() {
        
    }

    public void UpdateLvl(int _exp, int _lvl, int _req)
    {

        exp = _exp;
        lvl = _lvl;
        req = _req;
        SaveSystem.SavePlayerEXP(exp, lvl, req);
        CloudSaveScript.instance.SaveUserEXP(exp, lvl, req);


    }

}
