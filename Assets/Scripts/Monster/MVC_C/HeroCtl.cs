using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.UI;

public class HeroCtl : MonsterCtl 
{
    [HideInInspector]
    public event Action ChangePropEvent;   // 监控属性变化 事件

    [HideInInspector]
    public HeroBirthPlace BirthPlace;

    public Sprite SkillSprite01;
    public Sprite SkillSprite02;

    Transform AttackGuangquan;

    [HideInInspector]
    public HeroCtl heroCtl = null;   //用于获取玩家信息 判断声音的播放

    public override void Start()
    {
        //获取玩家信息
        if (GameDefine.CurGameMapType == GameMapType.GMT_Type_01)
        {
            heroCtl = GameCtl_Type_01.Instance.Player;
        }
        else if (GameDefine.CurGameMapType == GameMapType.GMT_Type_02)
        {
            heroCtl = GameCtl_Type_02.Instance.Player;
        }
    }

    public void Awake()
    {
        //获取攻击光圈
        AttackGuangquan = this.transform.Find("guangquan_fanwei");
        if (AttackGuangquan != null)
        {
            AttackGuangquan.gameObject.SetActive(false);
        }
       
        //获取动画组件
        animator = this.GetComponent<Animator>();

        navMeshAgent = this.GetComponent<NavMeshAgent>();

        //动态生成属性
        MyProp = DataCacheSystem.Instance.GetMonsterInfo(monsterType);
        MyProp.campType = this.MyCampType;

        //监听 收到攻击事件
        MyEventSystem.AddEventListener(EventType.ET_Attack, HurtListen);

        //监听死亡事件（获取经验 金钱）
        MyEventSystem.AddEventListener(EventType.ET_Death, DeadListen);
    }

    //显示攻击光圈
    public void ShowAttackGuangquan()
    {
        if(AttackGuangquan!=null && AttackGuangquan.gameObject.activeSelf==false)
        {
            AttackGuangquan.gameObject.SetActive(true);

            //短暂时间后去激活
            StartCoroutine(HidAttackGuanngquan());
        }
    }

    IEnumerator HidAttackGuanngquan()
    {
        yield return new WaitForSeconds(0.2f);

        AttackGuangquan.gameObject.SetActive(false);
    }

    //注销时不再监听事件
    public override void OnDestroy()
    {
        MyEventSystem.DelEventListener(EventType.ET_Attack, HurtListen);
        MyEventSystem.DelEventListener(EventType.ET_Death, DeadListen);
    }

    //监听死亡事件（ MonsterCtl01 -> 死者  MonsterCtl02 -> 攻击者）
    void DeadListen(object MonsterCtl01, object MonsterCtl02)
    {
        //确认自己，没有死亡
        if(MyProp.curHP <= 0)
        {
            return;
        }

        MonsterCtl UnderAttacked = MonsterCtl01 as MonsterCtl;
        MonsterCtl Attacker = MonsterCtl02 as MonsterCtl;

        //确认是否 己方杀死
        if (Attacker.MyCampType != MyCampType)
        {
            return;
        }

        //获取经验 金币
        int exp = UnderAttacked.MyProp.giveEXP;
        int money = UnderAttacked.MyProp.giveMoney;

        //不是自己杀死获取 0.2
        if(Attacker !=this)
        {
            exp =(int) (exp* 0.2f);
            money = (int)(money * 0.2f);
        }

        HeroPropModel go = this.MyProp as HeroPropModel;
        go.Money += money;
        go.curExp += exp;

        //升级
        if(go.UpLevel() >0)
        {
            //升级的粒子特效
            GameObject goLevelUpEffect = Instantiate(ResourcesManager.Load<GameObject>("Prefabs/Effect/LevelUp"));
            goLevelUpEffect.transform.position = this.transform.position;
            goLevelUpEffect.transform.SetParent(this.transform);

            //延迟销毁
            StartCoroutine(delayDesTroy(goLevelUpEffect));

            //播放升级的声音
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_LevelUp, this.transform);
        }

