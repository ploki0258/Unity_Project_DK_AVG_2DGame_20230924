using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideUIByFillAmount : MonoBehaviour
{
    [SerializeField] List<Image> 參考中的Bar條 = new List<Image>();
    [SerializeField] CanvasGroup 應用的CanvasGroup;
    [SerializeField] 大於小於 判定 = 大於小於.小於;
    [SerializeField] [Range(0f, 1f)] float 這個參數要顯示 = 0.99f;
    [SerializeField] float 顯示時出現速度 = 10f;
    [SerializeField] float 隱藏時消失速度 = 5f;
    [SerializeField] 初始化時 初始狀態 = 初始化時.隱藏;
    private void Reset()
    {
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = GetComponent<CanvasGroup>();
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (參考中的Bar條.Count == 0)
        {
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                if (image != null)
                {
                    if (image.type == Image.Type.Filled)
                        參考中的Bar條.Add(image);
                }
            }
        }
    }
    private void Start()
    {
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = GetComponent<CanvasGroup>();
        if (應用的CanvasGroup == null)
            應用的CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        if (初始狀態 == 初始化時.隱藏)
            應用的CanvasGroup.alpha = 0f;
        else if (初始狀態 == 初始化時.顯示)
            應用的CanvasGroup.alpha = 1f;
    }
    private void LateUpdate()
    {
        if (參考中的Bar條.Count == 0)
            return;
        if (應用的CanvasGroup == null)
            return;
        float min = float.MaxValue;
        float max = float.MinValue;
        bool 顯示 = false;
        foreach (Image image in 參考中的Bar條)
        {
            if (image == null)
                continue;
            if (!image.enabled)
                continue;
            if (!image.gameObject.activeInHierarchy)
                continue;
            min = Mathf.Min(min, image.fillAmount);
            max = Mathf.Max(max, image.fillAmount);
        }
        if (判定 == 大於小於.小於)
            顯示 = min < 這個參數要顯示;
        else
            顯示 = max > 這個參數要顯示;
        應用的CanvasGroup.alpha = Mathf.Lerp(應用的CanvasGroup.alpha, 顯示 ? 1f : 0f, Time.deltaTime * (顯示 ? 顯示時出現速度 : 隱藏時消失速度));
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
