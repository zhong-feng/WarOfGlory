using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorProp : ItemProp
{
    public int defense { get; set; }

    public ArmorProp() : base()
    {
        this.defense = 0;
    }

    //拷贝构造函数
    public ArmorProp(ArmorProp _armorProp) : base(_armorProp)
    {
        if (_armorProp != null)
        {
            this.defense = _armorProp.defense;
        }
    }

    //拷贝构造函数
    public ArmorProp(ItemProp _armorProp) : base(_armorProp)
    {
        this.defense = 0;
    }

    public override ItemProp Clone()
    {
        ArmorProp gp = new ArmorProp(this);
        return gp;
    }
}
