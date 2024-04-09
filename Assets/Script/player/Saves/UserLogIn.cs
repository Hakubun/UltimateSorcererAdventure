using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserLogIn 
{
    public string username;
    public string pw;

    public UserLogIn(string _username, string _pw)
    {
        username = _username;
        pw = _pw;
    }
}
