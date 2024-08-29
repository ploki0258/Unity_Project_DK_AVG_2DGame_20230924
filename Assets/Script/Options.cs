using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI text = null;
	[SerializeField] GameObject options = null;
	[SerializeField] RectTransform option = null;
	[SerializeField] float minWidth;
	[SerializeField] float optionSpace;

	public void OnPointerClick(PointerEventData eventData)
	{
		
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		options.gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
		text.fontStyle = FontStyles.Bold;
		float width = option.sizeDelta.x;
		if (width < minWidth)
		{
			width = minWidth + optionSpace;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		options.gameObject.transform.localScale = Vector3.one;
		text.fontStyle = FontStyles.Normal;
	}
}
