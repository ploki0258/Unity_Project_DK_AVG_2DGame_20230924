using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogGrid : MonoBehaviour
{
	[SerializeField, Header("對話內容底圖")]
	Image contentBG = null;
	[SerializeField, Header("之前對話內容")]
	TextMeshProUGUI textContentBefore = null;
	[SerializeField, Header("對話人名底圖")]
	Image talkerBG = null;
	[SerializeField, Header("之前對話人名")]
	TextMeshProUGUI textTalkerBefore = null;

	Dialogue dataLog;

	public void InputDialogueData(int id)
	{
		dataLog = DialogueManager.instance.FindDialogueById(id);

		for (int i = 0; i < DialogueSystem.instance.dialogueData.Length; i++)
		{
			textTalkerBefore.text = dataLog.dialogueContents[i];
			textContentBefore.text = dataLog.talkerName;
			if (DialogueManager.instance.characterBasemapDic[dataLog.talkerName] != null)
			{
				talkerBG.color = DialogueManager.instance.characterBasemapDic[dataLog.talkerName];
				contentBG.color = DialogueManager.instance.characterBasemapDic[dataLog.talkerName];
			}
		}
	}
}