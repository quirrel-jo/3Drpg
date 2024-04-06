using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterStats characterStats;
    private GameObject attackTarget;
    private float lastAttackTime;

    private bool isDead;
    private float stopDistance;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();//获取导航组件
        anim = GetComponent<Animator>();//获取动画组件
        characterStats = GetComponent<CharacterStats>();//获取角色状态组件
        stopDistance = agent.stoppingDistance;//获取停止距离

    }
    void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;//注册事件
        MouseManager.Instance.OnEnemyClicked += EventAttack;//注册事件
        GameManager.Instance.RegisterPlayer(characterStats);//注册玩家
    }
    void Start()
    {
        
        SaveManager.Instance.LoadPlayerData();//加载玩家数据    

    }

    void OnDisable()
    {
        if(!MouseManager.IsInitialized) return;//如果鼠标管理器未初始化则不注销事件
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;//注销事件
        MouseManager.Instance.OnEnemyClicked -= EventAttack;//注销事件
    }



    void Update()
    {
        if (GameManager.Instance.playerStats.characterData != null)//
        isDead = characterStats.CurrentHealth == 0;//角色血量为0时角色死亡
        if (isDead)
            GameManager.Instance.NoitfyObserves();//如果角色死亡则通知观察者角色死亡
        SwitchAnimation();//切换动画

        lastAttackTime -= Time.deltaTime;//减少上次攻击时间
        //Debug.Log("PLAYER HEALTH: "+characterStats.CurrentHealth);
    }
    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }
    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
            StartCoroutine(MovetoAttackTarget());
        }
    }

    IEnumerator MovetoAttackTarget()
    {
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData.attackRange;

        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats.attackData.attackRange + attackTarget.GetComponent<NavMeshAgent>().radius)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;
        //Attack
        if (lastAttackTime < 0)
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
            lastAttackTime = characterStats.attackData.coolDown;
        }
    }
    //Animation Event
    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }

        }
        else
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }

    }
}
