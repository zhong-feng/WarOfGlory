using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MenuPanelLeftToggleGroupCtl : MonoBehaviour
{

    [HideInInspector]
    public Toggle[] LeftToggleArray;
    public GameObject[] MenuPanelArray;

    // Use this for initialization
    void Start ()
	{

        var toggleGroup = this.GetComponent<ToggleGroup>();

        if (toggleGroup)
        {
            //默认全部关闭
            toggleGroup.SetAllTogglesOff();

            //获取Toggle
            LeftToggleArray = toggleGroup.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < LeftToggleArray.Length; i++)
            {
                //给Toggle添加回调函数
                LeftToggleArray[i].onValueChanged.AddListener(OnValueChanged);

            }

            //默认选中第一个
            LeftToggleArray[0].isOn = true;

        }
    }

    void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        for (int i = 0; i < LeftToggleArray.Length; i++)
        {
            if (LeftToggleArray[i].isOn)
            {
                MenuPanelArray[i].SetActive(true);
            }
            else
            {
                MenuPanelArray[i].SetActive(false);
            }
        }
    }
}
