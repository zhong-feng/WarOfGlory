using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.AI;

public class AttackPanelCtl : MonoBehaviour
{
    HeroCtl heroCtl;

    Button attackBtn;
    Button skill01Btn;
    Button skill02Btn;

    Button upSkillLevel01Btn;
    Button upSkillLevel02Btn;

    Button backHomeBtn;
    
	// Use this for initialization
	void Start ()
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

        attackBtn = this.transform.Find("Attack").GetComponent<Button>();
        attackBtn.onClick.AddListener(attack);

        skill01Btn = this.transform.Find("Skill01").GetComponent<Button>();
        skill01Btn.onClick.AddListener(skill01);
        upSkillLevel01Btn = skill01Btn.transform.Find("LevelUp").GetComponent<Button>();
        upSkillLevel01Btn.onClick.AddListener(upSkillLevel01);

        skill02Btn = this.transform.Find("Skill02").GetComponent<Button>();
        skill02Btn.onClick.AddListener(skill02);
        upSkillLevel02Btn = skill02Btn.transform.Find("LevelUp").GetComponent<Button>();
        upSkillLevel02Btn.onClick.AddListener(upsKILLLevel02);

        backHomeBtn = this.transform.Find("BackHome").GetComponent<Button>();
        backHomeBtn.onClick.AddListener(backHome);

        skill01Btn.GetComponent<Image>().sprite = heroCtl.SkillSprite01;
        skill01Btn.transform.Find("Gray").GetComponent<Image>().sprite = heroCtl.SkillSprite01;

        skill02Btn.GetComponent<Image>().sprite = heroCtl.SkillSprite02;
        skill02Btn.transform.Find("Gray").GetComponent<Image>().sprite = heroCtl.SkillSprite02;

        //更新技能升级图标状态
        CheckCanSkillLevelUp();
        
        //注册事件
        heroCtl.ChangePropEvent += CheckCanSkillLevelUp;
    }

    private void OnDestroy()
    {
        //注册事件
        heroCtl.ChangePropEvent -= CheckCanSkillLevelUp;
    }

    //设置快捷键
    private void Update()
    {
        if(Input.GetKeyDown (KeyCode.J))
        {
            attack();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            skill01();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            skill02();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            backHome();
        }

        //自动攻击
        if (GameDefine.isAutoAttack && GameDefine.inAutoAttack)
        {
            if (heroCtl.enemy == null)
            {
                //自动添加攻击2倍攻击范围内的敌人
                var radius = heroCtl.MyProp.attackRadius * 2;
                RaycastHit[] hitArray = Physics.SphereCastAll(heroCtl.transform.position, radius, Vector3.up);
                for (int i = 0; i < hitArray.Length; i++)
                {
                    MonsterCtl go = hitArray[i].transform.GetComponent<MonsterCtl>();
                    if (go != null && go.MyProp.campType != heroCtl.MyProp.campType && go.MyProp.curHP > 0)
                    {
                        //添加敌人
                        heroCtl.enemy = go;

                        break;
                    }
                }
            }
            else
            {
                if(heroCtl.enemy.MyProp.curHP<=0)
                {
                    heroCtl.enemy = null;
                    return;
                }

                //确认攻击范围
                var radius = heroCtl.MyProp.attackRadius;

                //检测 与目标的距离
                Vector3 curPos = heroCtl.transform.position;  //当前自己的位置
                Vector3 destPos = heroCtl.enemy.transform.position;  //目标位置

                //计算当前位置 和 敌人 的距离
                float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

                //敌人离开攻击范围
                if (distance > radius)  //超过攻击范围
                {
                    heroCtl.navMeshAgent.isStopped = false;
                    heroCtl.navMeshAgent.destination = destPos;
                }
                else
                {
                    //看向目标点
                    heroCtl.transform.LookAt(new Vector3(destPos.x, curPos.y, destPos.z));

                    //停止移动
                    heroCtl.navMeshAgent.isStopped = true;

                    attack();
                }
            }
        }
    }




    void CheckCanSkillLevelUp()
    {
        if((heroCtl.MyProp as HeroPropModel).NumOfFreeSkill>0 )
        {
            upSkillLevel01Btn.gameObject.SetActive(true);
            upSkillLevel02Btn.gameObject.SetActive(true);
        }
        else
        {
            upSkillLevel01Btn.gameObject.SetActive(false);
            upSkillLevel02Btn.gameObject.SetActive(false);
        }
    }


	public void attack()
    {
        heroCtl.AttackNormal();

        heroCtl.ShowAttackGuangquan();

        GameDefine.inAutoAttack = true;
    }

    public void skill01()
    {
        if((heroCtl.MyProp as HeroPropModel).skill01.isCD == false)
        {
            if (heroCtl.AttackSkill01())
            {
                //技能进入冷却
                StartCoroutine(SkillCD(true));

                GameDefine.inAutoAttack = true;
            }
        }
       
    }

    public void skill02()
    {
        if ((heroCtl.MyProp as HeroPropModel).skill02.isCD == false)
        {
            if (heroCtl.AttackSkill02())
            {
                //技能进入冷却
                StartCoroutine(SkillCD(false));

                GameDefine.inAutoAttack = true;
            }
        }
    }

    //技能冷却
    IEnumerator SkillCD(bool isFirstSkill)
    {
        if(heroCtl != null ||( heroCtl.MyProp is HeroPropModel)==true )
        {
            float CDTime;
            Image graySprite;

            if (isFirstSkill == true)
            {
                CDTime = (heroCtl.MyProp as HeroPropModel).skill01.skillCD;
                (heroCtl.MyProp as HeroPropModel).skill01.isCD = true;
                graySprite = skill01Btn.transform.Find("Gray").GetComponent<Image>();
            }
            else
            {
                CDTime = (heroCtl.MyProp as HeroPropModel).skill02.skillCD;
                (heroCtl.MyProp as HeroPropModel).skill02.isCD = true;
                graySprite = skill02Btn.transform.Find("Gray").GetComponent<Image>();
            }

            float t = 0.0f;
            while (true)
            {
                t += Time.deltaTime;
                graySprite.fillAmount =1- t / CDTime;

                if (t >= CDTime)
                {
                    if (isFirstSkill == true)
                    {
                        (heroCtl.MyProp as HeroPropModel).skill01.isCD = false;
                    }
                    else
                    {
                        (heroCtl.MyProp as HeroPropModel).skill02.isCD = false;
                    }
                    break;
                }

                yield return null;
            }
        }
       
        yield return null;
    }

    public void upSkillLevel01()
    {
        (heroCtl.MyProp as HeroPropModel).ChooseUpSkill(true);
        heroCtl.ChangeHeroProp();
    }

    public void upsKILLLevel02()
    {
        (heroCtl.MyProp as HeroPropModel).ChooseUpSkill(false);
        heroCtl.ChangeHeroProp();
    }

    public void backHome()
    {
        heroCtl.Backhome();

        GameDefine.inAutoAttack = false;
    }
}
