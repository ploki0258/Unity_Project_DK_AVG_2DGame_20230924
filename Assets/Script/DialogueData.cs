using System.Collections.Generic;
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
	[Header("對話總表")]
	public List<dialogue> dialogueTotalList = new List<dialogue>();
}

[System.Serializable]
public class dialogue
{
	[Header("對話內容欄位\n\n對話編號"), Tooltip("對話或選項的ID")]
	public int dialogueID;
	[Header("對話/選項編號跳轉至"), Tooltip("要跳轉到的對話或選項的ID")]
	public int[] toDialogueOrOptionID;
	//[Header("選項編號跳轉至"), Tooltip("要跳轉到的對話或選項ID")]
	//public int[] toOptionID;
	[Header("對話種類")]
	public DialogueType dialogueType = DialogueType.對話;
	[Header("角色位置"), Tooltip("角色位置顯示的狀態")]
	public TalkerShow characterPos = TalkerShow.無顯示;
	[Header("對話者名稱")]
	public string dialogueTalkerName = "";
	[Header("對話/選項內容陣列"), TextArea(3, 5)]
	public string[] dialogueContents = new string[0];
	//[Header("選項內容"), TextArea(2, 5)]
	//public string[] optionContents = { "" };
	[Header("對話效果欄位\n\n對話效果種類")]
	public EffectType[] effectType = { EffectType.無效果 };
	[Header("效果對象名稱")]
	public string[] effectTargetName;
	[Header("好感度/體力值"), Range(0, 100), Tooltip("可提升或降低好感度或體力值的數值")]
	public float[] effectValue = { 0 };
	//[Header("體力值"), Range(0, 100), Tooltip("可提升或降低體力值的數值")]
	//public float strength;
}

public enum TalkerShow
{
	無顯示, 左邊, 右邊, 兩人_左邊, 兩人_右邊, 兩人_一起
}

public enum DialogueType
{
	對話, 選項, 重要, 結束
}

public enum EffectType
{
	無效果, 好感度, 體力值
}
