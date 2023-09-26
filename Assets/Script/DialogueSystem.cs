﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 對話系統：
/// 1.決定對話者名稱
/// 2.決定對話內容 - 可多段
/// 3.顯示對話完成的動態圖示效果
/// </summary>
public class DialogueSystem : MonoBehaviour
{
	// 將 DialogSystem 設定為單例模式
	public static DialogueSystem instance = null;
	#region 欄位
	[Header("對話資料")]
	public DialogueData dialogueData;
	[Header("對話間隔"), Range(0, 2)]
	public float interval = 0.2f;
	[Header("對話框")]
	public CanvasGroup dialogieUI;
	[Header("文字介面：\n對話人名")]
	public TextMeshProUGUI textTalker;
	[Header("對話內容")]
	public TextMeshProUGUI textContent;
	[Header("對話繼續圖示")]
	public GameObject continueIcon = null;
	#endregion
	private void Awake()
	{
		instance = this;    // 讓單例等於自己
							//talkUI.alpha = 0f;  // 一開始隱藏對話框 α值為0

		StartDialogue();
	}

	/// <summary>
	/// 開始對話
	/// </summary>
	private void StartDialogue()
	{
		StartCoroutine(DisplayEveryDialogue());
	}

	/// <summary>
	/// 顯示每段對話：並在段落之間等待玩家按下繼續按鍵
	/// </summary>
	/// <returns></returns>
	private IEnumerator DisplayEveryDialogue()
	{
		dialogieUI.alpha = 1;
		textTalker.text = dialogueData.dialogueTalkerName;
		textContent.text = "";

		// 第一個迴圈跑總共有幾個對話內容
		// 迴圈初始值不可為重複
		for (int i = 0; i < dialogueData.dialogueContents.Length; i++)
		{
			// 第二個迴圈跑該總對話內容中共有幾個字
			for (int j = 0; j < dialogueData.dialogueContents[i].Length; j++)
			{
				//Debug.Log(dialogueData.dialogueContents[i][j]);
				textContent.text += dialogueData.dialogueContents[i][j];
				yield return new WaitForSeconds(interval);
			}

			continueIcon.SetActive(true);

			if (textContent.text == "")
			{
				textTalker.text = "";
			}

			// 等待玩家按下指定的按鍵 來繼續下段對話
			while (!(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
			{
				yield return null;
			}

			textContent.text = "";
			continueIcon.SetActive(false);

			if (i == dialogueData.dialogueContents.Length - 1) dialogieUI.alpha = 0;
		}
	}
}
