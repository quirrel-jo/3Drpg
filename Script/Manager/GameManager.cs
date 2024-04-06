using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    private CinemachineFreeLook followCamera;
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

     protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);//Dont destroy the game manager when loading a new scene
    }
    public void RegisterPlayer(CharacterStats player)//注册玩家
    {
        playerStats = player;//Set the player stats
        followCamera = FindObjectOfType<CinemachineFreeLook>();//Find the follow camera
        if(followCamera!= null)
        {
            followCamera.Follow = playerStats.transform.GetChild(2);//Follow the player
            followCamera.LookAt = playerStats.transform.GetChild(2);
        }
    }

    public void AddObserve(IEndGameObserver observer)//注册观察者
    {
        endGameObservers.Add(observer);//Add the observer to the list of observers
    }
    public void RemoveObserver(IEndGameObserver observer)//注销观察者
    {
        endGameObservers.Remove(observer);
    }

    public void NoitfyObserves()//通知观察者
    {
        foreach(var observer in endGameObservers)//遍历所有观察者
        {
            observer.EndNotify();//通知观察者
        }
    }

    public Transform GetEntrance()//获取入口
    {
        foreach(var item in FindObjectsOfType<TransitionDestination>())//遍历所有TransitionDestination
        {
            if(item.destinationTag == TransitionDestination.DestinationTag.Enter)//找到入口
                return item.transform;//返回入口Transform
        }
        return null;
    }
}
