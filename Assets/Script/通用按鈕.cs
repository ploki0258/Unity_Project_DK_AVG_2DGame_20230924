using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class 通用按鈕 : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
	[SerializeField][Header("UI按鈕")] Image button;
	[SerializeField][Header("UI顏色")] Color colorEnter = Color.white, colorExit = Color.white;
	[SerializeField][Header("點擊事件")] UnityEvent clickToDo = null;

	private void Start()
	{
		//button.color = colorExit;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		button.color = colorEnter;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		button.color = colorExit;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		clickToDo.Invoke();
	}
}
