using UnityEngine;

/// <summary>
/// 管理對話系統的操作：
/// 1.回想功能
/// 2.自動播放
/// 3.對話框隱藏/顯示功能
/// </summary>
public class DialogueManager : MonoBehaviour
{
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
		DialogueSystem.instance.isHideDialogue = true;
		DialogueSystem.instance.dialogieUI.alpha = 0;
	}

	/// <summary>
	/// 自動顯示對話
	/// </summary>
	public void AutoPlay()
	{
		DialogueSystem.instance.isAutoplay = true;
		Debug.Log("<color=#906>正在自動播放</color>");
	}

	/// <summary>
	/// 回想之前對話內容
	/// </summary>
	public void dialoogueLog()
	{

	}
}
