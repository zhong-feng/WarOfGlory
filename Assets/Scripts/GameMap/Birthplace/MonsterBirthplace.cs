using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBirthplace : MonoBehaviour 
{
    public PathNode TargetPathNode;
    public GameObject MonsterPrefab;
    public Transform MonsterParent;

	// Use this for initialization
	public virtual void Start () 
	{
        if (MonsterParent != null && TargetPathNode != null && MonsterPrefab != null)
        {
            StartCoroutine(SpawnMonster());
        }
	}

    public virtual IEnumerator SpawnMonster()
    {
        //开始延迟 10 秒出兵
        yield return new WaitForSeconds(10);

        while(true)
        {
            //生成一波小兵 3个
            for (int i = 0; i < 3;i++ )
            {
                var go = GameObject.Instantiate(MonsterPrefab);
                go.transform.position = this.transform.position;

                go.transform.SetParent(MonsterParent);

                var monsterAI = go.GetComponent<MonsterAI>();

                if(monsterAI != null)
                {
                    monsterAI.SetTargetPoint(TargetPathNode);
                }

                //每个间隔一定时间
                yield return new WaitForSeconds(0.2f);

            }

            //每隔 30 秒出一波兵
            yield return new WaitForSeconds(30);

            break;
        }
    }

}
