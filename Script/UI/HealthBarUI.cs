using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthBarUIPrefab;

    public Transform barPoint;
    public bool alwaysVisible;

    public float visibleTime;

    private float timeleft;

    Image healthSlider;//滑动血条 实际血量
    Transform UIbar;//和barpoint保持一致的看不到的坐标

    Transform cam;//摄像机位置 为了保持血条一直对着摄像机

    CharacterStats currentStats;

    void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;

    }

    void OnEnable()
    {
        cam = Camera.main.transform;//获取摄像机位置
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)//找到世界空间的canvas 因为只有血条是世界空间所以目前只有血条会被找到
            {
                UIbar = Instantiate(healthBarUIPrefab, canvas.transform).transform;//实例化血条
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();//找到血条的滑动条
                UIbar.gameObject.SetActive(alwaysVisible);//是否一直显示血条


            }

        }

    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIbar.gameObject);

        UIbar.gameObject.SetActive(true);
        timeleft = visibleTime;

        float sliderPercent = (float)currentHealth / (float)maxHealth;//血量百分比
        healthSlider.fillAmount = sliderPercent;//设置血量百分比   

    }

    void LateUpdate()
    {
        if(UIbar != null)
        {
            UIbar.position = barPoint.position;//保持血条和barpoint的位置一致
            //UIbar.forward = -cam.forward;//保持血条一直朝向摄像机
            UIbar.LookAt(cam);//保持血条一直对着摄像机

            if (timeleft <= 0 && !alwaysVisible) 
                UIbar.gameObject.SetActive(false);//血条消失
            else
                timeleft -= Time.deltaTime;
        }
    }
}
