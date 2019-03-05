using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtl_Type_02 : GameController
{
    private void Start()
    {
        AudioSourceSystem.Instance.PlayBackgroundMusic(AudioSourceType.AT_BG_Map02);
    }

}
