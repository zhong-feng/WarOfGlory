using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHeiMoLvCtl : HeroCtl 
{

    //子弹预置体
    public Transform BulletPrefab;

    public override void AttackNormal()
    {
        //生成 并 控制 子弹
        StartCoroutine(CreatBullet());
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
            if (enemy == null || enemy.MyProp.CurStatus == StatusType.ST_Dead)
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
            if (distance < 0.1f)
            {
                //广播消息
                MyEventSystem.DispatchEventMsg(EventType.ET_Attack, enemy, this);

                yield return StartCoroutine(AttackComplete(bullet));

                break;
            }

            //没到目标点 ->向目标点移动
            t += Time.deltaTime / 0.5f; //0.5秒 打到目标
            bullet.transform.position = Vector3.Lerp(curPos, destPos, t);
            //bullet.transform.Translate((destPos - curPos).normalized * Time.deltaTime * GameDefine.BulletSpeed);

            yield return null;
        }

        yield return null;
    }

    //暂停特效播放
    IEnumerator AttackBegin(Transform _bullet)
    {
        yield return new WaitForSeconds(0.1f);

        if (_bullet != null)
        {
            _bullet.GetComponent<ParticleSystem>().Pause();
        }
    }

    //攻击完成  特效继续播放
    IEnumerator AttackComplete(Transform _bullet)
    {
        ParticleSystem go = _bullet.GetComponent<ParticleSystem>();
        go.Play();

        yield return new WaitForSeconds(go.main.duration);

        if (_bullet != null)
        {
            GameObject.Destroy(_bullet.gameObject);
        }
    }


}
