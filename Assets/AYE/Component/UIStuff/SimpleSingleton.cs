using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>單例設計模式底層</summary>
public class SimpleSingleton<T> where T : class, new()
{
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
    static T _instance = null;

    public bool isSingletonLoad = false;
    public void LoadSingleton()
    {
        if (isSingletonLoad == true)
            return;
        isSingletonLoad = true;
        OnSingletonLoad();
    }
    virtual public void OnSingletonLoad()
    {
        Debug.Log(typeof(T).ToString() + " loading completed");
    }
}
