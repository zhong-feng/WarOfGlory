using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProp : ItemProp
{
    public int attack { get; set; }

    public WeaponProp() : base()
    {
        this.attack = 0;
    }

    //拷贝构造函数
    public WeaponProp(WeaponProp _weaponProp) : base(_weaponProp)
    {
        if (_weaponProp != null)
        {
            this.attack = _weaponProp.attack;
        }
    }

    //拷贝构造函数
    public WeaponProp(ItemProp _weaponProp) : base(_weaponProp)
    {
        this.attack = 0;
    }

    public override ItemProp Clone()
    {
        WeaponProp gp = new WeaponProp(this);
        return gp;
    }
}