        //广播属性变更消息
        ChangeHeroProp();
    }

    IEnumerator delayDesTroy(GameObject _effect)
    {
        float deleyTime = 0.2f;

        //获取粒子特效长度-
        if (_effect != null)
        {
            ParticleSystem go = _effect.GetComponent<ParticleSystem>();
            if (go != null)
            {
                deleyTime = go.main.duration;
            }
        }

        yield return new WaitForSeconds(deleyTime);

        if (_effect != null)
        {
            Destroy(_effect.gameObject);
        }
    }

    //广播属性变更事件
    public void ChangeHeroProp()
    {
        if(ChangePropEvent != null)
        {
            ChangePropEvent();
        }

    }

    //重写基类扣血
    public override void changeHP(int num)
    {
        base.changeHP(num);

        //属性变化
        ChangeHeroProp();
    }

    //预留技能接口 返回释放结果 （成功或者失败）
    public virtual bool AttackSkill01()
    {
        ChangeHeroProp();

        return false;
    }

    public virtual bool AttackSkill02()
    {
        ChangeHeroProp();

        return false;
    }

    bool isBackingHome = false;

    //重写死亡事件
    public override void OnDeath()
    {
        if(this.gameObject.activeSelf==true)
        {
            //停止移动
            this.navMeshAgent.isStopped = true;
        }
        
        GameDefine.inAutoAttack = false;

        //播放死亡动画
        UpdataAnimation(StatusType.ST_Dead);

        //延迟回到原点
        StartCoroutine(relive());
    }

    public virtual void onDeadInAnimationCallBack()
    {
        if(heroCtl == this)
        {
            //死亡的声音
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Dead, this.transform);
        }

    }

    IEnumerator relive()
    {
        yield return new WaitForSeconds(5.0f);

        BirthPlace.resetHeroProp(this);

        if (heroCtl == this)
        {
            //播放复活的声音
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Revive, this.transform);
        }
    }



    public override void CompleteAnimationCallback()
    {
        base.CompleteAnimationCallback();

        MyProp.skillAttack = 1;
    }


    public void Backhome()
    {
        if (isBackingHome == false && MyProp.CurStatus < StatusType.ST_NoBreak && MyProp.curHP >0)
        {
            this.UpdataAnimation(StatusType.ST_Idle);
            this.navMeshAgent.isStopped = true;

            StartCoroutine(backhomeEffect());
        }
    }

    IEnumerator backhomeEffect()
    {
        isBackingHome =true;

        //生成回城声音 （可以被打断 特殊处理）
        GameObject BackHomeAudio=GameObject.Instantiate(ResourcesManager.Load<GameObject>("Prefabs/Audio/BackHome"));
        BackHomeAudio.transform.SetParent(this.transform);
        BackHomeAudio.transform.position = Vector3.zero;

        //生成回城特效
        GameObject BackEffect;
        if(this.MyCampType == CampType.CT_Blue)
        {
            BackEffect = GameObject.Instantiate(ResourcesManager.Load<GameObject>("Prefabs/Effect/BackHome_Blue"));
        }
        else
        {
            BackEffect = GameObject.Instantiate(ResourcesManager.Load<GameObject>("Prefabs/Effect/BackHome_Red"));
        }

        BackEffect.transform.position = this.transform.position;

        ParticleSystem BottomEffect = BackEffect.transform.Find("Begin").GetComponent<ParticleSystem>();
        ParticleSystem BackgroundEffect = BackEffect.transform.Find("Background").GetComponent<ParticleSystem>();

        float time = 0.0f;

        //辅助检测 回城的时间段
        bool isFirst=true;

        //确认是否回城被打断
        bool isBreakBackHome = false;

        //扣血会打断回城
        float preHP = MyProp.curHP / MyProp.maxHP;   //使用HP比例 升级不会打断回城

        //组合的回城效果 分为两个部分
        while(true)
        {
            time += Time.deltaTime;

            if (isFirst && time>0.1)
            {
                if (BottomEffect != null)
                {
                    BottomEffect.Pause();
                }
            }

            if (time>6.3f)
            {
                break;
            }

            //回城被打断
            if (MyProp.CurStatus != StatusType.ST_Idle || Mathf.Abs( preHP - MyProp.curHP / MyProp.maxHP) > Mathf.Epsilon)
            {
                GameObject.Destroy(BackEffect);
                GameObject.Destroy(BackHomeAudio);
                isBreakBackHome = true;
                break;
            }

            yield return null;
        }

        if (BackEffect != null)
        {
            if (BackgroundEffect != null)
            {
                BackgroundEffect.gameObject.SetActive(false);
            }

            if (BottomEffect != null)
            {
                BottomEffect.Play();
            }

            time = 0.0f;
            while(true)
            {
                time += Time.deltaTime;

                if(time >=1.2)
                {
                    break;
                }
                yield return null;
            }

            //回城特效播放完毕
            GameObject.Destroy(BackEffect);
            GameObject.Destroy(BackHomeAudio);

        }

        yield return null;

        if(isBreakBackHome==false)
        {
            this.transform.position = BirthPlace.transform.position;
        }

        isBackingHome = false;
    }

    //播放声音  脚步声
    public virtual void onRunAnimationCallBack()
    {
        if (heroCtl == this)
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Walk, this.transform);
        }

    }

    void OnDrawGizmos()
    {

        var radius = this.MyProp.attackRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
        
    }

}
