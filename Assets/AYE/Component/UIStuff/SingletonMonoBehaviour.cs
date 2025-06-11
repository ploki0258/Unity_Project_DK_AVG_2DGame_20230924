using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
{
    static public T instance = null;
    virtual protected void Awake()
    {
        instance = this as T;
    }
}
