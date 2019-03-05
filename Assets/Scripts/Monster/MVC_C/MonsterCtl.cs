using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtl : MonoBehaviour
{
    public MonsterType monsterType;  //外部引入传参， 怪物类型
    public CampType MyCampType;

    [HideInInspector]
    public MonsterPropModel MyProp;  //自己的属性 根据类型动态生成

    [HideInInspector]
    public MonsterCtl enemy;    //敌人  通过其他脚本控制传递

    [HideInInspector]
    public Animator animator;    // 动画组件 控制动画

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    public event Action<float> HPBarUpdataEvent;   //事件 监控血条变化
    
    // 初始化
    public virtual void  Start ()
	{
        //获取动画组件
        animator = this.GetComponent<Animator>();

        navMeshAgent = this.GetComponent<NavMeshAgent>();

        //动态生成属性
        MyProp = DataCacheSystem.Instance.GetMonsterInfo(monsterType);
        MyProp.campType = this.MyCampType;

        //监听 收到攻击事件
        MyEventSystem.AddEventListener(EventType.ET_Attack, HurtListen);
    }

    // 销毁 撤销监听事件
    public virtual void OnDestroy()
    {
        MyEventSystem.DelEventListener(EventType.ET_Attack, HurtListen);
    }
    
    //动画播放完成回调函数
    public virtual void CompleteAnimationCallback()
    {
        //短暂延迟后 该状态为 idle
        StartCoroutine(waitToIdle());
    }

    IEnumerator waitToIdle()
    {
        yield return null;

        if(this.MyProp.curHP >0)
        {
            //将动画状态切换为 idle
            this.MyProp.CurStatus = StatusType.ST_Idle;

            animator.SetInteger("StatusType", (int)StatusType.ST_Idle);
        }
    }

    //更新动画状态
    public void UpdataAnimation(StatusType _type)
    {
        if (animator != null && MyProp != null)
        {
            this.MyProp.CurStatus = _type;

            animator.SetInteger("StatusType", (int)_type);
        }
    }

    //普攻方法
    public virtual void AttackNormal()
    {
        //确认目标存在
        if(enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead)
        {
            enemy = null;
            return;
        }
        
        //广播消息
        MyEventSystem.DispatchEventMsg(EventType.ET_Attack, enemy, this);
    }

    //监听攻击事件（ MonsterCtl01 -> 被攻击者  MonsterCtl02 -> 攻击者）
    public virtual void HurtListen(object MonsterCtl01, object MonsterCtl02)
    {
        if(MyProp.curHP <=0 )
        {
            return;
        }

        MonsterCtl UnderAttacked = MonsterCtl01 as MonsterCtl;
        MonsterCtl Attacker = MonsterCtl02 as MonsterCtl;

        //确认是否自己受到伤害
        if(UnderAttacked != this)
        {
            return;
        }

        //计算伤害
        float damage = Attacker.MyProp.attact * Attacker.MyProp.skillAttack - UnderAttacked.MyProp.defense;
        if(damage < 1)
        {
            damage = 1;
        }

        //扣血
        changeHP((int)damage*(-1));

        if(UnderAttacked.MyProp.curHP <=0)
        {
            //广播死亡
            MyEventSystem.DispatchEventMsg(EventType.ET_Death, MonsterCtl01, MonsterCtl02);

            OnDeath();
        }
        
    }

    public virtual void OnDeath()
    {
        //取消订阅 攻击
        MyEventSystem.DelEventListener(EventType.ET_Attack, HurtListen);

        //播放死亡动画
        UpdataAnimation(StatusType.ST_Dead);

        //延迟销毁
        StartCoroutine(DeleyDestroy());
    }

    IEnumerator DeleyDestroy()
    {
        yield return new WaitForSeconds(10.0f);

        GameObject.Destroy(this.gameObject);

    }

    public virtual void changeHP(int num)
    {
        MyProp.curHP += num;

        if(MyProp.curHP <0)
        {
            MyProp.curHP = 0;
        }
        else if(MyProp.curHP > MyProp.maxHP)
        {
            MyProp.curHP = MyProp.maxHP;
        }
        
        if (HPBarUpdataEvent != null)
        {
            HPBarUpdataEvent(MyProp.curHP * 1.0f / MyProp.maxHP);
        }
    }
}
