using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBirthPlace : MonoBehaviour 
{
    public Transform MonsterParent;
    public PathNode[] TargetPathNode;
    public HeroCtl[] HeroPrefabsArray;
    public CampType campType;
    public int NumOfHero;
	// Use this for initialization
	void Start () 
	{
        if(campType == GameDefine.camp)
        {
            NumOfHero--;
        }

        if(TargetPathNode.Length >0 
            && HeroPrefabsArray.Length >0 
            && NumOfHero >0)
        {
            StartCoroutine(SpawnMonster());
        }
        
    }

    protected IEnumerator SpawnMonster()
    {
        for (int i = 0; i < NumOfHero; i++)
        {
            var go = GameObject.Instantiate(HeroPrefabsArray[Random.Range(0, HeroPrefabsArray.Length)]);

            //设置属性
            go.transform.SetParent(MonsterParent);
            go.MyCampType = campType;
            go.MyProp.campType = campType;
            go.BirthPlace = this;

            if (go.MyCampType==CampType.CT_Red)
            {
                go.transform.Find("MiniMap").Find("Blue").gameObject.SetActive(false);
            }
            else
            {
                go.transform.Find("MiniMap").Find("Red").gameObject.SetActive(false);
            }

            //重置英雄属性
            resetHeroProp(go);
            
            yield return null;
        }
    }

    //重置英雄属性
    public void resetHeroProp(HeroCtl heroCtl)
    {
        heroCtl.transform.position = this.transform.position;

        heroCtl.GetComponent<MonsterAI>().SetTargetPoint(TargetPathNode[Random.Range(0, TargetPathNode.Length)]);

        heroCtl.MyProp.curHP = heroCtl.MyProp.maxHP;
        heroCtl.MyProp.curMP = heroCtl.MyProp.maxMP;

        heroCtl.UpdataAnimation(StatusType.ST_Idle);

        (heroCtl.MyProp as HeroPropModel).skill01.isCD = false;
        (heroCtl.MyProp as HeroPropModel).skill02.isCD = false;

        heroCtl.ChangeHeroProp();
        heroCtl.changeHP(heroCtl.MyProp.curHP);
    }
}
