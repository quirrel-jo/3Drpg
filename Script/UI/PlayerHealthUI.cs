using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    TextMeshProUGUI levelText;
    Image healthSlider;
    Image expSlider;

    void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image >();
    }
    void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playerStats.characterData.currentlevel.ToString("00");
        UpdateHealth();
        UpdateExp();
        //Debug.Log("当前生命值为："+ GameManager.Instance.playerStats.CurrentHealth +"当前经验值为"+GameManager.Instance.playerStats.characterData.currentExp);
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
     void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats. characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
    
}
