using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneMainCtl : MonoBehaviour 
{
    GameObject UserInputPanel;
    GameObject RegisterPanel;
    GameObject ChooseSeverPanel;
    GameObject CreatRolePanel;
    GameObject LoginPanel;

	// Use this for initialization
	void Start () 
	{
        UserInputPanel = this.transform.Find("UserInputPanel").gameObject;
        RegisterPanel = this.transform.Find("RegisterPanel").gameObject;
        ChooseSeverPanel = this.transform.Find("ChooseSeverPanel").gameObject;
        CreatRolePanel = this.transform.Find("CreatRolePanel").gameObject;
        LoginPanel = this.transform.Find("LoginPanel").gameObject;


        if (PlayerDefine.Instance.UserID == "")
        {
            ShowUserInputPanel();
        }
        else if(PlayerDefine.Instance.ServerName=="" )
        {
            //ShowChooseSeverPanel();
            ShowLoginPanel();

        }
        else
        {
            ShowLoginPanel();
        }


	}

    bool isFindAll()
    {
        if (UserInputPanel == null)
        {
            return false;
        }

        if (RegisterPanel == null)
        {
            return false;
        }

        if (ChooseSeverPanel == null)
        {
            return false;
        }

        if (CreatRolePanel == null)
        {
            return false;
        }

        if (LoginPanel == null)
        {
            return false;
        }

        return true;
    }

    //显示输入账号面板
    public void ShowUserInputPanel()
    {
        //判空
        if (isFindAll() == true)
        {
            UserInputPanel.SetActive(true);
            RegisterPanel.SetActive(false);
            ChooseSeverPanel.SetActive(false);
            CreatRolePanel.SetActive(false);
            LoginPanel.SetActive(false);
        }
    }

    //显示注册面板
    public void ShowRegisterPanel()
    {
        if (isFindAll() == true)
        {
            UserInputPanel.SetActive(false);
            RegisterPanel.SetActive(true);
            ChooseSeverPanel.SetActive(false);
            CreatRolePanel.SetActive(false);
            LoginPanel.SetActive(false);
        }
    }

    //显示服务器选择面板
    public void ShowChooseSeverPanel()
    {
        if (isFindAll() == true)
        {
            UserInputPanel.SetActive(false);
            RegisterPanel.SetActive(false);
            ChooseSeverPanel.SetActive(true);
            CreatRolePanel.SetActive(false);
            LoginPanel.SetActive(false);
        }
    }

    //显示创建角色面板
    public void ShowCreatRolePanel()
    {
        if (isFindAll() == true)
        {
            UserInputPanel.SetActive(false);
            RegisterPanel.SetActive(false);
            ChooseSeverPanel.SetActive(false);
            CreatRolePanel.SetActive(true);
            LoginPanel.SetActive(false);
        }
    }

    //显示登录面板
    public void ShowLoginPanel()
    {
        if (isFindAll() == true)
        {
            UserInputPanel.SetActive(false);
            RegisterPanel.SetActive(false);
            ChooseSeverPanel.SetActive(false);
            CreatRolePanel.SetActive(false);
            LoginPanel.SetActive(true);
        }
    }
}
