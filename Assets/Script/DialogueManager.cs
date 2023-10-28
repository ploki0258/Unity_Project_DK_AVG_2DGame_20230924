using UnityEngine;
using System;

/// <summary>
/// 管理對話系統的操作：
/// 1.回想功能
/// 2.自動播放
/// 3.對話框隱藏/顯示功能
/// </summary>
public class DialogueManager : MonoBehaviour
{
	[Tooltip("是否自動播放")]
	public bool isAutoplay = false;
	[Tooltip("是否隱藏對話框")]
	public bool isHideDialogue = false;

	public static DialogueManager instance = null;

	private void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// 隱藏對話框
	/// </summary>
	public void HideDialogue()
	{
		isHideDialogue = true;
		DialogueSystem.instance.dialogieUI.alpha = 0;
		Debug.Log("<color=#906>隱藏對話框...</color>");
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
				if (DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueType == DialogueType.對話)
				{
					isAutoplay = true;
					Debug.Log("<color=#906>正在自動播放</color>");
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
