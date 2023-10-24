using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 對話系統：
/// 1.決定對話者名稱
/// 2.決定對話內容 - 可多段
/// 3.顯示對話完成的動態圖示效果
/// </summary>
public class DialogueSystem : MonoBehaviour
{
	// 將 DialogSystem 設定為單例模式
	public static DialogueSystem instance = null;

	[Tooltip("對話資料陣列")]
	public DialogueData[] dialogueDataArrary = new DialogueData[0];

	#region 欄位
	[Header("對話資料")]
	public DialogueData[] dialogueData;
	[Header("當前對話編號"), Tooltip("當前對話編號")]
	public int currentDialogueID = 0;
	[Header("對話間隔"), Range(0, 2)]
	public float interval = 0.2f;
	[Header("對話框")]
	public CanvasGroup dialogieUI;
	[Header("文字介面：\n對話人名")]
	public TextMeshProUGUI textTalker;
	[Header("對話內容")]
	public TextMeshProUGUI textContent;
	[Header("對話繼續圖示")]
	public GameObject continueIcon = null;
	[Header("對話人物圖示_左")]
	public Image dialogueImage_left;
	[Header("對話人物圖示_右")]
	public Image dialogueImage_right;
	[Header("對話選項按鈕")]
	public GameObject optionButton = null;
	[Header("對話選項座標")]
	public RectTransform dialoguePos = null;

	[Tooltip("是否自動播放")]
	private bool isAutoplay = false;
	[Tooltip("是否隱藏對話框")]
	private bool isHideDialogue = false;
	#endregion

	private void Awake()
	{
		instance = this;    // 讓單例等於自己
							//talkUI.alpha = 0f;  // 一開始隱藏對話框 α值為0
		dialogueDataArrary = Resources.LoadAll<DialogueData>("");
	}

	private void Start()
	{
		StartDialogue();
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	StartDialogue();
		//}

		// 如果 對話框隱藏時
		if (isHideDialogue == true)
		{
			// 如果 按下滑鼠左鍵
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				// 顯示對話畫布 透明度為1
				dialogieUI.alpha = 1;

				// 是否隱藏對話框 = false
				isHideDialogue = false;
			}
		}
	}

	/// <summary>
	/// 開始對話
	/// </summary>
	private void StartDialogue()
	{
		StartCoroutine(DisplayEveryDialogue());
	}

	/// <summary>
	/// 顯示每段對話：並在段落之間等待玩家按下繼續按鍵
	/// </summary>
	/// <returns></returns>
	private IEnumerator DisplayEveryDialogue()
	{
		// 顯示對話畫布 透明度為1
		dialogieUI.alpha = 1;
		// 清空對話內容
		textContent.text = "";
		// 隱藏對話選項按鈕
		optionButton.SetActive(false);

		// 第一個迴圈跑總共有幾個對話資料
		for (int i = 0; i < dialogueData.Length; i++)
		{
			// 如果第i個對話資料.對話類別為"對話" 且 當前ID 等於 第i個對話資料.對話編號 的話 才執行
			if (dialogueData[i].dialogueType == DialogueType.對話 && currentDialogueID == dialogueData[i].dialogueID)
			{
				optionButton.SetActive(false);

				// 更新對話者名稱
				textTalker.text = dialogueData[i].dialogueTalkerName;

				// 第二個迴圈跑第i個對話資料總共有幾個對話內容
				// 迴圈初始值不可為重複
				for (int j = 0; j < dialogueData[i].dialogueContents.Length; j++)
				{
					// 第三個迴圈跑第i個對話資料的第j個對話內容中總共有幾個字
					for (int k = 0; k < dialogueData[i].dialogueContents[j].Length; k++)
					{
						Debug.Log(dialogueData[i].dialogueContents[j][k]);
						// 更新對話內容
						textContent.text += dialogueData[i].dialogueContents[j][k];
						// 打字間隔
						yield return new WaitForSeconds(interval);
					}

					// 每段對話完成後顯示繼續圖示
					continueIcon.SetActive(true);

					// 如果 沒有隱藏對話框的話
					if (isHideDialogue == false)
					{
						// 如果 沒有自動播放的話
						if (isAutoplay == false)
						{
							// 等待玩家按下指定的按鍵 來繼續下段對話
							while (!(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)))
							{
								// null 為每一幀的時間
								yield return null;
							}
						}
						else if (isAutoplay)
						{
							continue;
						}
					}
					else if (isAutoplay == true)
					{
						Debug.Log("自動播放中");
						//yield return new WaitForSeconds(0.3f);
					}

										// 玩家按下繼續按鈕後 清空對話內容
					textContent.text = "";
					// 隱藏繼續圖示
					continueIcon.SetActive(false);
					// 如果對話段落已結束 就關閉對話介面
					//if (i == dialogueData.dialogueContents.Length - 1) dialogieUI.alpha = 0;
				}
				// 玩家按下繼續按鈕後 如果對話內容為空 則對話者名稱為空
				if (textContent.text == "")
				{
					textTalker.text = "";
				}

				currentDialogueID = dialogueData[i].toDialogueID;
			}
			else if (dialogueData[i].dialogueType == DialogueType.選項 && currentDialogueID == dialogueData[i].toDialogueID)
			{
				Debug.Log("<color=orange>這是「選項」</color>");
				//dialogueData[i].dialogueID = dialogueData[i].toDialogueID;
				//Debug.Log($"<color=orange>選項ID：{dialogueData[i].dialogueID}</color>");
				currentDialogueID = dialogueData[i].toDialogueID;
			}
		}
	}

	public void OnClick()
	{

	}

	/// <summary>
	/// 隱藏對話框
	/// </summary>
	public void HideDialogue()
	{
		isHideDialogue = true;
		dialogieUI.alpha = 0;
	}

	/// <summary>
	/// 自動顯示對話
	/// </summary>
	public void AutoPlay()
	{
		isAutoplay = true;
		Debug.Log("<color=#906>正在自動播放</color>");
	}
}
