using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseParentPanel : MonoBehaviour
{
    CanvasGroup group;
	// Use this for initialization
	void Start ()
	{
        group = this.transform.parent.GetComponent<CanvasGroup>();
        this.GetComponent<Button>().onClick.AddListener(closeParentPanel);
	}
	
    public void closeParentPanel()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}
