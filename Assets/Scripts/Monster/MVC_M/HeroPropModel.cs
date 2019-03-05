using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPropModel :  MonsterPropModel
{
    public SkillProp skill01;
    public SkillProp skill02;

    public int curExp=0;
    public int maxExp=100;

    public int Money = 100;

    public int Level = 1;
    public int NumOfFreeSkill = 1;

    //拷贝构造函数
    public HeroPropModel(MonsterPropModel baseMonsterr) :base(baseMonsterr)
    {
        if(baseMonsterr != null)
        {
            switch (monsterType)
            {
                case MonsterType.MT_Aige:
                    {
                        skill01 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Aige01);
                        skill02 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Aige02);
                        break;
                    }
                case MonsterType.MT_Heimolv:
                    {
                        skill01 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Heimolv01);
                        skill02 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Heimolv02);
                        break;
                    }
                case MonsterType.MT_Jinglingnan:
                    {
                        skill01 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Jinglingnan01);
                        skill02 = DataCacheSystem.Instance.GetSkillInfo(SkillID.SI_Jinglingnan02);
                        break;
                    }
                default: break;
            }
            
        }
    }

    //递归升级
    public virtual int UpLevel()
    {
        //确定结束条件
        if(curExp < maxExp)
        {
            return 0;
        }

        //升级 更新属性
        Level++;
        NumOfFreeSkill++;

        curExp -= maxExp;
        maxExp =(int)(maxExp* 1.3f);

        curHP = (int)(curHP * 1.3f);
        maxHP = (int)(maxHP * 1.3f);

        curMP = (int)(curMP * 1.3f);
        maxMP = (int)(maxMP * 1.3f);

        attact *= 1.3f;
        defense *= 1.3f;

        giveEXP = (int)(giveEXP * 1.3f);
        giveMoney = (int)(giveMoney * 1.3f);

        return 1 + UpLevel();
    }
    
    public void ChooseUpSkill(bool isFirstSkill)
    {
        if (NumOfFreeSkill >0)
        {
            if (isFirstSkill)
            {
                UpSkill(skill01);
            }
            else
            {
                UpSkill(skill02);
            }
        }
    }

    void UpSkill(SkillProp _skill)
    {
        NumOfFreeSkill--;

        //_skill.skillCD *= 0.8f;   //冷却时间

        _skill.propRation *= 1.3f;  //作用系数（攻击、防御、）
        _skill.use = (int)(_skill.use * 1.3f);
    }
}
