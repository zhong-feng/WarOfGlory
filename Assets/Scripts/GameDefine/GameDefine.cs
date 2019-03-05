using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine  
{
    public static float MonsterMoveSpeed = 8.0f;
    public static float NeutralChaseRadius = 15.0f;
    public static bool CanController = true;
    public static bool IsBlueCampType=true;

    public static bool isViceOn = true;
    public static bool isAutoAttack = true;
    public static bool inAutoAttack = true;

    public static GameMapType CurGameMapType = GameMapType.GMT_Type_02;  // 确定游戏地图
    public static MonsterType Hero = MonsterType.MT_Jinglingnan;      // 确定玩家角色
    public static CampType camp = CampType.CT_Blue;                   // 确定玩家阵营
}
