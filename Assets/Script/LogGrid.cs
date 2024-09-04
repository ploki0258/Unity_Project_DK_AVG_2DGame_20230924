using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogGrid : MonoBehaviour
{
	[SerializeField, Header("��ܤ��e����")]
	Image contentBG = null;
	[SerializeField, Header("���e��ܤ��e")]
	TextMeshProUGUI textContentBefore = null;
	[SerializeField, Header("��ܤH�W����")]
	Image talkerBG = null;
	[SerializeField, Header("���e��ܤH�W")]
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