using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UserInputPanel : MonoBehaviour 
{
    //控制面板切换
    LoginSceneMainCtl loginSceneMainCtl;

    //管控面板元素
    InputField NameInputField;
    InputField PasswordInputField;

    Button EnterButton;
    Button RegisterButton;
    Button EscButton;

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

        EnterButton = this.transform.Find("EnterButton").GetComponent<Button>();
        EnterButton.onClick.AddListener(onLogin);

        RegisterButton = this.transform.Find("RegisterButton").GetComponent<Button>();
        RegisterButton.onClick.AddListener(onRegister);

        EscButton = this.transform.Find("EscButton").GetComponent<Button>();
        EscButton.onClick.AddListener(onExit);

        WarningText = this.transform.Find("WarningText").GetComponent<Text>();

        //重置控件信息
        initInfo();
	}

    void initInfo()
    {
        NameInputField.text = "";
        PasswordInputField.text = "";

        EnterButton.interactable = false;
        WarningText.text = "";
    }

    //检查确认输入框不为空
    void onCheck(string context)
    {
        if (!string.IsNullOrEmpty(NameInputField.text) && !string.IsNullOrEmpty(PasswordInputField.text))
        {
            EnterButton.interactable = true;
        }
    }

    //登录按钮回调
    void onLogin()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        if (LoginModel.Instance.LoginSuccess(NameInputField.text, PasswordInputField.text))
        {
            //密码验证成功
            LoginSuccess();
        }
        else
        {
            //密码验证失败
            WarningText.text = "登录失败!";

            PasswordInputField.text = "";
        }
    }

    //注册按钮回调
    void onRegister()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //重置控件信息
        initInfo();

        //显示注册面板
        loginSceneMainCtl.ShowRegisterPanel();
    }

    //退出按钮回调
    void onExit()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        Application.Quit();
    }

    //登录成功
    void LoginSuccess()
    {
        PlayerDefine.Instance.UserID = NameInputField.text;
        PlayerDefine.Instance.ServerName = "深圳一区";

        //重置控件信息
        initInfo();

        loginSceneMainCtl.ShowCreatRolePanel();
    }

}
