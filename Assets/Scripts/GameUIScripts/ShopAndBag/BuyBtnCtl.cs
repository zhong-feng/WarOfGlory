using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyBtnCtl : MonoBehaviour 
{
    [HideInInspector]
    public ItemID id;

	// Use this for initialization
	void Start () 
	{
        this.GetComponent<Button>().onClick.AddListener(onClick);
	}

    void onClick()
    {
        if(id != ItemID.IT_None)
        {
            if(BagSystem.Instance.AddItem(this.id))
            {
                var go = DataCacheSystem.Instance.GetItemProp(this.id);

                //播放声音
                AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_Buy);

                //物品出售获得金钱
                (BagSystem.Instance.heroCtl.MyProp as HeroPropModel).Money -= go.buyPrice;

                //广播属性变更
                BagSystem.Instance.heroCtl.ChangeHeroProp();

                //更新按钮状态
                UpButtonInteractable();
            }
            
        }
    }

    //更新按钮状态
    public void UpButtonInteractable()
    {
        if( id==ItemID.IT_None)
        {
            //禁用购买按钮
            OnUnableBuyBtn();
            return;
        }

        var go = DataCacheSystem.Instance.GetItemProp(this.id);

        //确认是否还可以继续买物品
        if ((BagSystem.Instance.heroCtl.MyProp as HeroPropModel).Money < go.buyPrice)  //有钱购买
        {
            //禁用购买按钮
            OnUnableBuyBtn();
            return;
        }
                
        if((BagSystem.Instance.NumOfItem >= BagSystem.Instance.BagItemUnitToggleArray.Length  //有空的背包格
                && BagSystem.Instance.cheakIsSameItem(go.id) == false )  )
        {
            //禁用购买按钮
            OnUnableBuyBtn();
            return;
        }

        //启用 购买按钮
        this.GetComponent<Button>().interactable = true;
    }

    //禁用购买按钮
    void OnUnableBuyBtn()
    {
       // id = ItemID.IT_None;

        this.GetComponent<Button>().interactable = false;
    }

}
