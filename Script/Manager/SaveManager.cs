using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "level";//场景名称
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//按下ESC键
        {
           SceneController.Instance.TranstionToMain();//转到主界面
        }
        if (Input.GetKeyDown(KeyCode.S))//按下S键
        {
            SavePlayerData();//保存玩家数据
        }
        if (Input.GetKeyDown(KeyCode.L))//按下L键
        {
            LoadPlayerData();//加载玩家数据
        }
    }
    public void SavePlayerData()//保存玩家数据
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);//保 存玩家数据
    }
    public void LoadPlayerData()//加载玩家数据
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);//保 存玩家数据
    }
    public void Save(Object data, string key)//保存数据
    {
        var jsonData = JsonUtility.ToJson(data, true);//将数据转化为json格式,并格式化变得好看
        PlayerPrefs.SetString(key, jsonData);//保存到PlayerPrefs      
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);//保存场景名称  
        PlayerPrefs.Save();//保存到硬盘
    }



    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))//如果PlayerPrefs中有数据
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);//将json格式的数据转化为数据类型并覆盖原数据
        }
    }
}
