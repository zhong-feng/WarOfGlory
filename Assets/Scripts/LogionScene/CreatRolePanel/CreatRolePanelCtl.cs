using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CreatRolePanelCtl : MonoBehaviour 
{
    //控制面板切换
    LoginSceneMainCtl loginSceneMainCtl;

    //管控面板元素
    Image headImage;
    Toggle[] sexToggleArray;
    Toggle[] headToggleArray;
    InputField NameInputField;
    Button SubmitButton;

	// Use this for initialization
    void Awake() 
	{
        //获取相关组件
        loginSceneMainCtl = this.transform.parent.GetComponent<LoginSceneMainCtl>();

        NameInputField = this.transform.Find("NameInputField").GetComponent<InputField>();
        NameInputField.onValueChanged.AddListener(onCheck);

        SubmitButton = this.transform.Find("SubmitButton").GetComponent<Button>();
        SubmitButton.onClick.AddListener(onConfirm);

        headImage = this.transform.Find("HeadImageBorder").Find("HeadImage").GetComponent<Image>();

        var sexGroup = this.transform.Find("SexChoose").GetComponent<ToggleGroup>();
        if (sexGroup)
        {
            //获取Toggle
            sexToggleArray = sexGroup.GetComponentsInChildren<Toggle>();
        }

        var headGroup = this.transform.Find("HeadChoose").GetComponent<ToggleGroup>();
        if (headGroup)
        {
            //获取Toggle
            headToggleArray = headGroup.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < headToggleArray.Length; i++)
            {
                //给Toggle添加回调函数
                headToggleArray[i].onValueChanged.AddListener(OnValueChanged);

            }

        }

        initInfo();

	}

    void initInfo()
    {
        SubmitButton.interactable = false;
        NameInputField.text = "";
        sexToggleArray[0].isOn = true;
        headToggleArray[0].isOn = true;

    }

    //提交按钮回调
    void onConfirm()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        PlayerDefine.Instance.Name = NameInputField.text;
        PlayerDefine.Instance.HeadImage = headImage.sprite;

        for (int i = 0; i < sexToggleArray.Length; i++)
        {
            if (sexToggleArray[i].isOn)
            {
                PlayerDefine.Instance.Sex = (SexType)(i+1);
                break;
            }

        }

        //重置控件信息
        initInfo();

        //显示 进入游戏面板
        loginSceneMainCtl.ShowLoginPanel();
    }

    // 检查确认输入框不为空
    void onCheck(string context)
    {
        if (!string.IsNullOrEmpty(NameInputField.text))
        {
            SubmitButton.interactable = true;
        }
    }

    void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        for (int i = 0; i < headToggleArray.Length; i++)
        {
            if (headToggleArray[i].isOn)
            {
                headImage.sprite = headToggleArray[i].GetComponent<Transform>().Find("Image").GetComponent<Image>().sprite;

                break;
            }

        }
    }
}
