using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemUnit : ShopItemUnit
{
    protected override void setItemPropTip()
    {
        //更新提示框属性 (名称 信息)
        itemPropTip.transform.Find("Name").GetComponent<Text>().text = item.name;
        itemPropTip.transform.Find("Describe").GetComponent<Text>().text = item.desc;
        itemPropTip.transform.Find("Price").GetComponent<Text>().text = "+" + item.sellPrice.ToString();

        //先设置 父对象为 BagItem ～ 确定预制物的合适位置
        itemPropTip.transform.SetParent(this.transform);
        (itemPropTip.transform as RectTransform).pivot = new Vector2(0, 0);
        itemPropTip.transform.localPosition = Vector3.zero;

        //为解决遮挡关系，再设置一次父物体
        var canvas = GameObject.Find("Canvas");
        itemPropTip.transform.SetParent(canvas.transform);

    }


}
