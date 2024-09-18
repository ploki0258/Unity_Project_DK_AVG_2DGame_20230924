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
	
	DialogueData dataLog;

	public void InputDialogueData(int id)
	{
		dataLog = DialogueManager.instance.FindDialogueById(id);

		for (int i = 0; i < DialogueSystem.instance.dialogueData.Length; i++)
		{
			for (int j = 0; j < DialogueSystem.instance.dialogueData[i].dialogueTotalList.Count; j++)
			{
				textTalkerBefore.text = dataLog.dialogueTotalList[j].talkerName;
				for (int x = 0; x < DialogueSystem.instance.dialogueData[i].dialogueTotalList[j].dialogueContents.Length; x++)
				{
					textContentBefore.text = dataLog.dialogueTotalList[j].dialogueContents[x];
				}

				foreach (string name in DialogueManager.instance.characteName)
				{
					if (DialogueManager.instance.characterBasemapDic.ContainsKey(name))
					{
						Debug.Log(name);
						//talkerBG.color = DialogueManager.instance.characterBasemapDic[dataLog.talkerName];
						//contentBG.color = DialogueManager.instance.characterBasemapDic[dataLog.talkerName];
					}
					else
					{
						Debug.Log("�䤣��H�W");
					}
				}
			}
		}
	}
}