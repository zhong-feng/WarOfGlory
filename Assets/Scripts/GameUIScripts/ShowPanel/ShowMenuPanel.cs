using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenuPanel : MonoBehaviour
{
    public GameObject MenuPanel;
    
    CanvasGroup group;

    private void Start()
    {
        group = MenuPanel.transform.GetComponent<CanvasGroup>();

        closePanel();
    }
    
    public void ShowPanel()
    {
        //播放声音
        AudioSourceSystem.Instance.PlayerVoiceWithPrefab(AudioSourceType.AT_UI_OnClick);

        if (group.alpha == 1)
        {
            closePanel();
        }
        else
        {
            showPanel();
        }
    }

    void closePanel()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
    
    void showPanel()
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;

        group.transform.SetAsLastSibling();
        group.transform.localPosition = Vector3.zero;

    }


}
