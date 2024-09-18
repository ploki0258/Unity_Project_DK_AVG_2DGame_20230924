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
						Debug.Log("找不到人名");
					}
				}
			}
		}
	}
}