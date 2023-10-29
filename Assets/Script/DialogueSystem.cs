using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 對話系統：
/// 1.決定對話者名稱
/// 2.決定對話內容 - 可多段
/// 3.顯示對話完成的動態圖示效果
/// 4.對話者圖示顯示
/// 5.回想功能
/// 6.自動播放
/// 7.選項選擇與跳轉功能
/// 8.對話框隱藏/顯示功能
/// </summary>
public class DialogueSystem : MonoBehaviour
{
	// 將 DialogSystem 設定為單例模式
	public static DialogueSystem instance = null;

	[Tooltip("對話資料陣列")]
	public DialogueData[] dialogueDataArrary = new DialogueData[0];

	private List<GameObject> garbageCan = new List<GameObject>();

	[Tooltip("角色名稱對應角色圖示字典")]
	public Dictionary<string, Image> characterImagesDic = new Dictionary<string, Image>();

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
	[Header("消失漸變倍數")]
	public float vanishMultiple = 1f;

	[Header("角色名稱陣列")]
	public string[] characteName;
	[Header("角色圖示列表")]
	public List<Image> characterImages = new List<Image>();

	[Tooltip("是否取消打字")]
	private bool cancelTyping = false;
	[Tooltip("是否在進行對話")]
	private bool isTalking = false;
	#endregion

	private void Awake()
	{
		instance = this;    // 讓單例等於自己
							//talkUI.alpha = 0f;  // 一開始隱藏對話框 α值為0
		dialogueDataArrary = Resources.LoadAll<DialogueData>("");
		// 指定字典的對應值
		for (int i = 0; i < characteName.Length; i++)
		{
			characterImagesDic[characteName[i]] = characterImages[i];
		}
	}

	private void Start()
	{
		//talkerShowChange += isTalkerShow;
		DialogueManager.instance.dialogueHideChange += DialogueManager.instance.ShowDialogueUI;
		StartDialogue();
	}

	private void OnDisable()
	{
		//talkerShowChange -= isTalkerShow;
		DialogueManager.instance.dialogueHideChange -= DialogueManager.instance.ShowDialogueUI;
	}

	private void Update()
	{
		Debug.Log($"<color=Green>當前ID：{currentDialogueID}</color>");

		vanishDialogueUI(vanishMultiple);
		DialogueManager.instance.ShowDialogueUI();
		quickShowDialogue();
	}

	/// <summary>
	/// 開始對話
	/// </summary>
	public void StartDialogue()
	{
		StartCoroutine(DisplayEveryDialogue());
	}

	void isTalkerShow()
	{
		//talkerShow = talkerShow;
	}

	/*public TalkerShow talkerShow
	{
		get { return _talkerShow; }
		set
		{
			_talkerShow = value;

			for (int i = 0; i < dialogueData.Length; i++)
			{
				for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
				{
					if (_talkerShow == TalkerShow.兩人_左邊)
					{
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
						}
					}
					else if (_talkerShow == TalkerShow.兩人_右邊)
					{
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
						}
					}
					else if (_talkerShow == TalkerShow.兩人_一起)
					{
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.one;
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (_talkerShow == TalkerShow.左邊)
					{
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.zero;
						dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (_talkerShow == TalkerShow.右邊)
					{
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left.transform.localScale = Vector3.zero;
						dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (_talkerShow == TalkerShow.無顯示)
					{
						dialogueImage_right.transform.localScale = Vector3.zero;
						dialogueImage_left.transform.localScale = Vector3.zero;
					}
				}
			}

			if (talkerShowChange != null)
				talkerShowChange.Invoke();
		}
	}
	TalkerShow _talkerShow = TalkerShow.無顯示;
	public Action talkerShowChange = null;*/

