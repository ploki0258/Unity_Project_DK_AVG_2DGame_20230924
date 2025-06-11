using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HideUIByDistance : MonoBehaviour
{
    [SerializeField] Transform 參考位置用的3D物件;
    [SerializeField] CanvasGroup 應用的CanvasGroup;
    [SerializeField] Camera 應用的Camera;
    [SerializeField] 大於小於 判定 = 大於小於.小於;
    [SerializeField] float 這個參數要顯示 = 5f;
    [SerializeField] float 顯示時出現速度 = 5f;
    [SerializeField] float 隱藏時消失速度 = 5f;
    [SerializeField] 初始化時 初始狀態 = 初始化時.隱藏;
    [SerializeField] bool 視線檢定 = false;
    [SerializeField] LayerMask 阻擋視線的圖層 = 0;
    private void Reset()
    {
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = GetComponent<CanvasGroup>();
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (應用的Camera == null)
            應用的Camera = Camera.main;
        阻擋視線的圖層 = LayerMask.GetMask("Default");
    }
    void Start()
    {
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = GetComponent<CanvasGroup>();
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (應用的Camera == null)
            應用的Camera = Camera.main;

        if (初始狀態 == 初始化時.隱藏)
            應用的CanvasGroup.alpha = 0;
        else if (初始狀態 == 初始化時.顯示)
            應用的CanvasGroup.alpha = 1;
    }
    private void LateUpdate()
    {
        if (參考位置用的3D物件 == null)
        {
            Debug.LogError("需要設定參考位置用的3D物件，才有辦法計算距離。");
            return;
        }

        if (應用的Camera == null || 應用的Camera.gameObject.activeInHierarchy == false || 應用的Camera.gameObject.activeSelf == false)
            應用的Camera = Camera.main;
        if (應用的Camera == null)
        {
            Debug.LogError("缺少攝影機，至少要有一個攝影機的Tag被設定為 : MainCamera");
            return;
        }
        float dis = Vector3.Distance(應用的Camera.gameObject.transform.position, 參考位置用的3D物件.position);

        bool 視線被阻擋 = false;
        if (視線檢定)
        {
            RaycastHit hit;
            視線被阻擋 = Physics.Raycast(應用的Camera.gameObject.transform.position, 參考位置用的3D物件.position - 應用的Camera.gameObject.transform.position, out hit, dis, 阻擋視線的圖層);
        }
        if (視線被阻擋)
        {
            應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 0, 隱藏時消失速度 * Time.deltaTime);
            return;
        }
        if (判定 == 大於小於.小於)
        {
            if (dis < 這個參數要顯示)
                應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 1, 顯示時出現速度 * Time.deltaTime);
            else
                應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 0, 隱藏時消失速度 * Time.deltaTime);
        }
        else if (判定 == 大於小於.大於)
        {
            if (dis > 這個參數要顯示)
                應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 1, 顯示時出現速度 * Time.deltaTime);
            else
                應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 0, 隱藏時消失速度 * Time.deltaTime);
        }
    }
    public enum 大於小於
    {
        小於, 大於
    }
    public enum 初始化時
    {
        隱藏, 顯示, 不特別處理
    }
}
