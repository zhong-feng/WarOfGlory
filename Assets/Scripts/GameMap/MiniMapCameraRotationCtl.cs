using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraRotationCtl : MonoBehaviour 
{
    public void LateUpdate()
    {
        this.transform.eulerAngles  = new Vector3(90, 0, 0);
    }


}
