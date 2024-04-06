using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.VisualScripting;

public class SceneController : Singleton<SceneController>
{
    public GameObject playerPrefab;
    GameObject player;
    NavMeshAgent playerAgent;

    override protected void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//保证场景切换后不销毁场景控制器
    }

  
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)//切换场景类型
        {
            case TransitionPoint.TransitionType.SameScene://同场景传送
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));//开始协程 从scenemanager获取当前场景名，并传给Transition方法
                break;
            case TransitionPoint.TransitionType.DifferentScene://不同场景传送
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));//开始协程 从TransitionPoint获取目标场景名和传送点标签，并传给Transition方法
                break;

        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        
        //TODO: 保存shuffle点的位置信息，以便下次传送时恢复
        SaveManager.Instance.SavePlayerData();//保存玩家数据

        if (SceneManager.GetActiveScene().name != sceneName)//如果当前场景名和目标场景名不一致
        {
            yield return SceneManager.LoadSceneAsync(sceneName);//加载目标场景 异步加载
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);////实例化玩家对象
            SaveManager.Instance.LoadPlayerData();//读取玩家数据
            yield break;
        }
        else//如果当前场景名和目标场景名一致
        {
            player = GameManager.Instance.playerStats.gameObject;//获取玩家对象
            playerAgent = player.GetComponent<NavMeshAgent>();//获取玩家的NavMeshAgent组件
            playerAgent.enabled = false;//关闭玩家的NavMeshAgent

            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);//设置玩家位置 从TransitionDestination获取传送点位置
            playerAgent.enabled = true;//传送完成后重新打开玩家的NavMeshAgent
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)//获取传送点位置
    {
        var entrances = FindObjectsOfType<TransitionDestination>();//获取所有TransitionDestination然后遍历
        for (int i = 0; i < entrances.Length; i++)//遍历所有TransitionDestination
        {
            if (entrances[i].destinationTag == destinationTag)//如果目标传送点标签匹配
                return entrances[i];//找到目标传送点返回
        }
        return null;
    }
      public void TranstionToMain()//  传送到main场景
    {
        StartCoroutine(LoadMain());//异步加载main场景
    }
     public void TransitionToLoadGame()//  传送到保存的场景
     {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));//异步加载当前场景
     }
    public void TransitionToFirstLevel()//  传送到第一个场景
    {
        StartCoroutine(LoadLevel("start"));//异步加载第一个场景
    }
    IEnumerator LoadLevel(string scene)//协程加载场景
    {
        if(scene != "")//如果场景名不为空
        {
            yield return SceneManager.LoadSceneAsync(scene);//异步加载场景
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation); //通过gamemanager获取入口位置，实例化玩家对象

        SaveManager.Instance.SavePlayerData();//保存玩家数据
        yield break;
        }
    }
    IEnumerator LoadMain()//协程加载main场景
    {
        yield return SceneManager.LoadSceneAsync("Main");//异步加载main场景
        yield break;
    }
}
