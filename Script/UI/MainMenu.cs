using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;
    PlayableDirector director;
    void Awake()
    {
        newGameBtn = transform.GetChild(01).GetComponent<Button>();
        continueBtn = transform.GetChild(02).GetComponent<Button>();
        quitBtn = transform.GetChild(03).GetComponent<Button>();


        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);//绑定按钮事件


        director = FindObjectOfType<PlayableDirector>();//获取播放器
        director.stopped += NewGame;//当播放器停止时，调用NewGame()
    }
    
    void PlayTimeLine()//播放时间轴
    {
        director.Play();//播放时间轴
    }
    void NewGame(PlayableDirector obj)//开始新游戏
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
