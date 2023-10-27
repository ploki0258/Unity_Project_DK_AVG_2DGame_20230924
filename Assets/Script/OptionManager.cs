using UnityEngine;

public class OptionManager : MonoBehaviour
{
	public static OptionManager instance = null;

	private void Awake()
	{
		instance = this;
	}

	public void OnClickToDialogueOrOption(int id)
	{
		StopAllCoroutines();
		DialogueSystem.instance.currentDialogueID = id;
		DialogueSystem.instance.StartDialogue();
		// 玩家按下繼續按鈕後 如果對話內容為空 則對話者名稱為空
		if (DialogueSystem.instance.textContent.text == "")
			DialogueSystem.instance.textTalker.text = "";
		// 隱藏角色圖示
		DialogueSystem.instance.dialogueImage_left.transform.localScale = Vector3.zero;
		DialogueSystem.instance.dialogueImage_right.transform.localScale = Vector3.zero;
		DialogueSystem.instance.continueIcon.SetActive(false);
		DialogueSystem.instance.optionButton.SetActive(false);
	}
}
