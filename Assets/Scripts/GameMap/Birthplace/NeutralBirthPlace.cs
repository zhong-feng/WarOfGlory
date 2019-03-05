using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralBirthPlace : MonsterBirthplace
{
    //管理生成的怪物
    GameObject monster;

    public override void Start () 
    {
        StartCoroutine(CheckMonster());
    }

    protected IEnumerator CheckMonster()
    {
        while(true)
        {
            if(monster==null)
            {
                yield return StartCoroutine(SpawnMonster());
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    public override IEnumerator SpawnMonster()
    {
        //间隔三十秒
        yield return new WaitForSeconds(3.0f);

        monster = GameObject.Instantiate(MonsterPrefab, 
            this.transform.position, 
            this.transform.rotation, 
            MonsterParent);
        
    }
}
