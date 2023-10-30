using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 管理對話系統的操作：
/// 1.回想功能
/// 2.自動播放
/// 3.對話框隱藏/顯示功能
/// </summary>
public class DialogueManager : MonoBehaviour
{
	[Header("對話框")]
	public CanvasGroup dialogieUI;
	[Header("對話選項")]
	public CanvasGroup optionUI;
	[Header("對話歷史紀錄")]
	public CanvasGroup dialogieLogUI;
	[SerializeField, Header("自動播放按鈕")]
	private GameObject autoplayButton = null;
	[SerializeField, Header("自動播放按鈕顏色")]
	private Color changeColor;

	private Color originalColor;
	[Tooltip("是否自動播放")]
	public bool isAutoplay = false;
	[Tooltip("是否隱藏對話框")]
	public bool isHideDialogue = false;

	public static DialogueManager instance = null;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		originalColor = autoplayButton.GetComponent<Image>().color;
	}

	private void Update()
	{
		ShowDialogueUI();
	}

	/// <summary>
	/// 隱藏對話框
	/// </summary>
	public void HideDialogueUI()
	{
		if (isHideDialogue == false)
		{
			isHideDialogue = true;
			dialogieUI.alpha = 0;
			Debug.Log("<color=#906>隱藏對話框...</color>");
		}
	}

	/// <summary>
	/// 顯示對話框
	/// </summary>
	public void ShowDialogueUI()
	{
		// 如果 對話框隱藏時
		if (isHideDialogue == true)
		{
			// 如果 按下指定按鍵
			if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
			{
				// 顯示對話畫布 透明度為1
				dialogieUI.alpha = 1;

				// 是否隱藏對話框 = false
				isHideDialogue = false;
			}
		}
	}

	public Action dialogueHideChange;

	/// <summary>
	/// 自動顯示對話
	/// </summary>
	public void AutoPlay()
	{
		for (int i = 0; i < DialogueSystem.instance.dialogueData.Length; i++)
		{
			for (int j = 0; j < DialogueSystem.instance.dialogueData[i].dialogueTotalList.Count; j++)
			{
				if (DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueType == DialogueType.對話 && !isAutoplay)
				{
					isAutoplay = true;
					autoplayButton.GetComponent<Image>().color = changeColor;
					Debug.Log("<color=#690><b>正在自動播放</b></color>");
				}
				else if (DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueType == DialogueType.對話 && isAutoplay)
				{
					isAutoplay = false;
					autoplayButton.GetComponent<Image>().color = originalColor;
					Debug.Log("<color=#690><b>取消自動播放</b></color>");
				}
			}
		}
	}

	/// <summary>
	/// 回想之前對話內容
	/// </summary>
	public void dialoogueLog()
	{

	}
}
