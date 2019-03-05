using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class NeutralAI : MonoBehaviour
{
    protected Vector3 startPoint;
    protected MonsterCtl monsterProp;

    protected NavMeshAgent navMeshAgent;

    protected AIStatusType AIType = AIStatusType.AIT_Idle;

    // Use this for initialization
    void Start ()
	{
        startPoint = this.transform.position;
        monsterProp = this.GetComponent<MonsterCtl>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update ()
	{
        if (this.monsterProp.MyProp.curHP <= 0)
        {
            navMeshAgent.isStopped = true;

            return;
        }
        
        switch (AIType)
        {
            case AIStatusType.AIT_Idle:
                {
                    onAIType_Idle();
                    break;
                }
            case AIStatusType.AIT_Attack:
                {
                    onAIType_Attack();
                    break;
                }
            case AIStatusType.AIT_Chase:
                {
                    onAIType_Chase();
                    break;
                }
            case AIStatusType.AIT_Back:
                {
                    onAIType_Back();
                    break;
                }
            default: break;
        }

    }

    void onAIType_Idle()
    {
        if(monsterProp.enemy != null )
        {
            //切换状态
            AIType = AIStatusType.AIT_Chase;

            //确认动画状态
            if (monsterProp.MyProp.CurStatus != StatusType.ST_Run)
            {
                monsterProp.UpdataAnimation(StatusType.ST_Run);
            }
        }
    }

    void onAIType_Chase()
    {
        //确认敌人是否存在
        if (CheckEnemy() == false)
        {
            //切换状态
            AIType = AIStatusType.AIT_Back;

            return;
        }

        //检测 离开原点的距离
        Vector3 curPos = this.transform.position;  //当前自己的位置

        //计算 距离
        float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(startPoint.x, startPoint.z));

        if(distance > GameDefine.NeutralChaseRadius)
        {
            //切换状态
            AIType = AIStatusType.AIT_Back;

            return;
        }

        //确认攻击范围
        var radius = monsterProp.MyProp.attackRadius;

        //检测 与目标的距离
        Vector3 destPos = monsterProp.enemy.transform.position;  //目标位置
        distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

        if(distance <= radius)
        {
            //切换状态
            AIType = AIStatusType.AIT_Attack;

            return;
        }

        //开始移动
        navMeshAgent.isStopped = false;

        //追击敌人
        navMeshAgent.destination = monsterProp.enemy.transform.position;

        //确认动画状态
        if (monsterProp.MyProp.CurStatus != StatusType.ST_Run)
        {
            monsterProp.UpdataAnimation(StatusType.ST_Run);
        }
    }

    void onAIType_Attack()
    {
        //确认敌人存在
        if (CheckEnemy() == false)
        {
            //切换状态
            AIType = AIStatusType.AIT_Back;

            //切换动画
            monsterProp.UpdataAnimation(StatusType.ST_Run);

            return;
        }
        
        //确认攻击范围
        var radius = monsterProp.MyProp.attackRadius;

        //检测 与目标的距离
        Vector3 curPos = this.transform.position;  //当前自己的位置
        Vector3 destPos = monsterProp.enemy.transform.position;  //目标位置

        //计算当前位置 和 敌人 的距离
        float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

        //敌人离开攻击范围
        if (distance > radius)  //超过攻击范围
        {
            //切换状态 
            AIType = AIStatusType.AIT_Chase;

            //切换动画
            monsterProp.UpdataAnimation(StatusType.ST_Run);

            return;
        }
        else
        {
            if ((int)monsterProp.MyProp.CurStatus < (int)StatusType.ST_NoBreak)
            {
                //停止移动
                navMeshAgent.isStopped = true;

                //攻击敌人
                monsterProp.UpdataAnimation(StatusType.ST_Attack01);
            }
        }
    }

    bool isCompleteBack = true;

    void onAIType_Back()
    {
        if(isCompleteBack ==false)
        {
            return;
        }

        isCompleteBack = false;

        //
        monsterProp.enemy = null;
        
        //开始移动
        navMeshAgent.isStopped = false;

        //返回出生地
        navMeshAgent.destination = startPoint;

        //确认动画状态
        if (monsterProp.MyProp.CurStatus != StatusType.ST_Run)
        {
            monsterProp.UpdataAnimation(StatusType.ST_Run);
        }

        //协程 确认返回完成
        StartCoroutine(checkCompleteBack());
    }

    IEnumerator checkCompleteBack()
    {
        while(true)
        {
            //检测 离开原点的距离
            Vector3 curPos = this.transform.position;  //当前自己的位置

            //计算 距离
            float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(startPoint.x, startPoint.z));

            if (distance < 0.1f)
            {
                //切换状态
                AIType = AIStatusType.AIT_Idle;

                //更新动画
                monsterProp.UpdataAnimation(StatusType.ST_Idle);

                //停止导航
                navMeshAgent.isStopped = true;
                this.transform.position = startPoint;

                //完成返回
                isCompleteBack = true;

                break;
            }

            //返回的时候一直回血
            monsterProp.changeHP(100);

            yield return null;
        }
    }

    bool CheckEnemy()
    {
        if (monsterProp == null)
        {
            return false;
        }

        if (monsterProp.enemy == null)
        {
            return false;
        }

        if (monsterProp.enemy.MyProp.curHP <= 0)
        {
            return false;
        }

        return true;
    }
}
