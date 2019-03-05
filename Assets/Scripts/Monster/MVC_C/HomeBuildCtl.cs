using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using DG.Tweening;

public class HomeBuildCtl : BuildAttackCtl 
{
    public GameOverPanle OverPanel;

    bool isPlayedAttackHome = false;

    public override void HurtListen(object MonsterCtl01, object MonsterCtl02)
    {
        base.HurtListen(MonsterCtl01, MonsterCtl02);

        MonsterCtl UnderAttacked = MonsterCtl01 as MonsterCtl;

        //确认是否自己受到伤害
        if (UnderAttacked != this)
        {
            return;
        }

        //播放水晶收到攻击声音
        if (MyCampType == GameDefine.camp && isPlayedAttackHome==false)
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_HomeAttacked);

            //协程更改 标志变量
            StartCoroutine(changeFlagOfAttackHome());
        }

    }

    IEnumerator changeFlagOfAttackHome()
    {
        isPlayedAttackHome = true;

        yield return new WaitForSeconds(30);

        isPlayedAttackHome = false;
    }

    public override void OnDeath()
    {
        //禁用 玩家 操作
        var gameConstroller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameConstroller.gameObject.SetActive(false);

        // 系统不再出兵
        var monsterBirthPlaceArray = GameObject.FindGameObjectWithTag("BirthPlaceParent").GetComponentsInChildren<MonsterBirthplace>();
        for (int j = 0; j < monsterBirthPlaceArray.Length; j++)
        {
            monsterBirthPlaceArray[j].gameObject.SetActive(false);
        }

        //禁用 monster 操作
        var monsterArray = GameObject.FindGameObjectWithTag("MonsterParent").GetComponentsInChildren<MonsterAI>();
        for (int i = 0; i < monsterArray.Length; i++)
        {
            monsterArray[i].enabled = false;
            monsterArray[i].GetComponent<NavMeshAgent>().enabled = false;
        }

        //播放游戏结束 第一阶段声音
        if (MyCampType == GameDefine.camp)
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Failed01);
        }
        else
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Victory01);
        }

        //协程处理动画 等待相关操作
        StartCoroutine(waitDoTween());
    }

    IEnumerator waitDoTween()
    {
        //将摄像机移过来 (固定位置 角度) 
        Camera.main.GetComponent<ThirdPersonCamera>().enabled = false;

        Vector3 target = this.transform.position - this.transform.forward.normalized * 50 + this.transform.up.normalized * 30 + this.transform.right.normalized * 1.3f;

        Camera.main.transform.eulerAngles = new Vector3(30, 0, 0);
        Camera.main.transform.DOMove(target, 3.0f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(4.0f);
        
        //播放游戏结束 第一阶段声音
        if (MyCampType == GameDefine.camp)
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Failed02);
        }
        else
        {
            AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_Victory02);
        }

        //生成死亡特效
        CreatDeadEffect();

        yield return new WaitForSeconds(2.0f);

        if (MyCampType == GameDefine.camp)
        {
            OverPanel.GameOver(false);
        }
        else
        {
            OverPanel.GameOver(true);
        }
    }
}
