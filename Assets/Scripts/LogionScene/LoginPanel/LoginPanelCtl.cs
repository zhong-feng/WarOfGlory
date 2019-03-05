using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginPanelCtl : MonoBehaviour 
{
    //控制面板切换
    LoginSceneMainCtl loginSceneMainCtl;

    //管控面板元素
    Button UserName;
    Button ServerName;

    InputField NameInputField;

    Button LoginBtn;


	// Use this for initialization
    void Awake() 
	{
        //获取相关组件
        loginSceneMainCtl = this.transform.parent.GetComponent<LoginSceneMainCtl>();

        UserName = this.transform.Find("UserName").GetComponent<Button>();
        UserName.onClick.AddListener(ToUserPanel);

        ServerName = this.transform.Find("ServerName").GetComponent<Button>();
        ServerName.onClick.AddListener(ToServerPanel);

        NameInputField = this.transform.Find("NameInputField").GetComponent<InputField>();

        LoginBtn = this.transform.Find("LoginBtn").Find("Button").GetComponent<Button>();
        LoginBtn.onClick.AddListener(OnLogin);

        //重置控件信息
        initInfo();
	}

    //转到 UserPanel
    void ToUserPanel()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //重置控件信息
        initInfo();

        loginSceneMainCtl.ShowUserInputPanel();
    }

    //转到 ServerPanel
    void ToServerPanel()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //重置控件信息
        initInfo();

    }

    //转到 下一个场景
    void OnLogin()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        //确认下一个场景
        PlayerDefine.Instance.NextScene = SceneType.ST_MainUIScene;

        //从异步加载场景切换到目标场景
        SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Single);
    }

    public void OnEnable()
    {
        initInfo();
    }

    void initInfo()
    {
        UserName.GetComponent<Transform>().Find("Text").GetComponent<Text>().text = PlayerDefine.Instance.UserID;
        ServerName.GetComponent<Transform>().Find("Text").GetComponent<Text>().text = PlayerDefine.Instance.ServerName;

        NameInputField.text = PlayerDefine.Instance.Name;

    }
}
