using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellBtnCtl : MonoBehaviour 
{
    [HideInInspector]
    public ItemProp item;

    public BuyBtnCtl buyBtn;

	// Use this for initialization
	void Start () 
	{
        this.GetComponent<Button>().onClick.AddListener(onClick);
	}

    void onClick()
    {
        if (item != null && item.id != ItemID.IT_None)
        {
            int money = item.sellPrice;

            //出售物品  （返回值 成功或失败）
            if(BagSystem.Instance.DelItem(item))
            {
                //播放声音
                AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_Sell);

                //物品出售获得金钱
                (BagSystem.Instance.heroCtl.MyProp as HeroPropModel).Money += money;

                //广播属性变更
                BagSystem.Instance.heroCtl.ChangeHeroProp();

                //查看是否需要激活购买按钮
                if(buyBtn)
                {
                    buyBtn.UpButtonInteractable();
                }
            }
        }
    }
}
