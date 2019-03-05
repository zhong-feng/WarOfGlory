using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BagPanelCtl : MonoBehaviour
{
    //管控背包面板单元格
    BagPanelUnit[] bagUnitArray;

    // Use this for initialization
    void Start ()
	{
        //获取Toggle
        bagUnitArray = this.GetComponentsInChildren<BagPanelUnit>();

        //将单元格与背包系统绑定
        for (int i = 0; i < bagUnitArray.Length && i < BagSystem.Instance.BagItemUnitToggleArray.Length; i++)
        {
            BagItemUnit go = BagSystem.Instance.BagItemUnitToggleArray[i].GetComponent<BagItemUnit>();
            bagUnitArray[i].bagUnit = go;
        }

        //更新单元格信息
        UpdataUnitInfo();

        //注册 事件
        BagSystem.Instance.ChangeBagEvent += UpdataUnitInfo;
    }

    //设置使用物品的快捷键
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[0].item);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[1].item);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[2].item);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[3].item);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[4].item);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            BagSystem.Instance.UesBagSystemItem(bagUnitArray[5].item);
        }
    }
    
    private void OnDestroy()
    {
        BagSystem.Instance.ChangeBagEvent -= UpdataUnitInfo;
    }
    
    void UpdataUnitInfo()
    {
        for (int i = 0; i < bagUnitArray.Length; i++)
        {
            bagUnitArray[i].UpdataBagPanelUnit();
        }
    }

}
