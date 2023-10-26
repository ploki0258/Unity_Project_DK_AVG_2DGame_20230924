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
		DialogueSystem.instance.currentDialogueID = id;
		DialogueSystem.instance.StartDialogue();
		DialogueSystem.instance.continueIcon.SetActive(false);
		DialogueSystem.instance.optionButton.SetActive(false);
	}
}
