using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ShopTagCtl : MonoBehaviour 
{
    public ItemType Type;

    public Button BuyBtn;

    [HideInInspector]
    public Toggle[] ShopItemUnitToggleArray;

    bool isFirstEnable = true;

    HeroCtl heroCtl = null;   //用于获取玩家信息

    public void Awake()
    {
        //获取 ToggleGroup
        var toggleGroup = this.GetComponent<ToggleGroup>();

        if (toggleGroup)
        {
            //获取Toggle
            ShopItemUnitToggleArray = toggleGroup.GetComponentsInChildren<Toggle>();
        }


    }

    void onInitShop()
    {
        //从缓存获取物品数据
        var itr = DataCacheSystem.Instance.itemDic.GetEnumerator();

        for(int i=0;itr.MoveNext();)
        {
            ItemProp go=itr.Current.Value;
            if(go.type == Type)
            {
                //给物品单元格赋值
                ShopItemUnit itemUnit = ShopItemUnitToggleArray[i].GetComponent<ShopItemUnit>();

                if(itemUnit != null)
                {
                    itemUnit.ChangeItem(go.Clone());
                }

                i++;
            }
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

        //初始化
        onInitShop();

        var toggleGroup = this.GetComponent<ToggleGroup>();

        for (int i = 0; i < ShopItemUnitToggleArray.Length; i++)
        {
            //给Toggle添加回调函数
            ShopItemUnitToggleArray[i].onValueChanged.AddListener(OnValueChanged);
        }

        if (toggleGroup)
        {
            //默认全部关闭
            toggleGroup.SetAllTogglesOff();
        }

        //禁用购买按钮
        if (BuyBtn != null)
        {
            BuyBtn.interactable = false;

            BuyBtnCtl buyBtnCtl = BuyBtn.GetComponent<BuyBtnCtl>();

            if (buyBtnCtl != null)
            {
                buyBtnCtl.id = ItemID.IT_None;
            }
        }
    }

    public void OnEnable()
    {
        //第一次 OnEnable 因为时间原因，第一次跳过
        if(isFirstEnable)
        {
            isFirstEnable = false;
            return;
        }

        //var toggleGroup = this.GetComponent<ToggleGroup>();

        //if (toggleGroup)
        //{
        //    //默认全部关闭
        //    toggleGroup.SetAllTogglesOff();
        //}

        for(int i=0;i<ShopItemUnitToggleArray.Length;i++ )
        {
            ShopItemUnitToggleArray[i].isOn = false;
        }

        //禁用购买按钮
        //获取 购买按钮的 脚本
        if (BuyBtn != null)
        {
            BuyBtn.interactable = false;

            BuyBtnCtl buyBtnCtl = BuyBtn.GetComponent<BuyBtnCtl>();

            if (buyBtnCtl != null)
            {
                 buyBtnCtl.id = ItemID.IT_None;
            }
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < ShopItemUnitToggleArray.Length; i++)
        {
            ShopItemUnitToggleArray[i].isOn = false;
        }

        //禁用购买按钮
        //获取 购买按钮的 脚本
        if (BuyBtn != null)
        {
            BuyBtn.interactable = false;

            BuyBtnCtl buyBtnCtl = BuyBtn.GetComponent<BuyBtnCtl>();

            if (buyBtnCtl != null)
            {
                buyBtnCtl.id = ItemID.IT_None;
            }
        }
    }

    void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        for (int i = 0; i < ShopItemUnitToggleArray.Length; i++)
        {
            if (ShopItemUnitToggleArray[i].isOn)
            {
                //获取物品信息
                ShopItemUnit go = ShopItemUnitToggleArray[i].GetComponent<ShopItemUnit>();

                if (go != null && go.item != null && go.item.id != ItemID.IT_None)
                {
                    //启用购买按钮
                    OnEnableBuyBtn(go.item.id);

                    if(  (heroCtl.MyProp as HeroPropModel).Money >= go.item.buyPrice   //有钱购买
                    && (BagSystem.Instance.NumOfItem < BagSystem.Instance.BagItemUnitToggleArray.Length  //有空的背包格
                    || BagSystem.Instance.cheakIsSameItem(go.item.id) )  )
                    {
                        //启用购买按钮
                       // OnEnableBuyBtn(go.item.id);
                    }
                    else
                    {
                        //禁用购买按钮
                        OnUnableBuyBtn();
                    }

                }
                else
                {
                    //禁用购买按钮
                    OnUnableBuyBtn();

                    ShopItemUnitToggleArray[i].isOn = false;
                }

                return;
            }
        }

        //禁用购买按钮
        OnUnableBuyBtn();
    }

    //禁用购买按钮
    void OnUnableBuyBtn()
    {
        //获取 购买按钮的 脚本
        if (BuyBtn != null)
        {
            BuyBtn.interactable = false;

            BuyBtnCtl buyBtnCtl = BuyBtn.GetComponent<BuyBtnCtl>();

            if(buyBtnCtl != null)
            {
               // buyBtnCtl.id = ItemID.IT_None;
            }
        }
    }

    //启用购买按钮
    void OnEnableBuyBtn(ItemID _id)
    {
        //获取 购买按钮的 脚本
        if (BuyBtn != null)
        {
            BuyBtnCtl buyBtnCtl = BuyBtn.GetComponent<BuyBtnCtl>();

            if (buyBtnCtl != null)
            {
                BuyBtn.interactable = true;

                buyBtnCtl.id = _id;
            }
        }
    }

}
