using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCtl : MonoBehaviour 
{
    //全地图查看点
    public Vector3 TargetMiniCameraPos;
    //地图摄像机
    public Camera MiniCamera;            

    GameObject AssistRaycaseTarget;   //辅助 检测点击面板
    Vector3 StartMiniCameraPos;

    bool isDoubleSize = false;
	// Use this for initialization
	void Start () 
	{
        AssistRaycaseTarget = this.transform.Find("AssistRaycaseTarget").gameObject;
        AssistRaycaseTarget.SetActive(false);
	}

    //放大小地图
    public void DoubleSizeMyself()
    {
        if (isDoubleSize == false)
        {
            isDoubleSize = true;

            //记录当前摄像机的位置
            StartMiniCameraPos = MiniCamera.transform.position;

            //将摄像机移到目标点
            MiniCamera.transform.position = TargetMiniCameraPos;

            //放大自己
            RectTransform go = this.transform as RectTransform;
            go.sizeDelta *= 2;

            //辅助 检测 激活
            AssistRaycaseTarget.SetActive(true);

            //
            GameDefine.CanController = false;

            //设置在 Canvas 最前面
            this.transform.SetAsLastSibling();
        }


    }

    //缩小小地图
    public void HalfSizeMyself()
    {
        //缩小自己
        RectTransform go = this.transform as RectTransform;
        go.sizeDelta /= 2;

        isDoubleSize = false;

        //辅助 检测 去激活
        AssistRaycaseTarget.SetActive(false);

        GameDefine.CanController = true;

        //将摄像机移回去
        MiniCamera.transform.position = StartMiniCameraPos;


    }
}
