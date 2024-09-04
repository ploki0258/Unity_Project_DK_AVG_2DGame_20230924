using System.Collections.Generic;
using UnityEngine;

public class DialogueLogWindows : MonoBehaviour
{
	[SerializeField, Header("��ܼҪO")]
	GameObject tempLog = null;
	[SerializeField, Header("��ܬ����I��")]
	RectTransform bgLog = null;

	float timeScaleLog = 0f;

	private void Start()
	{
		��s��ܬ���();
	}

	// �إ�"�U����"�C�� �Ω�Ȧs�n�M������l
	List<GameObject> �U���� = new List<GameObject>();

	void ��s��ܬ���()
	{
		// �M���W���Ȧs����l
		foreach (var gb in �U����)
			Destroy(gb);
		// ���s�}�C
		�U����.Clear();

		// ��l�ҪO���������
		tempLog.SetActive(false);
		// i�p���ܸ�Ƽƶq
		for (int i = 0; i < DialogueManager.instance.dialogueDataList.Count; i++)
		{
			for (int j = 0; j < DialogueManager.instance.dialogueDataList[i].dialogueTotalList.Count; j++)
			{
				bool ��ܹL�F = DialogueManager.instance.�w�g��ܹL�F(DialogueManager.instance.dialogueDataList[i].dialogueTotalList[j].dialogueID,
					out Dialogue dialogue);
				if (��ܹL�F)
				{
					// ��ܥ��b�Τw�g��ܹL����ƪ����e
					// �ƻs�@�ӹ�ܬ����ҪO �é�i��ܬ����I����
					GameObject tempLog = Instantiate(this.tempLog, bgLog);
					tempLog.SetActive(true);
					tempLog.GetComponent<LogGrid>().InputDialogueData(dialogue.dialogueID);
					�U����.Add(tempLog);
				}
				else
				{
					// �ƻs�@�ӹ�ܬ����ҪO �é�i��ܬ����I����
					GameObject tempLog = Instantiate(this.tempLog, bgLog);
					tempLog.transform.localScale = Vector2.zero;
					�U����.Add(tempLog);
				}
			}
		}
	}
}
