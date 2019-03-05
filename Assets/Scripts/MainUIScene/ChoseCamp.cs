using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChoseCamp : MonoBehaviour 
{
    public CampType Camp;

    public void Start()
    {
        this.GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }

    public  void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        if (isOn)
        {
            GameDefine.camp = Camp;
        }
       
    }

}
