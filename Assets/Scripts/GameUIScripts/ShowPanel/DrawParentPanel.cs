using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawParentPanel : MonoBehaviour, IDragHandler
{
    Transform parentPanel;

    // Use this for initialization
    void Start ()
	{
        parentPanel = this.transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //记录鼠标当前位置
        Vector2 mousePosInScene = eventData.position;

        //if(mousePosInScene.x <0 || mousePosInScene.x > Screen.height
        //    || mousePosInScene.y < 0 || mousePosInScene.y > Screen.width)
        //{
        //    return;
        //}


        parentPanel.Translate(eventData.delta);    

    }


}
