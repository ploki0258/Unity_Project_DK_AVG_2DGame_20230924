using UnityEngine;

/// <summary>
/// 儲存對話資料：
/// 1.對話編號
/// 2.欲跳轉至的對話或選項的ID
/// 3.對話類別：對話、選項、結束
/// 4.角色位置的顯示狀態
/// 5.對話者名稱
/// 6.對話內容 - 多段
/// 7.對話的效果種類
/// 8.效果對象名稱
/// 9.好感度
/// 10.體力值
/// </summary>
[CreateAssetMenu(menuName = "Create_New_DialogueData", fileName = "New_DialogueData")]
public class DialogueData : ScriptableObject
{
	[Header("對話編號"), Tooltip("對話或選項的ID")]
	public int dialogueID = 0;
	[Header("對話編號跳轉至"), Tooltip("要跳轉到的對話或選項ID")]
	public int toDialogueID;
	[Header("選項編號跳轉至"), Tooltip("要跳轉到的對話或選項ID")]
	public int[] toOptionID;
	[Header("對話種類")]
	public DialogueType dialogueType = DialogueType.對話;
	[Header("角色位置"), Tooltip("角色位置顯示的狀態")]
	public TalkerShow characterPos = TalkerShow.無顯示;
	[Header("對話者名稱")]
	public string dialogueTalkerName = "";
	[Header("對話/選項內容陣列"), TextArea(3, 5)]
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

	//private void Awake()
	//{
	//	toOptionID = new int[dialogueContents.Length];
	//}
}

public enum TalkerShow
{
	左邊, 右邊, 兩人_一起, 兩人_左邊, 兩人_右邊, 無顯示
}

public enum DialogueType
{
	對話, 選項, 結束
}

public enum EffectType
{
	無效果, 好感度, 體力值
}
