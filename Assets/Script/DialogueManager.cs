using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 管理對話系統的操作：
/// 1.回想功能
/// 2.自動播放
/// 3.對話框隱藏/顯示功能
/// </summary>
public class DialogueManager : MonoBehaviour
{
	[Tooltip("角色名稱對應底圖顏色字典")]
	public Dictionary<string, Color> characterBasemapDic = new Dictionary<string, Color>();

	#region 欄位
	[Header("對話框")]
	public CanvasGroup dialogieUI;
	[Header("對話選項")]
	public CanvasGroup optionUI;
	[Header("對話歷史紀錄")]
	public GameObject dialogieLogUI;
	[SerializeField, Header("自動播放按鈕")]
	private GameObject autoplayButton = null;
	[SerializeField, Header("自動播放按鈕顏色")]
	private Color changeColor;

	[SerializeField, Header("之前對話人名底圖")]
	Image basemapTextTalkerBefore = null;
	[SerializeField, Header("之前對話內容底圖")]
	Image basemapTextContentBefore = null;
	[SerializeField, Header("之前對話人名")]
	TextMeshProUGUI textTalkerBefore = null;
	[SerializeField, Header("之前對話內容")]
	TextMeshProUGUI textContentBefore = null;
	[SerializeField, Header("之前對話紀錄")]
	GameObject dialogueLogPrefab = null;
	[Header("底圖顏色列表")]
	public Color[] basemapImages = new Color[0];

	private CanvasGroup dialogieLogUI_CG = null;
	private Color originalColor;
	[Tooltip("是否自動播放")]
	public bool isAutoplay = false;
	[Tooltip("是否隱藏對話框")]
	public bool isHideDialogue = false;
	[Tooltip("是否隱藏對話紀錄")]
	private bool isHideDialogueLogUI = true;
	#endregion

	public static DialogueManager instance = null;

	private void Awake()
	{
		instance = this;
		dialogieLogUI_CG = GameObject.Find("回想用").GetComponent<CanvasGroup>();

		// 指定字典的對應值
		for (int i = 0; i < DialogueSystem.instance.characteName.Length; i++)
		{
			characterBasemapDic[DialogueSystem.instance.characteName[i]] = basemapImages[i];
		}
	}

	private void Start()
	{
		originalColor = autoplayButton.GetComponent<Image>().color;
		dialogieLogUI_CG.gameObject.SetActive(false);
	}

	private void Update()
	{
		ShowDialogueUI();
#if UNITY_EDITOR
		TestShowDialogueLogUI();
#endif
	}

	void TestShowDialogueLogUI()
	{
		if (Input.GetKeyDown(KeyCode.P) && isHideDialogueLogUI == true)
		{
			isHideDialogueLogUI = false;
			dialogieLogUI_CG.gameObject.SetActive(true);
			dialogieLogUI_CG.alpha = 1f;
			DialogueSystem.instance.enabled = false;
		}
		else if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && isHideDialogueLogUI == false)
		{
			isHideDialogueLogUI = true;
			dialogieLogUI_CG.gameObject.SetActive(false);
			dialogieLogUI_CG.alpha = 0f;
			DialogueSystem.instance.enabled = true;
		}
	}

	/// <summary>
	/// 顯示之前對話記錄介面
	/// </summary>
	void ShowDialogueLogUI()
	{
		if (isHideDialogueLogUI == true)
		{
			DialogueSystem.instance.enabled = false;
			isHideDialogueLogUI = false;
			dialogieLogUI_CG.gameObject.SetActive(true);
			dialogieLogUI_CG.alpha = 1f;
		}/*
		else if (Input.GetKeyDown(KeyCode.Escape) && isHideDialogueLogUI == false)
		{
			isHideDialogueLogUI = true;
			dialogieLogUI_CG.gameObject.SetActive(false);
			dialogieLogUI_CG.alpha = 0f;
			DialogueSystem.instance.enabled = true;
		}*/
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
	public void dialogueLogContent()
	{
		ShowDialogueLogUI();
		for (int i = 0; i < DialogueSystem.instance.dialogueData.Length; i++)
		{
			for (int j = 0; j < DialogueSystem.instance.dialogueData[i].dialogueTotalList.Count; j++)
			{
				for (int k = 0; k < DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueContents.Length; k++)
				{
					if (DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueTalkerName == "")
						return;

					Instantiate(dialogueLogPrefab, dialogieLogUI.transform);

					textTalkerBefore.text = DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueTalkerName;
					textContentBefore.text = DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueContents[k].ToString();
					basemapTextTalkerBefore.color = characterBasemapDic[DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
					//Debug.Log("回想人名：" + DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueTalkerName);
					basemapTextContentBefore.color = characterBasemapDic[DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
				}
			}
		}
	}

	public void CloseButton()
	{
		isHideDialogueLogUI = true;
		dialogieLogUI_CG.gameObject.SetActive(false);
		dialogieLogUI_CG.alpha = 0f;
		DialogueSystem.instance.enabled = true;
	}
}
