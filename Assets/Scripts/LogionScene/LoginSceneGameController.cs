using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneGameController : MonoBehaviour
{

    private void Awake()
    {
        //设置 声音系统
        GameObject audioSystem = GameObject.FindGameObjectWithTag("AudioSourceSystem");
        if (audioSystem == null)
        {
            audioSystem = GameObject.Instantiate(ResourcesManager.Load<GameObject>("Prefabs/AudioSourceSystem/AudioSourceSystem"));
        }
        
        audioSystem.transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        //播放背景音乐
        AudioSourceSystem.Instance.PlayBackgroundMusic(AudioSourceType.AT_BG_Logion);
    }



}
