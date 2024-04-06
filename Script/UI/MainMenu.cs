using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    void Awake()
    {
        newGameBtn = transform.GetChild(01).GetComponent<Button>();
        continueBtn = transform.GetChild(02).GetComponent<Button>();
        quitBtn = transform.GetChild(03).GetComponent<Button>();


        newGameBtn = onClick.AddListener(NewGame);
        continueBtn = onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
    }
    void NewGame()//开始新游戏
    {
        PlayerPrefs.DeleteAll();//删除所有存档
        SceneController.Instance.TransitionToFirstLevel();//跳转到第一关
    }
    void ContinueGame()//继续游戏
    {
        SceneController.Instance.TransitionToLoadGame();//跳转到载入存档界面
    }
    void QuitGame()//退出游戏
    {
        Application.Quit();//退出游戏
        Debug.Log("Quit Game");
    }

}
