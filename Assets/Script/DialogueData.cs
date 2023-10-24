using UnityEngine;

/// <summary>
/// 儲存對話資料：
/// 1.對話者名稱
/// 2.對話內容 - 多段
/// </summary>

[CreateAssetMenu(menuName = "Create_New_DialogueData", fileName = "New_DialogueData")]
public class DialogueData : ScriptableObject
{
	[Header("對話編號")]
	public int dialogueID = 0;
	[Header("對話編號跳轉至"), Tooltip("要跳轉至對話或選項的ID")]
	public int toDialogueID;
	[Header("選項編號")]
	public int optionID;
	[Header("對話種類")]
	public DialogueType dialogueType = DialogueType.對話;
	[Header("對話者名稱")]
	public string dialogueTalkerName = "";
	[Header("對話/選項內容陣列"), TextArea(2, 5)]
	public string[] dialogueContents;
	//[Header("選項個數"), Range(0, 5)]
	//public int optionCount = 0;
	//[Header("選項內容"), TextArea(2, 5)]
	//public string[] optionContents = { "" };
	[Header("對話效果種類")]
	public EffectType effectType = EffectType.無效果;
	[Header("效果對象名稱")]
	public string effectTargetName = "";
	[Header("好感度"), Range(0, 100)]
	public float favorability;
	[Header("體力值"), Range(0, 100)]
	public float strength;
}

public enum DialogueType
{
	對話, 選項, 結束
}

public enum EffectType
{
	無效果, 好感度, 體力值
}
