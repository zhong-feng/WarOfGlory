using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    ET_None,

    ET_Attack,   //攻击
    ET_Death,    //死亡

    ET_ChangeViceSet, //更新声音设置
}

public class MyEventSystem
{
    static Dictionary<EventType, Action<object, object>> EventDic = new Dictionary<EventType, Action<object,object>>();

    public static void AddEventListener(EventType eventName, Action<object, object> action)
    {
        if (EventDic.ContainsKey(eventName))
        {
            EventDic[eventName] += action;
        }
        else
        {
            EventDic[eventName] = action;
        }
    }

    public static void DelEventListener(EventType eventName, Action<object, object> action)
    {
        if (EventDic.ContainsKey(eventName))
        {
            EventDic[eventName] -= action;

            if (EventDic[eventName] == null)
            {
                EventDic.Remove(eventName);
            }
        }
    }

    public static void DispatchEventMsg(EventType eventName, object obj1, object obj2)
    {
        if (EventDic.ContainsKey(eventName))
        {
            EventDic[eventName](obj1,obj2);
        }
    }


}
