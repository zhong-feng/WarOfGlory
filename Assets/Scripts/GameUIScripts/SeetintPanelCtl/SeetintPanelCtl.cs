using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.AI;

public class SeetintPanelCtl : MonoBehaviour 
{
    public GameOverPanle gameOver;

	// Use this for initialization
	void Start () 
	{
        Toggle ViceToggle = this.transform.Find("ViceToggle").GetComponent<Toggle>();
        ViceToggle.isOn = GameDefine.isViceOn;
        ViceToggle.onValueChanged.AddListener(CheckVice);

        Toggle AutoAttackToggle = this.transform.Find("AutoAttackToggle").GetComponent<Toggle>();
        AutoAttackToggle.isOn = GameDefine.isAutoAttack;
        AutoAttackToggle.onValueChanged.AddListener(checkAutoAttack);

        Button SurrenderBtn = this.transform.Find("SurrenderBtn").GetComponent<Button>();
        SurrenderBtn.onClick.AddListener(onClickSurrenderBtn);
	}

    void onClickSurrenderBtn()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_StartGame);

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

        gameOver.GameOver(false);
    }

    void CheckVice(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        GameDefine.isViceOn = !GameDefine.isViceOn;

        MyEventSystem.DispatchEventMsg(EventType.ET_ChangeViceSet, null, null);
    }

    void checkAutoAttack(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        GameDefine.isAutoAttack = !GameDefine.isAutoAttack;
    }
}
