using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroJingLIngNanCtl : HeroCtl 
{
    //技能攻击特效预置体
    public Transform Skill01Prefab;
    public Transform Skill02Prefab;

    //写两个普通攻击动作
    bool isFirstAttackNormal=true;

    //重写普通攻击
    public override void AttackNormal()
    {
        //检测动画状态是否可以技能攻击
        if (this.MyProp.CurStatus > StatusType.ST_NoBreak)
        {
            return ;
        }

        //检测 敌人
        if (this.enemy == null)
        {
            //检测前方攻击距离内是否有敌人
            var radius = MyProp.attackRadius;
            RaycastHit[] hitArray = Physics.SphereCastAll(this.transform.position, radius, Vector3.up);
            for (int i = 0; i < hitArray.Length; i++)
            {
                MonsterCtl go = hitArray[i].transform.GetComponent<MonsterCtl>();
                if (go != null && go.MyProp.campType != this.MyProp.campType && go.MyProp.curHP > 0)
                {
                    //添加敌人
                    this.enemy = go;

                    break;
                }
            }
        }
        else
        {
            //确认攻击范围
            var radius = this.MyProp.attackRadius;

            //检测 与目标的距离
            Vector3 curPos = this.transform.position;  //当前自己的位置
            Vector3 destPos = this.enemy.transform.position;  //目标位置

            //计算当前位置 和 敌人 的距离
            float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

            //敌人离开攻击范围
            if (distance > radius)  //超过攻击范围
            {
                this.enemy = null;
                return;
            }
        }


        if (this.enemy != null)
        {
            //看向目标点
            this.transform.LookAt(new Vector3(enemy.transform.position.x, this.transform.position.y, enemy.transform.position.z));
        }

        //播放动画
        if (isFirstAttackNormal)
        {
            //开协程改变下一次普通的动画
            StartCoroutine(changeAttackNormal());

            this.UpdataAnimation(StatusType.ST_Attack01);
        }
        else
        {
            this.UpdataAnimation(StatusType.ST_Attack02);
        }
    }

    IEnumerator changeAttackNormal()
    {
        isFirstAttackNormal = false;
        yield return new WaitForSeconds(3.0f);
        isFirstAttackNormal = true;
    }

    //普通攻击回调函数
    public void AttackNormalAnimationCallback()
    {
        //确认目标存在
        if (enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead)
        {
            enemy = null;
            return;
        }

        //广播消息
        MyEventSystem.DispatchEventMsg(EventType.ET_Attack, enemy, this);

        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Attack, this.transform);
    }

    //技能01 攻击前方敌人
    public override bool AttackSkill01()
    {
        //检测动画状态是否可以技能攻击
        if(this.MyProp.CurStatus>StatusType.ST_CanSkill)
        {
            return false;
        }

        //检测 敌人
        if(this.enemy == null)
        {
            //检测前方攻击距离内是否有敌人
            var radius = MyProp.attackRadius;
            RaycastHit[] hitArray = Physics.SphereCastAll(this.transform.position, radius , Vector3.up);
            for (int i = 0; i < hitArray.Length; i++)
            {
                MonsterCtl go = hitArray[i].transform.GetComponent<MonsterCtl>();
                if (go != null && go.MyProp.campType != this.MyProp.campType && go.MyProp.curHP > 0)
                {
                    //确认敌人在前方 (前方 +/- 60度角内)
                    Vector3 direction = go.transform.position - this.transform.position;
                    float angle = Vector3.Angle(this.transform.forward, direction);
                    if (Mathf.Abs(angle) <60)
                    {
                        //添加敌人
                        this.enemy = go;

                        break;
                    }

                }
            }
        }
        else
        {
            //确认攻击范围
            var radius = this.MyProp.attackRadius;

            //检测 与目标的距离
            Vector3 curPos = this.transform.position;  //当前自己的位置
            Vector3 destPos = this.enemy.transform.position;  //目标位置

            //计算当前位置 和 敌人 的距离
            float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

            //敌人离开攻击范围
            if (distance > radius)  //超过攻击范围
            {
                this.enemy = null;
                return false;
            }
        }

        if (this.enemy !=null)
        {
            //消耗MP
            if (MyProp.curMP < (MyProp as HeroPropModel).skill01.use)
            {
                return false;
            }
            else
            {
                MyProp.curMP -= (MyProp as HeroPropModel).skill01.use;

                //MP减少 广播消息
                this.ChangeHeroProp();
            }

            //看向目标点
            this.transform.LookAt(new Vector3(enemy.transform.position.x,this.transform.position.y,enemy.transform.position.z) );

            //播放动画
            this.UpdataAnimation(StatusType.ST_Skill01);

            return true;
        }

        return false;
    }

    public void Skill01AnimationCallback()
    {
        //确认目标存在
        if (enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead)
        {
            enemy = null;
            return;
        }

        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Skill01, this.transform);

        //生成特效
        Transform go = GameObject.Instantiate(Skill01Prefab);
        go.SetParent(enemy.transform.Find("HitPoint"));
        go.localPosition = Vector3.zero;
        go.SetParent(null);

        //开协程 延迟销毁
        StartCoroutine(DeleyDestroyEffect01(go));

        //更新伤害
        MyProp.skillAttack = (MyProp as HeroPropModel).skill01.propRation;

        //广播消息
        MyEventSystem.DispatchEventMsg(EventType.ET_Attack, enemy, this);
    }


    IEnumerator DeleyDestroyEffect01(Transform _effect)
    {
        float deleyTime = 0.2f;

        if (_effect != null)
        {
            ParticleSystem go = _effect.GetComponent<ParticleSystem>();
            if (go != null)
            {
                deleyTime = go.main.duration;
            }
        }

        yield return new WaitForSeconds(deleyTime);

        if(_effect !=null)
        {
            Destroy(_effect.gameObject);
        }
    }

    //攻击前方范围的人
    public override bool AttackSkill02()
    {
        //检测动画状态是否可以技能攻击
        if (this.MyProp.CurStatus > StatusType.ST_CanSkill)
        {
            return false;
        }

        //消耗MP
        if (MyProp.curMP < (MyProp as HeroPropModel).skill01.use)
        {
            return false;
        }
        else
        {
            MyProp.curMP -= (MyProp as HeroPropModel).skill01.use;

            //MP减少 广播消息
            this.ChangeHeroProp();
        }

        //播放动画
        this.UpdataAnimation(StatusType.ST_Skill02);

        return true;
    }

    public void Skill02AnimationCallback()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Skill02, this.transform);

        //生成特效
        Transform go = GameObject.Instantiate(Skill02Prefab);
        go.SetParent(this.transform.Find("Skill02Point"));
        go.transform.position = Vector3.zero;
        go.SetParent(null);
        
        //开协程 延迟销毁
        StartCoroutine(DeleyDestroyEffect02(go));

        //更新伤害
        MyProp.skillAttack = (MyProp as HeroPropModel).skill02.propRation;

        //检测前方技能攻击范围内是否有敌人
        var radius = (this.MyProp as HeroPropModel).skill02.radiusRatio;
        RaycastHit[] hitArray = Physics.SphereCastAll(this.transform.position + this.transform.forward.normalized * 5, radius, Vector3.up);
        for (int i = 0; i < hitArray.Length; i++)
        {
            MonsterCtl goMonster = hitArray[i].transform.GetComponent<MonsterCtl>();
            if (goMonster != null && goMonster.MyProp.campType != this.MyProp.campType && goMonster.MyProp.curHP > 0)
            {
                //广播消息
                MyEventSystem.DispatchEventMsg(EventType.ET_Attack, goMonster, this);
            }
        }

    }

    IEnumerator DeleyDestroyEffect02(Transform _effect)
    {
        float deleyTime = 0.2f;

        if (_effect != null)
        {
            ParticleSystem go = _effect.Find("Effect").GetComponent<ParticleSystem>();
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


    //void OnDrawGizmos()
    //{
    //    //确认技能2攻击范围
    //    var radius = (this.MyProp as HeroPropModel).skill02.radiusRatio;

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(this.transform.position + this.transform.forward.normalized *5, radius);

    //}

}
