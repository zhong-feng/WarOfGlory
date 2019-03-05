using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BagSystem : MonoSingleton<BagSystem>  
{
    [HideInInspector]
    public HeroCtl heroCtl = null;   //用于获取玩家信息

    public Button SellBtn;    //购买按钮

    [HideInInspector]
    public Toggle[] BagItemUnitToggleArray;
    [HideInInspector]
    public int NumOfItem = 0;

    //默认 空物品图标
    Sprite defaultSprite;

    [HideInInspector]
    public event Action ChangeBagEvent;

    //重载 初始化
    public override void OnInitSingleton()
    {
        //获取 ToggleGroup
        var toggleGroup = this.GetComponent<ToggleGroup>();

        if (toggleGroup)
        {
            //获取Toggle
            BagItemUnitToggleArray = toggleGroup.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < BagItemUnitToggleArray.Length; i++)
            {
                //给Toggle添加回调函数
                BagItemUnitToggleArray[i].onValueChanged.AddListener(OnValueChanged);
            }
            
            //存留默认 空物品图标
            defaultSprite = BagItemUnitToggleArray[0].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        }
    }

    public void Start()
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
        
    }

    void DispatchEventChangeBag()
    {
        if(ChangeBagEvent != null)
        {
            ChangeBagEvent();
        }
    }

    void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        for (int i = 0; i < BagItemUnitToggleArray.Length; i++)
        {
            if (BagItemUnitToggleArray[i].isOn)
            {
                //获取物品信息
                BagItemUnit go = BagItemUnitToggleArray[i].GetComponent<BagItemUnit>();

                if (go != null && go.item != null && go.item.id != ItemID.IT_None)
                {
                    //启用出售按钮
                    OnEnableSellBtn(go.item);
                }
                else
                {
                    //禁用购买按钮
                    OnUnableSellBtn();

                    BagItemUnitToggleArray[i].isOn = false;
                }

                return;
            }
        }

        //禁用购买按钮
        OnUnableSellBtn();
    }

    #region  出售按钮相关操作
    
    //禁用出售按钮
    void OnUnableSellBtn()
    {
        //获取 购买按钮的 脚本
        if (SellBtn != null)
        {
            SellBtn.interactable = false;

            SellBtnCtl sellBtnCtl = SellBtn.GetComponent<SellBtnCtl>();

            if (sellBtnCtl != null)
            {
                sellBtnCtl.item = null;
            }
        }
    }

    //启用出售按钮
    void OnEnableSellBtn(ItemProp _item)
    {
        //获取 购买按钮的 脚本
        if (SellBtn != null)
        {
            SellBtnCtl sellBtnCtl = SellBtn.GetComponent<SellBtnCtl>();

            if (sellBtnCtl != null)
            {
                SellBtn.interactable = true;

                sellBtnCtl.item = _item;
            }
        }
    }

    #endregion

    #region //添加 购买物品

    public bool AddItem(ItemID _id)
    {
        //先确认有足够的金币购买该物品
        ItemProp goItemProp = DataCacheSystem.Instance.GetItemProp(_id);
        if ((heroCtl.MyProp as HeroPropModel).Money < goItemProp.buyPrice)
        {
            return false;
        }

        //查看是否已有物品且可以叠加
        if(cheakIsSameItem(_id))
        {
            //遍历 查找 id相同 且可以叠加的物品单元格
            for (int j = 0; j < BagItemUnitToggleArray.Length; j++)
            {
                var go = BagItemUnitToggleArray[j].GetComponent<BagItemUnit>();

                if (go != null && go.item != null && go.item.id == _id && go.item.curNum < go.item.maxNum)
                {
                    go.item.curNum++;

                    //广播 背包改变事件
                    DispatchEventChangeBag();

                    //更新UI显示
                    if (go.item.curNum > 1)
                    {
                        BagItemUnitToggleArray[j].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.item.curNum.ToString();
                    }

                    //检查增加属性更新人物属性  广播 属性变更事件 
                    AddPlayerProp(go.item);

                    return true;
                }
            } //end for
        }
        else
        {
            //确认还有空的物品单元格
            if (NumOfItem >= BagItemUnitToggleArray.Length)
            {
                return false;
            }

            //找到空的单元格添加物品
            for (int i = 0; i < BagItemUnitToggleArray.Length; i++)
            {
                var goBagUnit = BagItemUnitToggleArray[i].GetComponent<BagItemUnit>();

                if (goBagUnit == null)
                {
                    continue;
                }

                if (goBagUnit.item == null || goBagUnit.item.id == ItemID.IT_None)
                {
                    goBagUnit.item = goItemProp;

                    //UI更新 (数量默认值是1 不显示 故只更新图片)
                    BagItemUnitToggleArray[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ResourcesManager.Load<Sprite>(goItemProp.iconPath);

                    //使用单元格+1
                    NumOfItem++;

                    //广播 背包改变事件
                    DispatchEventChangeBag();

                    //更新人物属性  广播 属性变更事件
                    AddPlayerProp(goItemProp);

                    return true;
                }

            } //end for
        }

        return false;
    }

    //购买装备 增加属性
    void AddPlayerProp(ItemProp _item)
    {
        if(_item.type == ItemType.IT_Weapon)
        {
            heroCtl.MyProp.attact += (_item as WeaponProp).attack;
        }
        else if(_item.type == ItemType.IT_Armor)
        {
            heroCtl.MyProp.defense += (_item as ArmorProp).defense;
        }

        //广播消息
        heroCtl.ChangeHeroProp();
    }

    //查看是否又可以叠加的物品
    public bool cheakIsSameItem(ItemID _id)
    {
        //遍历 查找 id相同 且可以叠加的物品单元格
        for(int j=0;j< BagItemUnitToggleArray.Length;j++ )
        {
            var go = BagItemUnitToggleArray[j].GetComponent<BagItemUnit>();

            if(go != null && go.item != null && go.item.id == _id && go.item.curNum < go.item.maxNum)
            {
                return true;
            }
        }
        
        return false;
    }

    #endregion

    #region //删除物品 出售 

    public bool DelItem(ItemProp _item)
    {
        //遍历 查找 选择的单元格
        for (int j = 0; j < BagItemUnitToggleArray.Length; j++)
        {
            var go = BagItemUnitToggleArray[j].GetComponent<BagItemUnit>();

            if (go != null && go.item != null && go.item.id !=ItemID.IT_None  && go.item == _item)
            {
                go.item.curNum--;

                if (go.item.curNum<=0)
                {
                    go.item = null;

                    BagItemUnitToggleArray[j].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "";
                    BagItemUnitToggleArray[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = defaultSprite;

                    //使用单元格-1
                    NumOfItem--;

                    //物品没有了 取消选中
                    BagItemUnitToggleArray[j].isOn = false;
                }
                else
                {
                    if(go.item.curNum > 1)
                    {
                        BagItemUnitToggleArray[j].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.item.curNum.ToString();
                    }
                    else
                    {
                        BagItemUnitToggleArray[j].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "";
                    }
                }
                
                //广播 背包改变事件
                DispatchEventChangeBag();
                
                //减少属性  广播事件
                subPlayerProp(_item);

                return true;
            }
        }

        return false;
    }

    //减少属性 广播事件
    void subPlayerProp(ItemProp _item)
    {
        if (_item.type == ItemType.IT_Weapon)
        {
            heroCtl.MyProp.attact -= (_item as WeaponProp).attack;
        }
        else if (_item.type == ItemType.IT_Armor)
        {
            heroCtl.MyProp.defense -= (_item as ArmorProp).defense;
        }

        //广播消息
        heroCtl.ChangeHeroProp();
    }

    #endregion

    //使用物品
    public void UesBagSystemItem(ItemProp _item)
    {
        if(_item == null)
        {
            return;
        }

        //使用物品
        _item.UseItem();

        for(int i=0;i < BagItemUnitToggleArray.Length;i++)
        {
            BagItemUnit go = BagItemUnitToggleArray[i].GetComponent<BagItemUnit>();
            if(go == null || go.item==null || go.item.curNum <=0 )
            {
                go.item = null;

                BagItemUnitToggleArray[i].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "";
                BagItemUnitToggleArray[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = defaultSprite;

                //使用单元格-1
                NumOfItem--;
            }
        }

        //广播消息
        DispatchEventChangeBag();
    }
}
