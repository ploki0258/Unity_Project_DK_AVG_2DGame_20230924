using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>簡單的讓UI跟隨世界座標中的物件位置</summary>
public class WorldUI : MonoBehaviour
{
    [Header("讓UI跟隨世界座標中的物件位置")]
    [SerializeField] Transform 參考位置用的3D物件;
    [SerializeField] RectTransform 根物件RectTransform;
    [SerializeField] Camera 攝影機;
    [SerializeField] float 一公尺處的尺寸 = 2.5f;
    [SerializeField] bool 限制最小最大尺寸 = false;
    [SerializeField] float 最小尺寸 = 0.5f;
    [SerializeField] float 最大尺寸 = 3f;
    [SerializeField] bool 啟用緩衝跟隨 = false;
    [SerializeField] 緩衝跟隨邏輯 緩衝邏輯 = 緩衝跟隨邏輯.Lerp;
    [SerializeField] float 緩衝參數 = 10f;
    
    private void Reset()
    {
        if (根物件RectTransform == null)
            根物件RectTransform = GetComponent<RectTransform>();
        if (攝影機 == null)
            攝影機 = Camera.main;
    }
    private void LateUpdate()
    {
        if (根物件RectTransform == null)
            根物件RectTransform = GetComponent<RectTransform>();
        if (根物件RectTransform == null)
        {
            Debug.LogError("沒有RectTransform");
            return;
        }
        if (攝影機 == null || 攝影機.gameObject.activeInHierarchy == false || 攝影機.gameObject.activeSelf == false)
            攝影機 = Camera.main;
        if (攝影機 == null)
        {
            Debug.LogError("沒有Camera，至少要有一個攝影機的Tag被設定為 : MainCamera");
            return;
        }

        Vector3 ab = 攝影機.transform.forward;
        Vector3 ac = 參考位置用的3D物件.position - 攝影機.transform.position;
        float angle = Vector3.Angle(ab, ac);
        if (angle >= 90f)
        {
            根物件RectTransform.localScale = Vector3.zero;
            return;
        }
        Vector3 screenPos = 攝影機.WorldToScreenPoint(參考位置用的3D物件.position);
        if (啟用緩衝跟隨)
        {
            if (緩衝邏輯 == 緩衝跟隨邏輯.Lerp)
                根物件RectTransform.position = Vector3.Lerp(根物件RectTransform.position, screenPos, 緩衝參數 * Time.deltaTime);
            else if (緩衝邏輯 == 緩衝跟隨邏輯.MoveTowards)
                根物件RectTransform.position = Vector3.MoveTowards(根物件RectTransform.position, screenPos, 緩衝參數 * Time.deltaTime);
            else
                根物件RectTransform.position = screenPos;
        }
        else
            根物件RectTransform.position = screenPos;

        根物件RectTransform.localScale = Vector3.one * (一公尺處的尺寸 / Vector3.Distance(參考位置用的3D物件.position, 攝影機.transform.position));
        if (限制最小最大尺寸)
            根物件RectTransform.localScale = Vector3.one * Mathf.Clamp(根物件RectTransform.localScale.x, 最小尺寸, 最大尺寸);
    }
    public enum 緩衝跟隨邏輯
    {
        Lerp, MoveTowards
    }
}
