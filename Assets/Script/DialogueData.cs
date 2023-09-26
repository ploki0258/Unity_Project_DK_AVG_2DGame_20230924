using UnityEngine;

/// <summary>
/// 儲存對話資料：
/// 1.對話者名稱
/// 2.對話內容 - 多段
/// </summary>

[CreateAssetMenu(menuName = "Create_New_DialogueData", fileName = "New_DialogueData")]
public class DialogueData : ScriptableObject
{
	[Header("對話者名稱")]
	public string dialogueTalkerName = "";
	[Header("對話內容陣列"), TextArea(2, 5)]
	public string[] dialogueContents;
}
