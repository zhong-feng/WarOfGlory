using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagPanelUnit : ShopItemUnit ,IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler
{
    [HideInInspector]
    public BagItemUnit bagUnit;

    //默认 空物品图标
    Sprite defaultSprite;

    //canvas
    RectTransform canvas;

    private void Awake()
    {
        //获取默认 空物体图片
        defaultSprite = this.transform.Find("Image").GetComponent<Image>().sprite;

        canvas = GameObject.Find("Canvas").transform as RectTransform;  //Canvas ()
    }
    
    public void UpdataBagPanelUnit()
    {
        this.item = bagUnit.item;

        if (this.item != null &&this.item.id != ItemID.IT_None)
        {
            this.transform.Find("Image").GetComponent<Image>().sprite
                = ResourcesManager.Load<Sprite>(this.item.iconPath);
        }
        else
        {
            this.transform.Find("Image").GetComponent<Image>().sprite = defaultSprite;
        }
    }

    protected override void setItemPropTip()
    {
        //更新提示框属性 (名称 信息)
        itemPropTip.transform.Find("Name").GetComponent<Text>().text = item.name;
        itemPropTip.transform.Find("Describe").GetComponent<Text>().text = item.desc;
        itemPropTip.transform.Find("PriceMessage").GetComponent<Text>().text = "";

        //先设置 父对象为 BagItem ～ 确定预制物的合适位置
        itemPropTip.transform.SetParent(this.transform);
        (itemPropTip.transform as RectTransform).pivot = new Vector2(0, 0);
        itemPropTip.transform.localPosition = Vector3.zero;

        //为解决遮挡关系，再设置一次父物体
        var canvas = GameObject.Find("Canvas");
        itemPropTip.transform.SetParent(canvas.transform);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //左键双击 使用物品
        if (eventData.clickCount >= 2 && (int)eventData.button == 0)
        {
            BagSystem.Instance.UesBagSystemItem(this.item);
        }

    }

    GameObject dragObj;

    public void OnDrag(PointerEventData eventData)
    {
        if (dragObj != null)
        {
            dragObj.transform.Translate(eventData.delta);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //确认当前有物品
        if (item == null || item.type == ItemType.IT_None)
        {
            return;
        }

        //销毁 提示框 和上一次拖拽框（安全）
        destoryItemPropTip();
        destoryDragObj();

        dragObj =GameObject.Instantiate(this.transform.Find("Image").gameObject);

        //避免遮罩，将其设为Canvas子物体
        dragObj.transform.SetParent(canvas);
        dragObj.transform.position = this.transform.Find("Image").position;

        //将图片换为默认无物品的图片
        this.transform.Find("Image").GetComponent<Image>().sprite = defaultSprite;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //销毁拖拽框
        destoryDragObj();

        //更新UI
        UpdataBagPanelUnit();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.tag=="BagPanelUnit")
        {
            var go = this.bagUnit;
            this.bagUnit = eventData.pointerDrag.transform.GetComponent<BagPanelUnit>().bagUnit;
            eventData.pointerDrag.transform.GetComponent<BagPanelUnit>().bagUnit = go;

            this.UpdataBagPanelUnit();
            eventData.pointerDrag.transform.GetComponent<BagPanelUnit>().UpdataBagPanelUnit();
        }
    }

    protected  void destoryDragObj()
    {
        if(dragObj != null)
        {
            Destroy(dragObj);

            dragObj = null;
        }
    }
}
