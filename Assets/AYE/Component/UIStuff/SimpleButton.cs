using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SimpleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] UnityEvent onClick = null;
    [SerializeField] UnityEvent onEnter = null;
    [SerializeField] UnityEvent onExit = null;
    [SerializeField] bool autoExitOnStart = true;
    void Start()
    {
        if (autoExitOnStart)
            onExit.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onExit.Invoke();
    }
    float lastClickTime = -1f;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.unscaledTime > lastClickTime + 0.2f)
        {
            lastClickTime = Time.unscaledTime;
            onClick.Invoke();
        }
    }
}
