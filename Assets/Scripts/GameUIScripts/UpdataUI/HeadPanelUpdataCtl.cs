using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HeadPanelUpdataCtl : MonoBehaviour
{
    [HideInInspector]
    public HeroCtl heroCtl = null;   //用于获取玩家信息

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

        //显示玩家信息
        showPlyerInfo();

        //注册事件
        heroCtl.ChangePropEvent += showPlyerInfo;
    }

    void showPlyerInfo()
    {
        if (heroCtl != null)
        {
            HeroPropModel go = heroCtl.MyProp as HeroPropModel;

            this.transform.Find("HPBar").GetComponent<Image>().fillAmount = go.curHP * 1.0f / go.maxHP;
            this.transform.Find("HPBar").GetChild(0).GetComponent<Text>().text = go.curHP.ToString() + "/" + go.maxHP.ToString();

            this.transform.Find("MPBar").GetComponent<Image>().fillAmount = go.curMP * 1.0f / go.maxMP;
            this.transform.Find("MPBar").GetChild(0).GetComponent<Text>().text = go.curMP.ToString() + "/" + go.maxMP.ToString();

            this.transform.Find("EXPBar").GetComponent<Image>().fillAmount = go.curExp * 1.0f / go.maxExp;
            this.transform.Find("EXPBar").GetChild(0).GetComponent<Text>().text = go.curExp.ToString() + "/" + go.maxExp.ToString();

            this.transform.Find("LevelText").GetComponent<Text>().text = "LV: " + go.Level.ToString();
        }
    }

}
