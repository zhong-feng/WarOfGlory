﻿using System;
using System.Collections.Generic;
using UnityEngine;
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region 继承MonoBehaviour的单例模板代码
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //该脚本组件对象的引用
        _instance = this as T;

        //预留接口
        OnInitSingleton();
    }

    public virtual void OnInitSingleton()
    {

    }

    #endregion
}

