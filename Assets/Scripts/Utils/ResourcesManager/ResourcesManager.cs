using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    //泛型加载
    static Hashtable resTable = new Hashtable();

    //泛型约束，明确特化时，类型形参必须是继承于UnityEngine.Object
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        if (resTable.ContainsKey(path))
        {
            return resTable[path] as T;
        }

        //var goSprite = Resources.Load<Sprite>(iconPath);
        T t = Resources.Load<T>(path);
        resTable[path] = t;

        return t;
    }


    //Resources目录加载资源（缓存）
    static Dictionary<string, GameObject> resDic = new Dictionary<string, GameObject>();

    //非泛型用于加载预置体对象
    public static GameObject Load(string path)
    {
        if (resDic.ContainsKey(path))
        {
            return resDic[path];
        }

        // var goPrefab = Resources.Load("Prefab//BagUnit") as GameObject;
        GameObject go = Resources.Load(path) as GameObject;
        resDic[path] = go;

        return go;
    }

}
