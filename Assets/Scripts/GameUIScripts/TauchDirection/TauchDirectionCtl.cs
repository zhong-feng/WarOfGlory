using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauchDirectionCtl : MonoBehaviour
{
    public float distance=1.0f;
	
	// Update is called once per frame
	void Update ()
	{
        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        
        if(Mathf.Abs( xDelta - Mathf.Epsilon) > 0 || Mathf.Abs(yDelta - Mathf.Epsilon) > 0)
        {
            Vector2 vec = new Vector2(xDelta, yDelta);

            this.transform.localPosition = vec.normalized * distance;
            
        }
        else
        {
            this.transform.localPosition = Vector3.zero;
        }
    }


}
