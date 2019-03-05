using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//技能ID
public enum SkillID
{
    SI_None,

    SI_Aige01 = 1,
    SI_Aige02,

    SI_Heimolv01 = 11,
    SI_Heimolv02,

    SI_Jinglingnan01 = 21,
    SI_Jinglingnan02,

}

public class SkillProp 
{
    public SkillID ID;      //技能ID
    public string skillName;     //技能名

    public string iconPath;

    public float skillCD;       //冷却时间
    public int use;             //技能消耗

    public float propRation;    //作用系数（攻击、防御、）
    public float radiusRatio;   //攻击距离（和基础攻击范围的比例）

    public string describe;     //技能描述

    public bool isCD = false;

    public SkillProp()
    {
        //赋初值
        this.ID = SkillID.SI_None;
        this.skillName = "";
        this.iconPath = "";
        this.skillCD = 0.0f;
        this.use = 0;
        this.propRation = 0.0f;
        this.radiusRatio = 0.0f;
        this.describe = "";
    }

    //拷贝构造
    public SkillProp(SkillProp _skill)
    {
        if (_skill != null)
        {
            this.ID = _skill.ID;
            this.skillName = _skill.skillName;
            this.iconPath = _skill.iconPath;
            this.skillCD = _skill.skillCD;
            this.use = _skill.use;
            this.propRation = _skill.propRation;
            this.radiusRatio = _skill.radiusRatio;
            this.describe = _skill.describe;
        }
    }
    
}
