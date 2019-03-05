using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class ShopItemUnit : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [HideInInspector]
    public ItemProp item;

    [HideInInspector]
    public GameObject itemPropTip = null;  //物品提示框

    //鼠标移入的时候生成提示框
    public void OnPointerEnter(PointerEventData eventData)
    {
        //判断是否有物品
        if (item == null || item.id == ItemID.IT_None)
        {
            return;
        }

        //确认删除上一次的Tip （确保安全）
        destoryItemPropTip();

        //生成提示框
        itemPropTip = Instantiate(ResourcesManager.Load<GameObject>("Prefabs/UI/ItemInfoTip"));

        setItemPropTip();           //设置提示框信息
    }

    //设置提示框信息
    protected virtual void setItemPropTip()
    {
        //更新提示框属性 (名称 信息)
        itemPropTip.transform.Find("Name").GetComponent<Text>().text = item.name;
        itemPropTip.transform.Find("Describe").GetComponent<Text>().text = item.desc;
        itemPropTip.transform.Find("Price").GetComponent<Text>().text ="-" + item.buyPrice.ToString() ;

        //先设置 父对象为 BagItem ～ 确定预制物的合适位置
        itemPropTip.transform.SetParent(this.transform);
        (itemPropTip.transform as RectTransform).pivot = new Vector2(0, 1);
        itemPropTip.transform.localPosition = Vector3.zero;

        //为解决遮挡关系，再设置一次父物体
        var canvas = GameObject.Find("Canvas");
        itemPropTip.transform.SetParent(canvas.transform);
    }

    //鼠标移出的时候销毁掉提示框
    public void OnPointerExit(PointerEventData eventData)
    {
        destoryItemPropTip();
    }

    //单击销毁 提示框
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        destoryItemPropTip();
    }

    //对象销毁时  删除提示框 (安全)
    public void OnDestroy()
    {
        destoryItemPropTip();
    }

    //删除 提示框
    protected virtual void destoryItemPropTip()
    {
        if (itemPropTip != null)
        {
            Destroy(itemPropTip);

            itemPropTip = null;
        }
    }


    //更新物品属性
    public void ChangeItem(ItemProp _item)
    {
        if(_item != null && _item.id != ItemID.IT_None)
        {
            item = _item;

            Image itemUnitImag = this.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            itemUnitImag.sprite = ResourcesManager.Load<Sprite>(item.iconPath);
        }
    }


}
