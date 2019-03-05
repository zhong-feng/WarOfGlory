using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public enum GameMapType
{
    GMT_None,
    GMT_Type_01=2,
    GMT_Type_02
}

public class GameController : MonoSingleton<GameController> 
{
    //玩家的信息
    public HeroCtl Player;

    public HeroBirthPlace RedHeroBirthPlace;
    public HeroBirthPlace BlueHeroBirthPlace;

    //外部引入方向盘
    public Transform TauchDirection;

    //设置第三人称摄像机
    public ThirdPersonCamera MainCamera;

    public override void OnInitSingleton()
    {
        string path = DataCacheSystem.Instance.GetMonsterInfo(GameDefine.Hero).modelPath;
        GameObject go = GameObject.Instantiate(ResourcesManager.Load<GameObject>(path));

        Player = go.GetComponent<HeroCtl>();

        //设置属性
        Player.MyCampType = GameDefine.camp;
        Player.MyProp.campType = GameDefine.camp;

        //禁用AI
        Player.GetComponent<MonsterAI>().enabled = false;


        //设置位置
        switch (Player.MyCampType)
        {
            case CampType.CT_Red:
                {
                    go.transform.SetParent(RedHeroBirthPlace.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.SetParent(null);

                    go.GetComponent<HeroCtl>().BirthPlace = RedHeroBirthPlace;

                    go.transform.Find("MiniMap").Find("Blue").gameObject.SetActive(false);

                    break;
                }
            case CampType.CT_Blue:
                {
                    go.transform.SetParent(BlueHeroBirthPlace.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.SetParent(null);

                    go.GetComponent<HeroCtl>().BirthPlace = BlueHeroBirthPlace;

                    go.transform.Find("MiniMap").Find("Red").gameObject.SetActive(false);
                    break;
                }
        }
        
        //设置小地图摄像机
        GameObject miniCamera = GameObject.Find("MiniCamera");
        miniCamera.transform.SetParent(go.transform.Find("MiniMap").Find("MiniMapPoint"));
        miniCamera.transform.localPosition = Vector3.zero;
        //miniCamera.transform.SetParent(null);
        miniCamera.SetActive(false);
        StartCoroutine(UpdataMiniMapCamera(miniCamera));

        //设置 RenderImage 
        GameObject RenderImagePoint = GameObject.Find("AcquisitionRenderImagePoint");
        GameObject HeroRenderImage = GameObject.Instantiate(go);
        HeroRenderImage.GetComponent<MonsterCtl>().enabled = false;
        HeroRenderImage.GetComponent<MonsterAI>().enabled = false;
        HeroRenderImage.GetComponent<NavMeshAgent>().enabled = false;

        HeroRenderImage.transform.SetParent(RenderImagePoint.transform.Find("PlayerPoint"));
        HeroRenderImage.transform.localPosition = Vector3.zero;

        //设置第三人称摄像机
        Vector3 cameraPoint = Player.transform.position + Vector3.up * 30 - Vector3.forward * 30;
        MainCamera.transform.position = cameraPoint;

        MainCamera.transform.LookAt(Player.transform.position);
        MainCamera.Target = Player.transform;
        MainCamera.Offset= MainCamera.transform.position - Player.transform.position;

        //设置 声音系统
        GameObject audioSystem = GameObject.FindGameObjectWithTag("AudioSourceSystem");
        if(audioSystem == null)
        {
            audioSystem = GameObject.Instantiate(ResourcesManager.Load<GameObject>("Prefabs/AudioSourceSystem/AudioSourceSystem"));
        }
        
        audioSystem.transform.localPosition = Vector3.zero;

    }

    IEnumerator UpdataMiniMapCamera(GameObject miniMapCamera)
    {
        yield return null;
        miniMapCamera.SetActive(true);
    }

    private void Update()
    {
        //移动
        StartMove();

        //停止
        StopMove();

    }

    void StartMove()
    {
        ////状态不可变时 不可移动
        //if ((int)Player.MyProp.CurStatus > (int)StatusType.ST_NoBreak)
        //{
        //    return;
        //}

        //-----------------------------轮盘模拟移动-----------------------
        if (TauchDirection != null && TauchDirection.localPosition != Vector3.zero)
        {
            //中断自动攻击
            GameDefine.inAutoAttack = false;

            //状态不可变时 不可移动
            if ((int)Player.MyProp.CurStatus > (int)StatusType.ST_NoBreak)
            {
                return;
            }

            //停止导航
            Player.navMeshAgent.isStopped = true;

            Vector3 go = new Vector3(TauchDirection.localPosition.x, 0, TauchDirection.localPosition.y);

            Vector3 target = Player.transform.position + go.normalized * 10.0f;
            Player.transform.LookAt(target);
            Player.transform.Translate(go.normalized * Time.deltaTime * 10.0f,Space.World);

            Player.UpdataAnimation(StatusType.ST_Run);



            return;
        }
        else
        {
            //状态不可变时 不可移动
            if ((int)Player.MyProp.CurStatus > (int)StatusType.ST_NoBreak)
            {
                return;
            }

            if(Player.navMeshAgent.isStopped)
            {
                Player.UpdataAnimation(StatusType.ST_Idle);
            }
        }
        

        //-----------------------------鼠标点击移动-----------------------
        //当前鼠标在UI元素上  不可移动
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //左键移动
        if (Input.GetMouseButtonDown(0))
        {
            //中断自动攻击
            GameDefine.inAutoAttack = false;

            //状态不可变时 不可移动
            if ((int)Player.MyProp.CurStatus > (int)StatusType.ST_NoBreak)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Player.navMeshAgent.isStopped = false;

                Player.navMeshAgent.destination = hit.point;
            }

            Player.UpdataAnimation(StatusType.ST_Run);

        }
    }


    void StopMove()
    {
        //状态不可变时 不可移动
        if ((int)Player.MyProp.CurStatus > (int)StatusType.ST_NoBreak)
        {
            return;
        }

        //已经停止 
        if (Player.MyProp.CurStatus == StatusType.ST_Idle)
        {
            return;
        }
        
        if(Player.navMeshAgent.isStopped)
        {
            return;
        }

        //当前位置 与目标位置重合
        if ((Player.navMeshAgent.destination - Player.transform.position).magnitude <= Mathf.Epsilon)
        {
            Player.UpdataAnimation(StatusType.ST_Idle);
            Player.navMeshAgent.isStopped = true;
        }


    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(this.transform.position, Player.navMeshAgent.destination);

    //    ////确认攻击范围
    //    //var radius = monsterProp.MyProp.attackRadius;

    //    //Gizmos.color = Color.yellow;
    //    //Gizmos.DrawWireSphere(this.transform.position, radius * 1.5f);

    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawWireSphere(this.transform.position, radius);
    //}



}