	/// <summary>
	/// 顯示每段對話：並在段落之間等待玩家按下繼續按鍵
	/// </summary>
	/// <returns></returns>
	private IEnumerator DisplayEveryDialogue()
	{
		isTalking = true;
		// 顯示對話畫布 透明度為1
		dialogieUI.alpha = 1;
		// 清空對話內容
		textContent.text = "";
		// 隱藏繼續圖示
		// 隱藏對話選項按鈕
		continueIcon.SetActive(false);
		optionButton.SetActive(false);

		// 第一個迴圈跑 總共有幾個對話資料_i
		for (int i = 0; i < dialogueData.Length; i++)
		{
			// 第二個迴圈跑第i個對話資料中的對話總表 總共有幾個對話數_x
			for (int x = 0; x < dialogueData[i].dialogueTotalList.Count; x++)
			{
				// 如果第i個對話資料.第x個對話數.對話類別為"對話" 且 當前ID 等於 第i個對話資料.第x個對話數.對話編號 的話 才執行
				if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.對話 && currentDialogueID ==
					dialogueData[i].dialogueTotalList[x].dialogueID)
				{
					// 隱藏對話選項按鈕
					optionButton.SetActive(false);
					// 更新對話者名稱
					textTalker.text = dialogueData[i].dialogueTotalList[x].dialogueTalkerName;
					// 更新對話者圖示顯示狀態
					if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.兩人_左邊)
					{
						Debug.Log("這是對話中_左邊");
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
						for (int y = 1; y <= 5; y++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_right.GetComponentsInChildren<Image>()[y].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
						}
					}
					else if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.兩人_右邊)
					{
						Debug.Log("這是對話中_右邊");
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
						for (int y = 1; y <= 5; y++)
						{
							dialogueImage_right.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
						}
					}
					else if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.兩人_一起)
					{
						Debug.Log("這是兩人");
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left.transform.localScale = Vector3.one;
						for (int y = 1; y <= 5; y++)
						{
							dialogueImage_right.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.左邊)
					{
						Debug.Log("這是左邊");
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
						for (int y = 1; y <= 5; y++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
						}
						dialogueImage_right.transform.localScale = Vector3.zero;
					}
					else if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.右邊)
					{
						Debug.Log("這是右邊");
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
						for (int y = 1; y <= 5; y++)
						{
							dialogueImage_right.gameObject.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
						}
						dialogueImage_left.transform.localScale = Vector3.zero;
					}
					else if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.無顯示)
					{
						Debug.Log("這是無顯示");
						dialogueImage_left.transform.localScale = Vector3.zero;
						dialogueImage_right.transform.localScale = Vector3.zero;
					}

					// 第三個迴圈跑第i個對話資料中的對話總表的第x個對話數 總共有幾個對話內容_j
					// 迴圈初始值不可為重複
					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].dialogueContents.Length; j++)
					{
						// 第四個迴圈跑第i個對話資料中的對話總表的第x個對話數的第j個對話內容中 總共有幾個字_k
						// 逐字顯示
						for (int k = 0; k < dialogueData[i].dialogueTotalList[x].dialogueContents[j].Length; k++)
						{
							Debug.Log(dialogueData[i].dialogueTotalList[x].dialogueContents[j][k]);
							// 更新對話內容
							textContent.text += dialogueData[i].dialogueTotalList[x].dialogueContents[j][k];
							// 打字間隔
							yield return new WaitForSeconds(interval);

							//if (cancelTyping)
							//{
							//	textContent.text = dialogueData[i].dialogueTotalList[x].dialogueContents[j];
							//}
						}

						// 每段對話完成後顯示繼續圖示
						continueIcon.SetActive(true);

						// 如果 沒有隱藏對話框的話
						if (DialogueManager.instance.isHideDialogue == false)
						{
							// 如果 正在進行自動播放的話
							if (DialogueManager.instance.isAutoplay == true)
							{
								yield return new WaitForSeconds(1f);
							}

							// 等待玩家按下指定的按鍵 來繼續下段對話
							// 沒按下指定的按鍵 且 正在對話中 且 沒有自動播放時 等待玩家繼續
							while (!(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
								&& isTalking == true && DialogueManager.instance.isAutoplay == false)
							{
								// null 為每一幀的時間
								yield return null;
							}
						}
						else if (DialogueManager.instance.isHideDialogue == true)
						{
							Debug.Log("對話框隱藏中，停止顯示對話");
							yield break;
						}

						// 玩家按下繼續按鈕後 清空對話內容
						textContent.text = "";
						// 隱藏繼續圖示
						continueIcon.SetActive(false);
					}

					// 玩家按下繼續按鈕後 如果對話內容為空 則對話者名稱為空
					//if (textContent.text == "")
					//	textTalker.text = "";
					// 隱藏角色圖示
					//dialogueImage_left.transform.localScale = Vector3.zero;
					//dialogueImage_right.transform.localScale = Vector3.zero;

					// 第五個迴圈跑第i個對話資料中的對話總表的第x個對話數 總共有幾個要跳轉至的對話或選項ID_j
					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID.Length; j++)
					{
						// 當前的對話ID = 第i個對話資料.第x個對話數.要跳轉至的對話或選項ID
						currentDialogueID = dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j];
						Debug.Log("當前ID：" + currentDialogueID);
					}
				}
				// 否則如果第i個對話資料.第x個對話數.對話類別為"選項" 且 當前ID 等於 第i個對話資料.第x個對話數.對話編號 的話 才執行
				else if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.選項 && currentDialogueID ==
						 dialogueData[i].dialogueTotalList[x].dialogueID)
				{
					Debug.Log("<color=orange>這是「選項」</color>");
					// 隱藏繼續圖示
					continueIcon.SetActive(false);
					// 顯示對話選項按鈕
					optionButton.SetActive(true);

					foreach (GameObject t in garbageCan)
					{
						Destroy(t.gameObject);
					}
					garbageCan.Clear();

					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].dialogueContents.Length; j++)
					{
						int tempID = dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j];
						GameObject tempOption = Instantiate(optionButton, dialoguePos);
						tempOption.GetComponentInChildren<TextMeshProUGUI>().text =
							dialogueData[i].dialogueTotalList[x].dialogueContents[j].ToString();

						Debug.Log($"<color=yellow>{dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j]}</color>");
						tempOption.GetComponent<Button>().onClick.AddListener
							(
								delegate
								{
									OptionManager.instance.OnClickToDialogueOrOption(tempID);
								}
							);
						garbageCan.Add(tempOption);
						Debug.Log($"<color=yellow>當前ID：{currentDialogueID}</color>");
					}
				}
				// 否則如果第i個對話資料.第x個對話數.對話類別為"結束" 且 當前ID 等於 第i個對話資料.第x個對話數.對話編號 的話 才執行
				else if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.結束 && currentDialogueID ==
						 dialogueData[i].dialogueTotalList[x].dialogueID)
				{
					// 玩家按下繼續按鈕後 清空對話內容
					textContent.text = "";
					// 隱藏繼續圖示
					continueIcon.SetActive(false);
					optionButton.SetActive(false);
					// 玩家按下繼續按鈕後 如果對話內容為空 則對話者名稱為空
					if (textContent.text == "")
						textTalker.text = "";
					// 隱藏角色圖示
					//dialogueImage_left.transform.localScale = Vector3.zero;
					//dialogueImage_right.transform.localScale = Vector3.zero;
					// 如果對話段落已結束 就關閉對話介面
					//for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
					//{
					//	if (j == dialogueData[i].dialogueTotalList[j].toDialogueOrOptionID.Length) dialogieUI.alpha -= (100 * Time.deltaTime);
					//}
				}
			}
		}
		isTalking = false;
	}

	/// <summary>
	/// 對話UI與角色逐漸消失
	/// </summary>
	/// <param name="_vanishMultiple">消失倍數</param>
	void vanishDialogueUI(float _vanishMultiple)
	{
		_vanishMultiple = vanishMultiple;
		for (int i = 0; i < dialogueData.Length; i++)
		{
			for (int x = 0; x < dialogueData[i].dialogueTotalList.Count; x++)
			{
				if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.結束 && currentDialogueID ==
						 dialogueData[i].dialogueTotalList[x].dialogueID)
				{
					for (int y = 1; y <= 5; y++)
					{
						// 讓角色圖示慢慢消失
						dialogueImage_left.GetComponentsInChildren<Image>()[y].color -= new Color(0f, 0f, 0f, _vanishMultiple * Time.deltaTime);
						dialogueImage_right.GetComponentsInChildren<Image>()[y].color -= new Color(0f, 0f, 0f, _vanishMultiple * Time.deltaTime);
					}

					// 如果對話已結束 就讓對話介面慢慢消失
					for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
					{
						if (j == dialogueData[i].dialogueTotalList[j].toDialogueOrOptionID.Length) dialogieUI.alpha -= (_vanishMultiple * Time.deltaTime);
					}
				}
			}
		}
	}

	/// <summary>
	/// 對話內容快速顯示
	/// </summary>
	void quickShowDialogue()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
		{
			cancelTyping = !cancelTyping;
		}
	}
}
