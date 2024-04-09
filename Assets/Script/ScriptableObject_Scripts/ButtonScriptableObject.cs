using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Button", menuName = "UpgradeButton", order = 1)]
public class ButtonScriptableObject : ScriptableObject
{
    public string _name;
    public int _skillID;
    public Sprite Icon;

    public string Description; // New field for skill description
}
