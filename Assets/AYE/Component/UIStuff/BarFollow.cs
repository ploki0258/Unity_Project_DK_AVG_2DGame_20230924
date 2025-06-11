using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarFollow : MonoBehaviour
{
    [Header("跟隨對象")][SerializeField] Image targetBar;
    [Header("我的ImageBar")][SerializeField] Image myBar;
    [Header("延遲多久")][SerializeField] float delay = 0.5f;
    [Header("跟隨速度")][SerializeField] float speed = 0.2f;
    [Header("跟隨模式")] [SerializeField] FollowType followType = FollowType.Max;
    private void Reset()
    {
        myBar = GetComponent<Image>();
    }
    void Start()
    {
        myBar.fillAmount = targetBar.fillAmount;
        lastTargetBarValue = targetBar.fillAmount;
    }
    float lastTargetBarValue;
    float delayTimer;
    private void LateUpdate()
    {
        if (targetBar.fillAmount < lastTargetBarValue)
            delayTimer = Time.time + delay;
        lastTargetBarValue = targetBar.fillAmount;
        if (Time.time > delayTimer)
        {
            myBar.fillAmount = Mathf.Lerp(myBar.fillAmount, targetBar.fillAmount, speed * Time.deltaTime);
        }
        if (followType == FollowType.Max)
        {
            myBar.fillAmount = Mathf.Max(myBar.fillAmount, targetBar.fillAmount);
        }
        else
        {
            myBar.fillAmount = Mathf.Min(myBar.fillAmount, targetBar.fillAmount);
        }
    }
    public enum FollowType
    {
        Max, Min
    }
}
