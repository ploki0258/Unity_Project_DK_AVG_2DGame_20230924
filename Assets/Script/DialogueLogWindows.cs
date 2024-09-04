using System.Collections.Generic;
using UnityEngine;

public class DialogueLogWindows : MonoBehaviour
{
	[SerializeField, Header("對話模板")]
	GameObject tempLog = null;
	[SerializeField, Header("對話紀錄背景")]
	RectTransform bgLog = null;

	float timeScaleLog = 0f;

	private void Start()
	{
		刷新對話紀錄();
	}

	// 建立"垃圾桶"列表 用於暫存要清除的格子
	List<GameObject> 垃圾桶 = new List<GameObject>();

	void 刷新對話紀錄()
	{
		// 清除上次暫存的格子
		foreach (var gb in 垃圾桶)
			Destroy(gb);
		// 重製陣列
		垃圾桶.Clear();

		// 格子模板本身不顯示
		tempLog.SetActive(false);
		// i小於對話資料數量
		for (int i = 0; i < DialogueManager.instance.dialogueDataList.Count; i++)
		{
			for (int j = 0; j < DialogueManager.instance.dialogueDataList[i].dialogueTotalList.Count; j++)
			{
				bool 對話過了 = DialogueManager.instance.已經對話過了(DialogueManager.instance.dialogueDataList[i].dialogueTotalList[j].dialogueID,
					out Dialogue dialogue);
				if (對話過了)
				{
					// 顯示正在或已經對話過的資料的內容
					// 複製一個對話紀錄模板 並放進對話紀錄背景中
					GameObject tempLog = Instantiate(this.tempLog, bgLog);
					tempLog.SetActive(true);
					tempLog.GetComponent<LogGrid>().InputDialogueData(dialogue.dialogueID);
					垃圾桶.Add(tempLog);
				}
				else
				{
					// 複製一個對話紀錄模板 並放進對話紀錄背景中
					GameObject tempLog = Instantiate(this.tempLog, bgLog);
					tempLog.transform.localScale = Vector2.zero;
					垃圾桶.Add(tempLog);
				}
			}
		}
	}
}
