using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene,
    }
 
    [Header("Transition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)//处于可以传送的情况并且按E键触发传送
        {
            //TODO: sceneController 传送
            SceneController.Instance.TransitionToDestination(this);//传送到目标点
        }
    }
    void OnTriggerStay(Collider other)//碰撞体进入触发器内
    {
        if (other.CompareTag("Player"))//如果碰撞体的tag是Player
            canTrans = true;//可以传送
    }

    void OnTriggerExit(Collider other)//碰撞体离开触发器内
    {
        if (other.CompareTag("Player"))//如果碰撞体的tag是Player
            canTrans = false;//不能传送

    }
}
