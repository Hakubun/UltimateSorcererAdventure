using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades", order = 1)]
public class UpgradeScriptableObject : ScriptableObject
{
    public string info;
    public Sprite Icon;
    public Sprite Frame;
    public int ID;
    public int price;
    public float multiplier;
}

