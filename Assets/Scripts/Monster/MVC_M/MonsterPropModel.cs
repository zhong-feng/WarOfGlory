using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物类型
public enum MonsterType
{
    MT_None,
    MT_Aige,
    MT_Heimolv,
    MT_Jinglingnan,

    MT_LastHero=20,  //做分界线

    MT_YuanRen=30,

    MT_Juren=40,

    MT_Zhizhu=50,

    MT_Build=60
}

//阵营
public enum CampType
{
    CT_None,
    CT_Red,
    CT_Blue,
    CT_Neutral
}

//播放动画的状态
public enum StatusType 
{
    ST_None,
    
    ST_Idle=1,
    
    ST_Run=2,
    
    ST_NoBreak=10,  //不可以被打断；做分界线，上面可以打断，下面不可以打断
    
    ST_Attack01=40,   //普通攻击
    ST_Attack02,
    ST_Attack03,
    
    ST_CanSkill=49, //分界线 可以从上面切到技能

    ST_Skill01 = 50,  //技能
    ST_Skill02,

    ST_Dead = 60,   //死亡
}

 public class MonsterPropModel
{
    public CampType campType = CampType.CT_None;    //阵营

    public MonsterType monsterType ;  //怪物种类
    
    public string name ;

    public string modelPath;

    public int curHP;  //HP
    public int maxHP;
    
    public int curMP;   //MP
    public int maxMP;
    
    public float attact;   //攻击力
    public float defense;  //防御力  
    
    public float attackRadius;//攻击范围

    public int giveEXP;    //死亡时掉落经验
    public int giveMoney;  //死亡时掉落的金币

    public StatusType CurStatus = StatusType.ST_None;    //动画当前状态

    public float skillAttack = 1; //技能攻击系数
    
    public MonsterPropModel()
    {
        //赋初值
        monsterType = MonsterType.MT_None;  //怪物种类

        this.name = "";
        this.modelPath = "";
        this.curHP = this.maxHP = 0;
        this.curMP = this.maxMP = 0;
        this.attact = 0.0f;
        this.defense = 0.0f;
        this.attackRadius = 0.0f;
        this.giveEXP =0;
        this.giveMoney = 0;
    }

    //拷贝构造
    public MonsterPropModel(MonsterPropModel monster)
    {
        if (monster != null)
        {
            this.monsterType = monster.monsterType;
            this.name = monster.name;
            this.modelPath = monster.modelPath;
            this.curHP = this.maxHP = monster.curHP;
            this.curMP = this.maxMP = monster.curMP;
            this.attact = monster.attact;
            this.defense = monster.defense;
            this.attackRadius = monster.attackRadius;
            this.giveEXP = monster.giveEXP;
            this.giveMoney = monster.giveMoney;
        }
    }

}
