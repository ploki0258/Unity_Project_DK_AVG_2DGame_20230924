using UnityEngine;

public class OptionManager : MonoBehaviour
{
	public static OptionManager instance = null;

	private void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// 點擊開始選項對話
	/// </summary>
	/// <param name="id">對話/選項編號</param>
	public void OnClickToDialogueOrOption(int id)
	{
		StopAllCoroutines();
		DialogueSystem.instance.currentDialogueID = id;	// 指定對話/選項編號
		DialogueSystem.instance.StartDialogue();		// 開始對話
	}
}
