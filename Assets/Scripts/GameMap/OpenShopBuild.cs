using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class OpenShopBuild : MonoBehaviour
{
    public CampType campType;
    public GameObject MenuPanel;

    CanvasGroup group;

    // Use this for initialization
    void Start ()
	{
        group = MenuPanel.transform.GetComponent<CanvasGroup>();

    }

    private void OnMouseUpAsButton()
    {
        if(campType==GameDefine.camp)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;

            group.transform.SetAsLastSibling();
            group.transform.localPosition = Vector3.zero;
        }
        
    }

}
