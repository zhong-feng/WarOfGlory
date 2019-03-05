using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    public int MouseButton = 1;

    public float RotateSpeed = 80.0f;
    public int xMinAngle = -10;
    public int xMaxAngle = 70;

    public float ZoomSpeed = 10.0f;
    public float Distance = 12;
    public float minDistance = 3;
    public float MaxDistance = 20;

    [HideInInspector]
    public Vector3 Offset;

    [HideInInspector]
    public float FollowSpeed = 10;

    private void LateUpdate()
    {
        float t = FollowSpeed * Time.deltaTime;
        
        if(Target != null)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, Target.position + Offset, t);
        }
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        //Input类，鼠标输入相关的成员方法
        if ( Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1))
        {
            var xDelta = Input.GetAxis("Mouse X");
            var yDelta = Input.GetAxis("Mouse Y");

            if (xDelta != 0 && yDelta != 0)
            {
                ////水平方向旋转
                //this.transform.RotateAround(Target.transform.position, Target.transform.up, RotateSpeed * Time.deltaTime * xDelta);
                //Offset = this.transform.position - Target.transform.position;

                //垂直方向旋转
                Vector3 prePosition = transform.position;
                Quaternion preRotation = transform.rotation;

                this.transform.RotateAround(Target.transform.position, this.transform.right, RotateSpeed * Time.deltaTime * -yDelta);
                Offset = this.transform.position - Target.transform.position;

                //影响视野的属性的有position，rotation
                //得到旋转角度，超出限制则让属性恢复原样，使其旋转无效
                float agule = transform.eulerAngles.x;

                if (agule > xMaxAngle || agule < xMinAngle)
                {
                    Offset = prePosition - Target.transform.position;
                    transform.position = prePosition;
                    transform.rotation = preRotation;
                }

            }
        } //END IF 按键 

        //相机视野拉近和拉远
        //var distance = offset.magnitude;
        Distance += -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        Distance = Mathf.Clamp(Distance, minDistance, MaxDistance);
        Offset = Offset.normalized * Distance;//让相机移动到这个位置

    }


}
