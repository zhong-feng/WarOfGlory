using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HPBarCtl : MonoBehaviour
{
    Slider hpBar;
    MonsterCtl monster;

	// Use this for initialization
	void Start ()
	{
        //获取组件
        hpBar = this.GetComponent<Slider>();
        monster = this.transform.parent.GetComponent<MonsterCtl>();

        //血条赋初值
        if(hpBar != null)
        {
            hpBar.value = 1;
        }

        //添加事件 管控血条的更新
        if(monster != null)
        {
            monster.HPBarUpdataEvent += updataHPBar;
        }

    }

    void Update()
    {
        //this.transform.LookAt(transform.position - Camera.main.transform.forward);
        this.transform.forward = Camera.main.transform.forward;
    }

    void updataHPBar(float _hp)
    {
        if(hpBar != null)
        {
            hpBar.value = _hp;
        }
    }
	
}
