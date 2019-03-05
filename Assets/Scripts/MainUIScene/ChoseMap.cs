using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChoseMap : MonoBehaviour 
{
    public GameMapType ToGameMap;
	// Use this for initialization
	void Start () 
	{
        this.GetComponent<Button>().onClick.AddListener(onClick);

	}
	
    void onClick()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_StartGame);

        GameDefine.CurGameMapType = ToGameMap;
        PlayerDefine.Instance.NextScene = (SceneType)ToGameMap;

        //从异步加载场景切换到目标场景
        SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Single);
    }
}
