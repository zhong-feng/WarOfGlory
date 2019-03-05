using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAttackCtl : MonsterCtl
{
    public Transform BulletPrefab;
    public Transform DeadEffect;

    protected bool isCD = false;


    Transform GuangquanRed;
    Transform GuangquanYellow;

    public override void Start()
    {
        MyProp = DataCacheSystem.Instance.GetMonsterInfo(MonsterType.MT_Build);

        MyProp.campType = MyCampType;

        //监听 收到攻击事件
        MyEventSystem.AddEventListener(EventType.ET_Attack, HurtListen);

        GuangquanRed = this.transform.Find("guangquan_Red");
        GuangquanRed.gameObject.SetActive(false);

        GuangquanYellow = this.transform.Find("guangquan_yellow");
        GuangquanYellow.gameObject.SetActive(false);

        //地方箭塔显示光圈
        if(this.MyCampType != GameDefine.camp)
        {
            StartCoroutine(ShowGuangquan());
        }
        
    }

    //协程显示光圈
    IEnumerator ShowGuangquan()
    {
        while(true)
        {
            if(this.MyProp.curHP<=0)
            {
                GuangquanRed.gameObject.SetActive(false);
                GuangquanYellow.gameObject.SetActive(false);
                break;
            }

            HeroCtl heroCtl=null;
            if (GameDefine.CurGameMapType == GameMapType.GMT_Type_01)
            {
                heroCtl = GameCtl_Type_01.Instance.Player;
            }
            else if (GameDefine.CurGameMapType == GameMapType.GMT_Type_02)
            {
                heroCtl = GameCtl_Type_02.Instance.Player;
            }

            if(heroCtl !=null  )
            {
                if(this.enemy == heroCtl)
                {
                    GuangquanRed.gameObject.SetActive(true);
                    GuangquanYellow.gameObject.SetActive(false);
                }
                else
                {
                    //检测主角进入警戒范围
                    var radius = this.MyProp.attackRadius;

                    //检测 技能球与目标的距离
                    Vector3 curPos = this.transform.position;  //当前自己的位置
                    Vector3 destPos = heroCtl.transform.position;  //目标位置

                    //计算当前位置 和 最新目标路点 的距离
                    float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

                    //确认距离
                    if (distance < radius *1.2f)
                    {
                        GuangquanRed.gameObject.SetActive(false);
                        GuangquanYellow.gameObject.SetActive(true);
                    }
                    else
                    {
                        GuangquanRed.gameObject.SetActive(false);
                        GuangquanYellow.gameObject.SetActive(false);
                    }

                }
            }

            //每秒钟检测一次
            yield return new WaitForSeconds(1.0f);
        }

    }


    public override void AttackNormal()
    {
        //确认冷却
        if (isCD)
        {
            return;
        }

        //确认目标存在
        if (enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead
            || enemy.MyProp.campType == MyProp.campType || BulletPrefab == null)
        {
            enemy = null;

            return;
        }

        //生成 并 控制 子弹
        StartCoroutine(CreatBullet());

        //攻击冷却
        StartCoroutine(waitCDTime());
    }

    IEnumerator waitCDTime()
    {
        isCD = true;

        yield return new WaitForSeconds(2.0f);

        isCD = false;
    }

    IEnumerator CreatBullet()
    {
        //生成子弹
        Transform bullet = GameObject.Instantiate(BulletPrefab);

        //控制子弹位置
        Transform parent = this.transform.Find("HitPoint");
        if (parent != null)
        {
            bullet.SetParent(parent);
        }
        else
        {
            bullet.SetParent(this.transform);
        }
        bullet.localPosition = Vector3.zero;
        bullet.transform.parent = null;

        //为了效果 子弹刚出现时不播放
        StartCoroutine(AttackBegin(bullet));

        //插值移动移动系数
        float t = 0;

        //控制子弹移动
        while (true)
        {
            //判空
            if (enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead || enemy.MyProp.campType == MyProp.campType )
            {
                enemy = null;
                GameObject.Destroy(bullet.gameObject);

                break;
            }

            //检测 技能球与目标的距离
            Vector3 curPos = bullet.transform.position;  //当前自己的位置
            Vector3 destPos = enemy.transform.Find("HitPoint").position;  //目标位置

            //计算当前位置 和 最新目标路点 的距离
            float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

            //攻击到 目标
            if (distance < 0.2f)
            {
                //广播消息
                MyEventSystem.DispatchEventMsg(EventType.ET_Attack, enemy, this);

                yield return StartCoroutine(AttackComplete(bullet));

                break;
            }

            //没到目标点 ->向目标点移动
            t += Time.deltaTime / 2.5f;  //2.5秒 打到目标
            bullet.transform.position = Vector3.Lerp(curPos, destPos, t);
            yield return null;
        }

        yield return null;
    }

    //暂停特效播放
    IEnumerator AttackBegin(Transform _bullet)
    {
        _bullet.GetChild(0).gameObject.SetActive(true);
        _bullet.GetChild(1).gameObject.SetActive(false);

        yield return null;
    }

    //攻击完成  特效继续播放
    IEnumerator AttackComplete(Transform _bullet)
    {
        if (_bullet != null)

        _bullet.GetChild(0).gameObject.SetActive(false);
        _bullet.GetChild(1).gameObject.SetActive(true);

        float time = 1.0f;
        ParticleSystem go = _bullet.GetChild(1).GetComponent<ParticleSystem>();
        if (go != null)
        {
            time = go.time;
        }

        yield return new WaitForSeconds(time);

        if (_bullet != null)
        {
            GameObject.Destroy(_bullet.gameObject);
        }

    }

    public override void OnDeath()
    {
        CreatDeadEffect();
    }

    //生成特效
    public void CreatDeadEffect()
    {
        Transform effect = null;
        //生成死亡特效
        if (DeadEffect != null)
        {
            effect = GameObject.Instantiate(DeadEffect);

            //控制特效位置
            Transform parent = this.transform.Find("HitPoint");
            if (parent != null)
            {
                effect.SetParent(parent);
            }
            else
            {
                effect.SetParent(this.transform);
            }
            effect.localPosition = Vector3.zero;

        }

        //延迟销毁
        StartCoroutine(DeleyDestroy(effect));
    }

    IEnumerator DeleyDestroy(Transform _effect)
    {
        float deleyTime = 2.0f;

        if (_effect != null)
        {
            ParticleSystem go = _effect.GetComponent<ParticleSystem>();
            if (go != null)
            {
                deleyTime = go.main.duration;
            }
        }
        else
        {
            deleyTime = 0.2f;
        }

        yield return new WaitForSeconds(deleyTime);

        //GameObject.Destroy(this.gameObject);
        for (int i = 0; i < this.transform.childCount;i++ )
        {
            if(i==0)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    
    }

}
