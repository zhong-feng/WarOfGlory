using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CharacteInfo : MonoBehaviour 
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

    public void OnDestroy()
    {
        //注册事件
        heroCtl.ChangePropEvent -= showPlyerInfo;
    }



    void showPlyerInfo()
    {
        if(heroCtl != null)
        {
            HeroPropModel go =heroCtl.MyProp as HeroPropModel;

            this.transform.Find("Name").GetComponent<Text>().text = go.name;

            this.transform.Find("HP").GetComponent<Text>().text = go.curHP.ToString() + "/" + go.maxHP.ToString();
            this.transform.Find("MP").GetComponent<Text>().text = go.curMP.ToString() + "/" + go.maxMP.ToString();
            this.transform.Find("Exp").GetComponent<Text>().text = go.curExp.ToString() + "/" + go.maxExp.ToString();

            this.transform.Find("ATK").GetComponent<Text>().text = go.attact.ToString();
            this.transform.Find("DEF").GetComponent<Text>().text = go.defense.ToString();
        }
    }
	
}
