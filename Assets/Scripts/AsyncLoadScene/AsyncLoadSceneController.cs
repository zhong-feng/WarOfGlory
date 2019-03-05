using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneType
{
    ST_LoginScene,
    ST_MainUIScene,
    ST_Map01_2v2,
    ST_Map02_1v1,
    ST_AsyncLoadScene,
    
    ST_None,
}

public class AsyncLoadSceneController : MonoBehaviour 
{
    AsyncOperation option;

    Slider progressSlider;
    Text progressText;

	// Use this for initialization
	void Start () 
	{
        progressSlider = transform.Find("ProgressSlider").GetComponent<Slider>();
        progressSlider.value = 0;

        progressText = transform.Find("ProgressText").GetComponent<Text>();
        progressText.text = "";


        StartCoroutine(AsyncLoadScene());
	}
	
	// Update is called once per frame
	void Update () 
	{
        progressSlider.value = option.progress;
        progressText.text = "加载进度：" + (option.progress * 100).ToString("0.0") + "%";
	}

    IEnumerator AsyncLoadScene()
    {
        if (PlayerDefine.Instance.NextScene==SceneType.ST_None)
        {
            PlayerDefine.Instance.NextScene = SceneType.ST_MainUIScene;
        }

        //根据外部数据确定加载场景
        option = SceneManager.LoadSceneAsync((int)PlayerDefine.Instance.NextScene);
        yield return option;
    }
}
