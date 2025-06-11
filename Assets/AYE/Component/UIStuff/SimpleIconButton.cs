using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SimpleIconButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Color normalColor = Color.gray;
    [SerializeField] Color highlightColor = Color.white;
    [SerializeField] UnityEvent onClick = null;
    [SerializeField] UnityEvent onEnter = null;
    private void Reset()
    {
        icon = GetComponent<Image>();
        if (icon != null)
        {
            normalColor = icon.color;
        }
    }
    void Start()
    {
        icon.color = normalColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.color = highlightColor;
        onEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = normalColor;
    }
    float lastClickTime = -1f;
    public void OnPointerClick(PointerEventData eventData)
    {
        icon.color = highlightColor;
        if (Time.time > lastClickTime + 0.5f)
        {
            lastClickTime = Time.time;
            onClick.Invoke();
        }
    }
}
