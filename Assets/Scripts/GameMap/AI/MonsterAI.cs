using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

//AI状态
public enum AIStatusType
{
    AIT_None,    // 
    AIT_Run,     // 正常沿路点行进
    AIT_Attack,  // 攻击状态
    AIT_Chase,   // 追击状态
    AIT_Back,    // 返回

    AIT_Idle,    // 等待
}

public class MonsterAI : MonoBehaviour 
{
    protected PathNode pathNode;
    protected Vector3 targetPoint;
    protected MonsterCtl monsterProp;

    protected AIStatusType AIType = AIStatusType.AIT_Run;
    protected NavMeshAgent navMeshAgent;
    
    //离开路点线路的位置
    protected Vector3 LeavvePathNode;

	// Use this for initialization
	void Start () 
	{
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        monsterProp = this.GetComponent<MonsterCtl>();
        monsterProp.UpdataAnimation(StatusType.ST_Idle);
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (this.monsterProp.MyProp.curHP <=0)
        {
            navMeshAgent.isStopped = true;

            return;
        }

        switch (AIType)
        {
            case AIStatusType.AIT_Run:
                {
                    onAIType_run();
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


    //按照状态封装  —————————— run
    void onAIType_run()
    {
        //检测是否有敌人进入视野
        var radius = monsterProp.MyProp.attackRadius;
        RaycastHit[] hitArray = Physics.SphereCastAll(this.transform.position, radius * 1.5f, Vector3.up);
        for (int i = 0; i < hitArray.Length; i++)
        {
            MonsterCtl go = hitArray[i].transform.GetComponent<MonsterCtl>();
            if (go != null && go.MyProp.campType != monsterProp.MyProp.campType && go.MyProp.curHP >0)
            {
                //添加敌人
                monsterProp.enemy = go;

                //切换状态
                AIType = AIStatusType.AIT_Chase;
                LeavvePathNode = this.transform.position;

                return;
            }
        }

        //没有敌人 正常移动
        MoveWithPathNode();

        //确认动画状态
        if(monsterProp.MyProp.CurStatus != StatusType.ST_Run)
        {
            monsterProp.UpdataAnimation(StatusType.ST_Run);
        }
    }

    //按照状态封装  —————————— Attack
    void onAIType_Attack()
    {
        //确认敌人存在 （还活着）
        if (CheckEnemy()==false)
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
            if((int)monsterProp.MyProp.CurStatus <(int)StatusType.ST_NoBreak)
            {
                //停止移动
                navMeshAgent.isStopped = true;

                //看向目标点
                this.transform.LookAt(new Vector3(destPos.x, curPos.y, destPos.z));
                
                //攻击敌人
                monsterProp.UpdataAnimation(StatusType.ST_Attack01);
            }
        }
    }

    //按照状态封装  —————————— Back
    void onAIType_Back()
    {
        //检测是否回到离开路线的点
        Vector3 curDistance = targetPoint - this.transform.position;
        Vector3 preDistance = targetPoint - LeavvePathNode;

        if (curDistance.magnitude > preDistance.magnitude)
        {
            //回到离开路点线路时的坐标点
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = LeavvePathNode;

            if((this.transform.position-LeavvePathNode).magnitude < 0.1f )
            {
                AIType = AIStatusType.AIT_Run;
                navMeshAgent.isStopped = false;
                navMeshAgent.destination = targetPoint;
            }
        }
        else
        {
            //直接向路点前进
            AIType = AIStatusType.AIT_Run;
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = targetPoint;
        }

        //确认动画状态
        if (monsterProp.MyProp.CurStatus != StatusType.ST_Run)
        {
            monsterProp.UpdataAnimation(StatusType.ST_Run);
        }
    }

    //按照状态封装  —————————— Chase
    void onAIType_Chase()
    {
        //确认敌人存在
        if (CheckEnemy()==false)
        {
            //切换状态
            AIType = AIStatusType.AIT_Back;

            return;
        }

        //确认攻击范围
        var radius = monsterProp.MyProp.attackRadius;

        //检测 与目标的距离
        Vector3 curPos = this.transform.position;  //当前自己的位置
        Vector3 destPos = monsterProp.enemy.transform.position;  //目标位置

        //计算当前位置 和 敌人 的距离
        float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

        if (distance > radius * 1.5f)  //超过视野范围
        {
            //切换状态 
            AIType = AIStatusType.AIT_Back;

            return;
        }
        else if (distance < radius)  //进入攻击范围
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

    bool CheckEnemy()
    {
        if(monsterProp==null)
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

    void MoveWithPathNode()
    {
        Vector3 curPos = this.transform.position;  //当前自己的位置

        //计算当前位置 和 最新目标路点 的距离
        float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(targetPoint.x, targetPoint.z));

        if (distance < 0.5f)
        {
            if (monsterProp.MyCampType == CampType.CT_Blue)
            {
                if (pathNode.BuleNextNode != null)
                {
                    SetTargetPoint(pathNode.BuleNextNode);
                }
            }
            else if(monsterProp.MyCampType == CampType.CT_Red)
            {
                if (pathNode.RedNextNode != null)
                {
                    SetTargetPoint(pathNode.RedNextNode);
                }
            }
        }

        //开始移动
        navMeshAgent.isStopped = false;

        //设置移动目标点
        //this.transform.Translate((targetPoint - this.transform.position).normalized * Time.deltaTime * GameDefine.MonsterMoveSpeed,Space.World);
        navMeshAgent.destination = targetPoint;
    }

    public void SetTargetPoint(PathNode _pathNode)
    {
        pathNode = _pathNode;

        Vector3 pathNodePoint=_pathNode.transform.position;

        //在目标点周围随机生成一个点
        Vector2 go = Random.insideUnitCircle * 1.5f;
        targetPoint = new Vector3(pathNodePoint.x + go.x, pathNodePoint.y, pathNodePoint.z + pathNodePoint.y);

        this.transform.LookAt(new Vector3(targetPoint.x, this.transform.position.y, targetPoint.z));
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(this.transform.position, navMeshAgent.destination);

    //    //确认攻击范围
    //    var radius = monsterProp.MyProp.attackRadius;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(this.transform.position, radius * 1.5f);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(this.transform.position, radius);
    //}

}
