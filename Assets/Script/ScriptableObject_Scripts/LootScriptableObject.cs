using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loots", menuName = "Loots", order = 1)]
public class LootScriptableObject : ScriptableObject
{
    public string info;
    public string name;
    public Sprite Icon;
    public Sprite Frame;

    
    public int ID; //0 = HP, 1 = Attack, 2 = Armor
    
    public float multiplier; //Brown: 5%, Green: 10%, Blue: 15%, Purple: 20%, Red: 25%, Gold: 40%, Glow: 50%

}
