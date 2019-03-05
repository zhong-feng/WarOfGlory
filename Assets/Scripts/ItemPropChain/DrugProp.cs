using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugProp : ItemProp
{
    public int hp { get; set; }
    public int mp { get; set; }

    public DrugProp() :base()
    {
        this.hp = 0;
        this.mp = 0;
    }

    //拷贝构造函数
    public DrugProp(DrugProp _drugProp): base(_drugProp)
    {
        if (_drugProp != null)
        {
            this.hp = _drugProp.hp;
            this.mp = _drugProp.mp;
        }
    }

    //拷贝构造函数
    public DrugProp(ItemProp _drugProp) : base(_drugProp)
    {
        this.hp = 0;
        this.mp = 0;
    }

    public override ItemProp Clone()
    {
        DrugProp gp = new DrugProp(this);
        return gp;
    }

    public override void UseItem()
    {
        HeroCtl hero = BagSystem.Instance.heroCtl;

        if(hero != null && hero.MyProp != null)
        {
            hero.MyProp.curHP += this.hp;
            if (hero.MyProp.curHP > hero.MyProp.maxHP)
            {
                hero.MyProp.curHP = hero.MyProp.maxHP;
            }

            hero.MyProp.curMP += this.mp;
            if (hero.MyProp.curMP > hero.MyProp.maxMP)
            {
                hero.MyProp.curMP = hero.MyProp.maxMP;
            }

            this.curNum--;

            hero.ChangeHeroProp();
        }

    }
}

