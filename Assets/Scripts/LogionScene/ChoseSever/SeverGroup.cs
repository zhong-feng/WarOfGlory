using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SeverGroup : MonoBehaviour 
{
    public Toggle choseSeverBtnPrefab;
	// Use this for initialization
	void Start () 
	{
        ToggleGroup group = this.GetComponent<ToggleGroup>();

        if(group != null)
        {
            for(int i=0;i<LoginModel.Instance.severDic.Count;i++)
            {
               // var go = Instantiate(choseSeverBtnPrefab);

            }
        }
	}
    
}
