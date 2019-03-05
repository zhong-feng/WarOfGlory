using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品id
public enum ItemID
{
    IT_None = 0,
    ID_HP = 10,
    ID_MP = 11,

    ID_IronySword = 21,  //铁剑
    ID_SteelSword = 31,      //钢剑
    ID_IronyAxe = 41,      //赤炎剑

    ID_IronyJacket = 51,   //铁甲
    ID_SteelJacket = 61,   //钢甲
    ID_IronyHelmet = 71,   //海神之护
};

public enum ItemType
{
    IT_None,
    IT_Drug ,      //药品
    IT_Weapon ,    //武器
    IT_Armor ,     //防具
}

public class ItemProp 
{
    public ItemID id { get; set; }      //物品ID
    public ItemType type { get; set; }  //物品类型

    public string name { get; set; }
    public string desc { get; set; }

    public int buyPrice { get; set; }     //购买的价格
    public int sellPrice { get; set; }    //卖出去的价格

    public int maxNum { get; set; }       //叠加上限  
    public int curNum  { get; set; }      //当前道具的叠加数量

    public string iconPath { get; set; }  //物品图标的路径

    //无参构造
    public ItemProp()
    {
        this.id = ItemID.IT_None;
        this.type = ItemType.IT_None;

        this.name = "";
        this.desc = "";

        this.buyPrice = 0;
        this.sellPrice = 0;

        this.curNum = 1;
        this.maxNum = 0;

        this.iconPath = "";
    }

    //拷贝构造函数
    public ItemProp(ItemProp _itemProp)
    {
        if (_itemProp != null)
        {
            this.id = _itemProp.id;
            this.type = _itemProp.type;

            this.name = _itemProp.name;
            this.desc = _itemProp.desc;
            this.iconPath = _itemProp.iconPath;
            this.buyPrice = _itemProp.buyPrice;
            this.sellPrice = _itemProp.sellPrice;

            this.curNum = _itemProp.curNum;
            this.maxNum = _itemProp.maxNum;
        }
    }

    //克隆该对象的新对象
    public virtual ItemProp Clone()
    {
        ItemProp prop = new ItemProp(this);
        return prop;
    }

    public virtual void UseItem()
    {

    }
}
