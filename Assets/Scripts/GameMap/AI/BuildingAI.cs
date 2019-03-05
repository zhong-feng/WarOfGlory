using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAI : MonoBehaviour 
{
    protected MonsterCtl BuildProp;
    
	// Use this for initialization
	void Start () 
	{
        BuildProp = this.GetComponent<MonsterCtl>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (BuildProp == null || BuildProp.MyProp.curHP<=0)
        {
            return;
        }

		//检测是否有敌人
        if (BuildProp.enemy == null)
        {
            CheckEnemy();
        }
        else
        {
            AttackEnemy();
        }
	}

    void CheckEnemy()
    {
        //检测是否有敌人进入视野
        var radius = BuildProp.MyProp.attackRadius;
        RaycastHit[] hitArray = Physics.SphereCastAll(this.transform.position, radius , Vector3.up);
        for (int i = 0; i < hitArray.Length; i++)
        {
            MonsterCtl go = hitArray[i].transform.GetComponent<MonsterCtl>();
            if (go != null && go.MyCampType!= BuildProp.MyProp.campType && go.MyProp.curHP > 0)
            {
                //添加敌人
                BuildProp.enemy = go;

                return;
            }
        }
    }

    void AttackEnemy()
    {
        if (BuildProp.enemy.MyProp.curHP <= 0)
        {
            BuildProp.enemy = null;

            return;
        }

        //确认攻击范围
        var radius = BuildProp.MyProp.attackRadius;

        //检测 与目标的距离
        Vector3 curPos = this.transform.position;  //当前自己的位置
        Vector3 destPos = BuildProp.enemy.transform.position;  //目标位置

        //计算当前位置 和 敌人 的距离
        float distance = Vector2.Distance(new Vector2(curPos.x, curPos.z), new Vector2(destPos.x, destPos.z));

        if (distance > radius)  //离开攻击范围
        {
            BuildProp.enemy = null;
            return;
        }

        BuildProp.AttackNormal();
    }

    void OnDrawGizmos()
    {
        //确认攻击范围
        if(BuildProp != null)
        {
            var radius = BuildProp.MyProp.attackRadius;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}
