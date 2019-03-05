using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioSourceType
{
    AT_None,
    AT_BG_Logion,     //背景音乐
    AT_BG_MainUI,
    AT_BG_Map01,
    AT_BG_Map02,

    AT_Kill_First = 10,  //击杀音效 第一个  《》辅助声音控件
    AT_Kill_Double,    //第二个
    AT_Kill_Triple,    //第三个
    AT_Kill_Aced,      //更多

    AT_UI_OnClick = 20,   //点击UI控件  《》主声音控件
    AT_UI_Sell,
    AT_UI_Buy,
    AT_UI_StartGame,   //开始游戏按钮

    AT_Attack = 40,    //攻击
    AT_Skill01,
    AT_Skill02,
    AT_Revive,      //复活
    AT_LevelUp,
    AT_Backhome,
    AT_Dead,
    AT_Walk,   //移动   《》辅助声音控件


    AT_Failed01 = 60,  //游戏结束  《》主副配合
    AT_Failed02,
    AT_Victory01,
    AT_Victory02,

    AT_HomeAttacked = 70,  //己方水晶被攻击

    AT_JinglingnanOnline01,   //  《》辅助声音控件
    AT_JinglingnanOnline02,

}

public class AudioSourceSystem : MonoSingleton<AudioSourceSystem> 
{
    AudioSource BackgroundMusic;
    //AudioSource SoundEffect;
    //AudioSource SecondSoundEffect;

    public override void OnInitSingleton()
    {
        //防止切换新场景后, 该游戏对象被销毁  
        DontDestroyOnLoad(this.gameObject);

        BackgroundMusic = this.transform.Find("BackgroundMusic").GetComponent<AudioSource>();
        //SoundEffect = this.transform.Find("SoundEffect").GetComponent<AudioSource>();
        //SecondSoundEffect = this.transform.Find("SecondSoundEffect").GetComponent<AudioSource>();

        checkVoiceSet();

        MyEventSystem.AddEventListener(EventType.ET_ChangeViceSet, checkVoiceSet);
    }

    public void OnDestroy()
    {
        MyEventSystem.DelEventListener(EventType.ET_ChangeViceSet, checkVoiceSet);
    }

    //检查声音设置 （背景音乐）
    void checkVoiceSet(object obj1=null, object obj2=null )
    {
        if(GameDefine.isViceOn)
        {
            BackgroundMusic.Play();
        }
        else
        {
            BackgroundMusic.Pause();
        }
    }
    public void PlayBackgroundMusic(AudioSourceType type)
    {
        switch(type)
        {
            case AudioSourceType.AT_BG_Logion:
                {
                    BackgroundMusic.clip = ResourcesManager.Load<AudioClip>("Audio/mus_fb_login_lp");
                    BackgroundMusic.Play();
                    break;
                }
            case AudioSourceType.AT_BG_MainUI:
                {
                    BackgroundMusic.clip = ResourcesManager.Load<AudioClip>("Audio/mus_fb_PVP001_lp_seniorbattlefield");
                    BackgroundMusic.Play();
                    break;
                }
            case AudioSourceType.AT_BG_Map01:
                {
                    BackgroundMusic.clip = ResourcesManager.Load<AudioClip>("Audio/mus_fb_PVP002_lp_intermediatebattlefield");
                    BackgroundMusic.Play();
                    break;
                }
            case AudioSourceType.AT_BG_Map02:
                {
                    BackgroundMusic.clip = ResourcesManager.Load<AudioClip>("Audio/mus_fb_PVP004_lp_primarybattlefield");
                    BackgroundMusic.Play();
                    break;
                }
        }
    }



    //public void PlayVoice(AudioSourceType type)
    //{
    //    if(GameDefine.isViceOn==false)
    //    {
    //        return;
    //    }

    //    switch (type)
    //    {
    //        case AudioSourceType.AT_UI_OnClick:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tdianji");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_UI_Sell:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tchushou");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_UI_Buy:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tgoumai");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_UI_StartGame:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tkaishi");
    //                SoundEffect.Play();
    //                break;
    //            }

