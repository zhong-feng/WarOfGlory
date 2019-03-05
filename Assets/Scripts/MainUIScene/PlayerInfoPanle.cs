using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInfoPanle : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
        if (PlayerDefine.Instance.HeadImage != null)
        {
            this.transform.Find("HeadImage").GetComponent<Image>().sprite = PlayerDefine.Instance.HeadImage;
        }

        if(PlayerDefine.Instance.Name != "")
        {
            this.transform.Find("PlayerName").GetComponent<Text>().text = PlayerDefine.Instance.Name;
        }

        if (PlayerDefine.Instance.UserID != "")
        {
            this.transform.Find("UserName").GetComponent<Text>().text = PlayerDefine.Instance.UserID;
        }

        if (PlayerDefine.Instance.ServerName != "")
        {
            this.transform.Find("SeverName").GetComponent<Text>().text = PlayerDefine.Instance.ServerName;
        }

        if(PlayerDefine.Instance.Sex == SexType.ST_Man)
        {
            this.transform.Find("Sex").GetComponent<Text>().text ="男";
        }
        else if(PlayerDefine.Instance.Sex == SexType.ST_Woman)
        {
            this.transform.Find("Sex").GetComponent<Text>().text = "女";
        }

        this.transform.Find("ChangePlayerButton").GetComponent<Button>().onClick.AddListener(onChangePlayer);
        this.transform.Find("ChangeUserButton").GetComponent<Button>().onClick.AddListener(onChangeUser);
        this.transform.Find("ESCButton").GetComponent<Button>().onClick.AddListener(onEsc);

	}
	

    void onChangePlayer()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        PlayerDefine.Instance.Name = "";
        PlayerDefine.Instance.ServerName = "";
        PlayerDefine.Instance.Sex = SexType.ST_None;
        PlayerDefine.Instance.HeadImage = null;

        PlayerDefine.Instance.NextScene = SceneType.ST_LoginScene;

        //从异步加载场景切换到目标场景
        SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Single);
    }

    void onChangeUser()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        PlayerDefine.Instance.UserID = "";
        PlayerDefine.Instance.Name = "";
        PlayerDefine.Instance.ServerName = "";
        PlayerDefine.Instance.Sex = SexType.ST_None;
        PlayerDefine.Instance.HeadImage = null;

        PlayerDefine.Instance.NextScene = SceneType.ST_LoginScene;

        //从异步加载场景切换到目标场景
        SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Single);
    }

    void onEsc()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        Application.Quit();
    }


}
