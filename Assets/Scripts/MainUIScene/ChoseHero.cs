using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChoseHero : MonoBehaviour 
{
    public MonsterType HeroType;
    public GameObject HeroModel;

    public void Start()
    {
        this.GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }



    public void OnValueChanged(bool isOn)
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        if (isOn)
        {
            HeroModel.SetActive(true);
            GameDefine.Hero = HeroType;
        }
        else
        {
            HeroModel.SetActive(false);
        }
    }
}
