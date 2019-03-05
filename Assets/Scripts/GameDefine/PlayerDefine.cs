using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SexType
{
    ST_None,
    ST_Man,
    ST_Woman
}

public struct BasePlayer
{
    public string PlayerID;
    public string UserID;
    public int ServerNum;

    public string Name;
    public SexType Sex;

    public int HeadImageId;
}


public class PlayerDefine
{
    #region 单例类

    private static PlayerDefine _instance=null;
    public static PlayerDefine Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new PlayerDefine();
            }

            return _instance;
        }
    }

    private PlayerDefine()
    {

    }

    #endregion

    public string UserID="";
    public string ServerName="";

    public string Name="";
    public SexType Sex=SexType.ST_None;

    public Sprite HeadImage=null;

    public SceneType NextScene=SceneType.ST_None;
}
