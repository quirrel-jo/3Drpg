using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    private AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAnimator;

    [HideInInspector]
    public bool isCritical;

    void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;

    }

    #region Read from Data_SO
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }

    #endregion// Start is called before the first frame update

    #region Character Combat
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:update ui

        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //TODO: 经验update
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.killPoint);
        Debug.Log(defener.name + "正在扣血，伤害值为：" +attacker.name + "的" + damage + "，当前血量：" + defener.CurrentHealth);
    }

     public void TakeDamage(int damage, CharacterStats defener)
    {
        int currentDamge = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamge, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
            GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);

        Debug.Log(defener.name + "正在扣血，伤害值为：" +damage + "，当前血量：" + defener.CurrentHealth);
    }
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }
        return (int)coreDamage;
    }
    #endregion
}
