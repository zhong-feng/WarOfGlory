using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour 
{
    //控制面板切换
    LoginSceneMainCtl loginSceneMainCtl;

    //管控面板元素
    InputField NameInputField;
    InputField PasswordInputField;
    InputField ConfirmPasswordInputField;

    Button ConfirmButton;
    Button BackButton;

    Text WarningText;

	// Use this for initialization
    void Awake() 
	{
        //获取相关组件
        loginSceneMainCtl = this.transform.parent.GetComponent<LoginSceneMainCtl>();

        NameInputField = this.transform.Find("IDInputField").GetComponent<InputField>();
        NameInputField.onValueChanged.AddListener(onCheck);

        PasswordInputField = this.transform.Find("PasswordInputField").GetComponent<InputField>();
        PasswordInputField.onValueChanged.AddListener(onCheck);

        ConfirmPasswordInputField = this.transform.Find("ConfirmPasswordInputField").GetComponent<InputField>();
        ConfirmPasswordInputField.onValueChanged.AddListener(onCheck);

        ConfirmButton = this.transform.Find("ConfirmButton").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(onConfirm);

        BackButton = this.transform.Find("BackButton").GetComponent<Button>();
        BackButton.onClick.AddListener(onBack);

        WarningText = this.transform.Find("WarningText").GetComponent<Text>();

        //重置控件信息
        initInfo();
        
	}

    void initInfo()
    {
        NameInputField.text = "";
        PasswordInputField.text ="";
        ConfirmPasswordInputField.text = "";

        ConfirmButton.interactable = false;
        WarningText.text = "";
    }

    // 检查确认输入框不为空
    void onCheck(string context)
    {
        if (!string.IsNullOrEmpty(NameInputField.text) && !string.IsNullOrEmpty(PasswordInputField.text) && !string.IsNullOrEmpty(ConfirmPasswordInputField.text))
        {
            ConfirmButton.interactable = true;
        }
    }

    //显示登录面板
    void onBack()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //重置控件信息
        initInfo();

        //显示登录面板
        loginSceneMainCtl.ShowUserInputPanel();
    }

    //注册按钮回调
    void onConfirm()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //判断密码和确认密码是否一致
        if (PasswordInputField.text == ConfirmPasswordInputField.text)
        {
            if (LoginModel.Instance.RegisterSuccess(NameInputField.text, PasswordInputField.text))
            {
                //注册成功，返回登录面板
                onBack();
            }
            else
            {
                //注册失败（账号已经被注册的报错）
                RegisterError("当前账号已经被注册!", true);
            }
        }
        else
        {
            //注册失败（密码和确认密码不一致的报错）
            RegisterError("当前密码和确认密码不一致!", false);
        }
    }

    //注册失败 msg->失败信息  clearFlag->是否清空用户名输入框
    void RegisterError(string msg, bool clearFlag = false)
    {
        WarningText.text = msg;

        PasswordInputField.text = "";
        ConfirmPasswordInputField.text = "";

        NameInputField.text = clearFlag ? "" : NameInputField.text;
    }
}
