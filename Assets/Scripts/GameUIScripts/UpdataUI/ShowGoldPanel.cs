using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ShowGoldPanel : MonoBehaviour 
{
    // 从外部引入数字图片  确认顺序 0-9 
    public Sprite[] NumOfSpriteArray;

    public Sprite defaultSprite;    //默认没有的图片

    HeroCtl heroCtl = null;   //用于获取玩家信息

	// Use this for initialization
	void Start () 
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



        //更新显示金币数量
        updataGold();

        //注册 事件
        if(heroCtl != null)
        {
            heroCtl.ChangePropEvent += updataGold;
        }
	}

    public void OnDestroy()
    {
        if (heroCtl != null)
        {
            heroCtl.ChangePropEvent -= updataGold;
        }
    }


	
    void updataGold()
    {
        if(heroCtl != null)
        {
            //获取金币数量
            int gold = (heroCtl.MyProp as HeroPropModel).Money;

            //分别对每一位数赋值
            if (gold > 9999)  //万位
            {
                this.transform.Find("WanWei").GetComponent<Image>().sprite = NumOfSpriteArray[gold % 100000 / 10000];
            }
            else
            {
                this.transform.Find("WanWei").GetComponent<Image>().sprite = defaultSprite;
            }

            if (gold > 999)  //千位
            {
                this.transform.Find("QianWei").GetComponent<Image>().sprite = NumOfSpriteArray[gold % 10000 / 1000];
            }
            else
            {
                this.transform.Find("QianWei").GetComponent<Image>().sprite = defaultSprite;
            }


            if (gold > 99)  //百位
            {
                this.transform.Find("BaiWei").GetComponent<Image>().sprite = NumOfSpriteArray[gold % 1000 / 100];
            }
            else
            {
                this.transform.Find("BaiWei").GetComponent<Image>().sprite = defaultSprite;
            }

            if (gold > 9)  //十位
            {
                this.transform.Find("ShiWei").GetComponent<Image>().sprite = NumOfSpriteArray[gold % 100 / 10];
            }
            else
            {
                this.transform.Find("ShiWei").GetComponent<Image>().sprite = defaultSprite;
            }

            //个位
            this.transform.Find("GeWei").GetComponent<Image>().sprite = NumOfSpriteArray[gold % 10];
        }
    }
}
