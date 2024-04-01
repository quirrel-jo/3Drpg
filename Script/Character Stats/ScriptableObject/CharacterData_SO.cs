using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewData", menuName = "Character Stats/Data", order = 1)]
public class CharacterData_SO : ScriptableObject
{
    // Start is called before the first frame update
    [Header("Stats Info")]
    public int maxHealth;

    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentlevel;
    public int maxlevel;
    public int baseExp;
    public int currentExp;
    public float LevelBuff;

    public float LevelMultiplier
    {
        get { return 1 + (currentlevel - 1) * LevelBuff; }

    }
    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            Levelup();

    }

    private void Levelup()
    {
        //所有想提升的数据都在这里
        currentlevel = Mathf.Clamp(currentlevel + 1, 0, maxlevel);

        baseExp = (int)(baseExp * LevelMultiplier);
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;

        Debug.Log("Level up to " + currentlevel + "Max Health: " + maxHealth);
    }
}
