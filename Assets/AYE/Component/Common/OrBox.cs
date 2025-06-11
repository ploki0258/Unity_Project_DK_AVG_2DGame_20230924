using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OrBox : MonoBehaviour
{
    public bool boolA
    {
        get { return _boolA; }
        set { _boolA = value; Checking(); }
    }
    public bool boolB
    {
        get { return _boolB; }
        set { _boolB = value; Checking(); }
    }
    public bool boolC
    {
        get { return _boolC; }
        set { _boolC = value; Checking(); }
    }
    public bool boolD
    {
        get { return _boolD; }
        set { _boolD = value; Checking(); }
    }
    public bool boolE
    {
        get { return _boolE; }
        set { _boolE = value; Checking(); }
    }
    public bool boolF
    {
        get { return _boolF; }
        set { _boolF = value; Checking(); }
    }
    public bool boolG
    {
        get { return _boolG; }
        set { _boolG = value; Checking(); }
    }
    [SerializeField, SetProperty("boolA")]
    bool _boolA = false;
    [SerializeField, SetProperty("boolB")]
    bool _boolB = false;
    [SerializeField, SetProperty("boolC")]
    bool _boolC = false;
    [SerializeField, SetProperty("boolD")]
    bool _boolD = false;
    [SerializeField, SetProperty("boolE")]
    bool _boolE = false;
    [SerializeField, SetProperty("boolF")]
    bool _boolF = false;
    [SerializeField, SetProperty("boolG")]
    bool _boolG = false;
    [SerializeField] UnityEvent<bool> outPut;
    bool _outPut = false;
    void Checking()
    {
        bool final = _boolA || _boolB || _boolC || _boolD || _boolE || _boolF || _boolG;
        if (final != _outPut)
        {
            outPut.Invoke(final);
            _outPut = final;
            if (outPutDebug)
                Debug.Log("OrBox: " + final);
        }
    }
    [SerializeField] bool outPutOnStart = false;
    private void Start()
    {
        if (outPutOnStart)
        {
            bool final = _boolA || _boolB || _boolC || _boolD || _boolE || _boolF || _boolG;
            _outPut = final;
            outPut.Invoke(final);
            if (outPutDebug)
                Debug.Log("OrBox: " + final);
        }
    }
    [SerializeField] bool outPutDebug = false;
}