    //        case AudioSourceType.AT_Attack:  //===================================================
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tkaishi");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Skill01:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/SkillAttack01");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Skill02:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/SkillAttack02");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Revive:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tfuhuo");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_LevelUp:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tshengji");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Backhome:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Tkaishi");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Dead:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Jinglingnan5_Dead");
    //                SoundEffect.Play();
    //                break;
    //            }

    //        case AudioSourceType.AT_Failed01:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/GameOverDefeat");
    //                SoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Victory01:
    //            {
    //                SoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/GameOverVictory");
    //                SoundEffect.Play();
    //                break;
    //            }
    //    }
    //}

    //public void PlaySecondVoice(AudioSourceType type)
    //{
    //    if (GameDefine.isViceOn == false)
    //    {
    //        return;
    //    }

    //    switch (type)
    //    {
    //        case AudioSourceType.AT_Kill_First:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/First_blood");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Kill_Double:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Double_kill");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Kill_Triple:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Triple_kill");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Kill_Aced:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Aced");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_HomeAttacked:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/HomeAttack");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_JinglingnanOnline01:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Jinglingnan_5_line_01");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_JinglingnanOnline02:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Jinglingnan_5_line_02");
    //                SecondSoundEffect.Play();
    //                break;
    //            }

    //        case AudioSourceType.AT_Walk:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/Walk_01");
    //                SecondSoundEffect.Play();
    //                break;
    //            }


    //        case AudioSourceType.AT_Failed02:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/GameOverFailed");
    //                SecondSoundEffect.Play();
    //                break;
    //            }
    //        case AudioSourceType.AT_Victory02:
    //            {
    //                SecondSoundEffect.clip = ResourcesManager.Load<AudioClip>("Audio/GameOverVictory02");
    //                SecondSoundEffect.Play();
    //                break;
    //            }

    //    }

    //}



    public void PlayerVoiceWithPrefab(AudioSourceType type ,Transform ParentObj =null )
    {
        if (GameDefine.isViceOn == false)
        {
            return;
        }

        GameObject AudioPrefab=null;

        switch(type)
        {
             case AudioSourceType.AT_UI_OnClick:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tdianji");
                    break;
                }
            case AudioSourceType.AT_UI_Sell:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tchushou");
                    break;
                }
            case AudioSourceType.AT_UI_Buy:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tgoumai");
                    break;
                }
            case AudioSourceType.AT_UI_StartGame:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tkaishi");
                    break;
                }

            case AudioSourceType.AT_Attack: 
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Attack");
                    break;
                }
            case AudioSourceType.AT_Skill01:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/SkillAttack01");
                    break;
                }
            case AudioSourceType.AT_Skill02:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/SkillAttack02");
                    break;
                }
            case AudioSourceType.AT_Revive:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tfuhuo");
                    break;
                }
            case AudioSourceType.AT_LevelUp:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Tshengji");
                    break;
                }
            case AudioSourceType.AT_Backhome:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/BackHome");
                    break;
                }
            case AudioSourceType.AT_Dead:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Jinglingnan5_Dead");
                    break;
                }

            case AudioSourceType.AT_Failed01:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/GameOverDefeatBegin");
                    break;
                }
            case AudioSourceType.AT_Victory01:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/GameOverVictoryBegin");
                    break;
                }
                 case AudioSourceType.AT_Kill_First:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/First_blood");
                    break;
                }
            case AudioSourceType.AT_Kill_Double:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Double_kill");
                    break;
                }
            case AudioSourceType.AT_Kill_Triple:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Triple_kill");
                    break;
                }
            case AudioSourceType.AT_Kill_Aced:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Aced");
                    break;
                }
            case AudioSourceType.AT_HomeAttacked:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/HomeAttack");
                    break;
                }
            case AudioSourceType.AT_JinglingnanOnline01:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Jinglingnan_5_line_01");
                    break;
                }
            case AudioSourceType.AT_JinglingnanOnline02:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Jinglingnan_5_line_02");
                    break;
                }

            case AudioSourceType.AT_Walk:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/Walk_01");
                    break;
                }


            case AudioSourceType.AT_Failed02:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/GameOverFailedEnd");
                    break;
                }
            case AudioSourceType.AT_Victory02:
                {
                    AudioPrefab = ResourcesManager.Load<GameObject>("Prefabs/Audio/GameOverVictoryend");
                    break;
                }
            default: break;
        }

        if (AudioPrefab != null)
        {
            var go = GameObject.Instantiate(AudioPrefab).GetComponent<AudioSource>();

            if(ParentObj ==null)
            {
                go.transform.SetParent(this.transform);
            }
            else
            {
                go.transform.SetParent(ParentObj);
            }

            go.transform.localPosition = Vector3.zero;

            StartCoroutine(DestroyStopAudioSource(go));
        }
    }

    IEnumerator DestroyStopAudioSource(AudioSource audio)
    {
        while(true)
        {
            if(audio ==null)
            {
                break;
            }

            if(audio.isPlaying == false)
            {
                GameObject.Destroy(audio.gameObject);

                break;
            }

            yield return new WaitForSeconds(1);
        }
    }

}
