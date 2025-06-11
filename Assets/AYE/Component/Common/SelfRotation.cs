using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public bool runByBool
    {
        get { return _runByBool; }
        set { _runByBool = value; }
    }
    [Header("主動跟隨母物件")] [SerializeField, SetProperty("runByBool")]
    bool _runByBool = false;
    [Header("大於這個角度跟隨母物件")]
    [SerializeField] float maximumAngle = 90f;
    [Header("跟隨之後持續時間")]
    [SerializeField] float lastingTime = 0.5f;
    [Header("跟隨之後旋轉速度")]
    [SerializeField] float lastingTimeRotateSpeed = 400f;
    [Header("大於角度時無視速度強制跟上")]
    [SerializeField] bool forcedAngle = true;

    float targetTime = 0;
    Quaternion myQuaternion = Quaternion.identity;
    private void Start()
    {
        myQuaternion = this.transform.rotation;
    }
    void Update()
    {
        float angle = Quaternion.Angle(myQuaternion, this.transform.parent.rotation);
        if (angle > maximumAngle || _runByBool)
        {
            targetTime = Time.time + lastingTime;
            if (forcedAngle)
                myQuaternion = Quaternion.RotateTowards(myQuaternion, this.transform.parent.rotation, Mathf.Max(angle - maximumAngle, 0f));
        }
        if (Time.time < targetTime)
        {
            myQuaternion = Quaternion.RotateTowards(myQuaternion, this.transform.parent.rotation, Time.deltaTime * lastingTimeRotateSpeed);
        }
        this.transform.rotation = myQuaternion;
    }
}