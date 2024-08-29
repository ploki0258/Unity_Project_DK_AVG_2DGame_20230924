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
	[Tooltip("垃圾桶")]
	private List<GameObject> garbageCan = new List<GameObject>();
	[Tooltip("角色名稱對應角色圖示字典")]
	public Dictionary<string, Image> characterImagesDic = new Dictionary<string, Image>();

	#region 欄位
	[Header("對話資料")]
	public DialogueData[] dialogueData;
	[Header("當前對話編號"), Tooltip("當前對話編號")]
	public int currentDialogueID = 0;
	[Header("對話間隔"), Range(0, 2)]
	[SerializeField] float interval = 0.1f;
	[Header("自動對話間隔"), Range(0, 2)]
	[SerializeField] float intervalAuto = 0.1f;
	[Header("文字介面：\n對話人名")]
	[SerializeField] TextMeshProUGUI textTalker;
	[Header("對話內容")]
	[SerializeField] TextMeshProUGUI textContent;
	[Header("對話繼續圖示")]
	[SerializeField] GameObject continueIcon = null;
	[Header("對話人物圖示_左")]
	[SerializeField] Image dialogueImage_left;
	[Header("對話人物圖示_右")]
	[SerializeField] Image dialogueImage_right;
	[Header("對話選項按鈕")]
	[SerializeField] GameObject optionButton = null;
	[Header("對話選項座標")]
	[SerializeField] RectTransform dialoguePos = null;
	[Header("消失漸變倍數")]
	[SerializeField] float vanishMultiple = 1f;
	[Header("繼續按鍵")]
	public KeyCode[] continueBtns = { 0 };
	[SerializeField] Animator ani;

	[Header("角色名稱陣列")]
	public string[] characteName;
	[Header("角色圖示列表")]
	public List<Image> characterImages = new List<Image>();

	[Tooltip("是否取消打字")]
	private bool cancelTyping = false;
	[Tooltip("是否可以取消打字")]
	private bool canCancel = false;
	[Tooltip("是否在對話中")]
	private bool isTalking = false;
	[Tooltip("對話是否在繼續")]
	private bool isContinueing = false;
	[Tooltip("是否停止")]
	private bool isStop = false;
	#endregion

	private void Awake()
	{
		instance = this;    // 讓單例等於自己
		dialogueDataArrary = Resources.LoadAll<DialogueData>("");
		// 指定字典的對應值
		for (int i = 0; i < characteName.Length; i++)
		{
			characterImagesDic[characteName[i]] = characterImages[i];
		}
	}

	private void Start()
	{
		DialogueManager.instance.dialogueHideChange += DialogueManager.instance.ShowDialogueUI;
		StartDialogue();
	}

	private void OnDisable()
	{
		DialogueManager.instance.dialogueHideChange -= DialogueManager.instance.ShowDialogueUI;
	}

	private void Update()
	{
		//Debug.Log($"<color=Green>當前ID：{currentDialogueID}</color>");

		vanishDialogueUI(vanishMultiple);
		ContinueDialogue();
	}

	/// <summary>
	/// 開始對話
	/// </summary>
	public void StartDialogue()
	{
		StartCoroutine(DisplayEveryDialogue());
	}

	/// <summary>
	/// 對話人物顯示
	/// </summary>
	public TalkerShow TalkerShow
	{
		get { return _talkerShow; }
		set
		{
			_talkerShow = value;

			for (int i = 0; i < dialogueData.Length; i++)
			{
				for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
				{
					switch (value)
					{
						case TalkerShow.無顯示:
							dialogueImage_right.transform.localScale = Vector3.zero;
							dialogueImage_left.transform.localScale = Vector3.zero;
							break;
						case TalkerShow.左邊:
							TalkerDisplays(value);
							break;
						case TalkerShow.右邊:
							TalkerDisplays(value);
							break;
						case TalkerShow.兩人_左邊:
							TalkerDisplays(value);
							break;
						case TalkerShow.兩人_右邊:
							TalkerDisplays(value);
							break;
						case TalkerShow.兩人_一起:
							dialogueImage_right.transform.localScale = Vector3.one;
							dialogueImage_left.transform.localScale = Vector3.one;
							break;
					}

					// if寫法
					/*if (value == TalkerShow.兩人_左邊)
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
					else if (value == TalkerShow.兩人_右邊)
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
					else if (value == TalkerShow.兩人_一起)
					{
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.one;
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (value == TalkerShow.左邊)
					{
						dialogueImage_left.transform.localScale = Vector3.one;
						dialogueImage_right.transform.localScale = Vector3.zero;
						dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_left.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (value == TalkerShow.右邊)
					{
						dialogueImage_right.transform.localScale = Vector3.one;
						dialogueImage_left.transform.localScale = Vector3.zero;
						dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
						for (int x = 1; x <= 5; x++)
						{
							dialogueImage_right.GetComponentsInChildren<Image>()[x].color = new Color(1f, 1f, 1f, 1f);
						}
					}
					else if (value == TalkerShow.無顯示)
					{
						dialogueImage_right.transform.localScale = Vector3.zero;
						dialogueImage_left.transform.localScale = Vector3.zero;
					}
					*/
				}
			}
		}
	}
	[SerializeField] TalkerShow _talkerShow = TalkerShow.無顯示;

	void TalkerDisplays(TalkerShow talkerShow)
	{
		// 顯示左右邊圖示
		dialogueImage_left.transform.localScale = talkerShow == TalkerShow.左邊 ? Vector3.one : Vector3.zero;
		dialogueImage_right.transform.localScale = talkerShow == TalkerShow.右邊 ? Vector3.one : Vector3.zero;
		//dialogueImage_left.transform.localScale = talkerShow == TalkerShow.兩人_左邊 ? Vector3.one : Vector3.one;
		//dialogueImage_right.transform.localScale = talkerShow == TalkerShow.兩人_右邊 ? Vector3.one : Vector3.one;

		// 依據對話人名取得相應的角色圖示
		for (int i = 0; i < dialogueData.Length; i++)
		{
			for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
			{
				if (talkerShow == TalkerShow.左邊 || talkerShow == TalkerShow.兩人_左邊)
				{
					Debug.Log(dialogueData[i].dialogueTotalList[j].dialogueTalkerName);
					dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
					// 變更角色圖示的透明度
					for (int x = 1; x <= 5; x++)
					{
						dialogueImage_left.GetComponentsInChildren<Image>()[x].color = talkerShow == TalkerShow.左邊 ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 0f, 0f, 0f);
						dialogueImage_left.GetComponentsInChildren<Image>()[x].color = talkerShow == TalkerShow.兩人_左邊 ? new Color(1f, 1f, 1f, 1f) : new Color(0.7f, 0.7f, 0.7f, 0.7f);
					}
				}
				else if (talkerShow == TalkerShow.右邊 || talkerShow == TalkerShow.兩人_右邊)
				{
					Debug.Log(dialogueData[i].dialogueTotalList[j].dialogueTalkerName);
					dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[j].dialogueTalkerName];
					// 變更角色圖示的透明度
					for (int x = 1; x <= 5; x++)
					{
						dialogueImage_right.GetComponentsInChildren<Image>()[x].color = talkerShow == TalkerShow.右邊 ? new Color(1f, 1f, 1f, 1f) : new Color(0f, 0f, 0f, 0f);
						dialogueImage_right.GetComponentsInChildren<Image>()[x].color = talkerShow == TalkerShow.兩人_右邊 ? new Color(1f, 1f, 1f, 1f) : new Color(0.7f, 0.7f, 0.7f, 0.7f);
					}
				}
			}
		}
	}

	/// <summary>
	/// 顯示每段對話：並在段落之間等待玩家按下繼續按鍵
	/// </summary>
	/// <returns></returns>
	private IEnumerator DisplayEveryDialogue()
	{
		isTalking = true;
		// 顯示對話畫布 透明度為1
		float alpha = DialogueManager.instance.dialogieUI.alpha;
		DialogueManager.instance.dialogieUI.alpha = Mathf.Lerp(0f, 1f, Time.unscaledDeltaTime * 100f);
		// 清空對話內容
		textContent.text = "";

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
					// 隱藏繼續圖示
					continueIcon.SetActive(false);
					// 隱藏對話選項按鈕
					optionButton.SetActive(false);
					DialogueManager.instance.optionUI.alpha = 0f;
					// 更新對話者名稱
					textTalker.text = dialogueData[i].dialogueTotalList[x].dialogueTalkerName;
					// 更新對話者圖示顯示狀態
					switch (dialogueData[i].dialogueTotalList[x].characterPos)
					{
						case TalkerShow.無顯示:
							dialogueImage_right.transform.localScale = Vector3.zero;
							dialogueImage_left.transform.localScale = Vector3.zero;
							break;
						case TalkerShow.左邊:
							dialogueImage_left.transform.localScale = Vector3.one;
							dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
							for (int y = 1; y <= 5; y++)
							{
								dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
							}
							dialogueImage_right.transform.localScale = Vector3.zero;
							break;
						case TalkerShow.右邊:
							dialogueImage_right.transform.localScale = Vector3.one;
							dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
							for (int y = 1; y <= 5; y++)
							{
								dialogueImage_right.gameObject.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
							}
							dialogueImage_left.transform.localScale = Vector3.zero;
							break;
						case TalkerShow.兩人_左邊:
							dialogueImage_left.transform.localScale = Vector3.one;
							dialogueImage_right.transform.localScale = Vector3.one;
							dialogueImage_left = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
							for (int y = 1; y <= 5; y++)
							{
								dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
								dialogueImage_right.GetComponentsInChildren<Image>()[y].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
							}
							break;
						case TalkerShow.兩人_右邊:
							dialogueImage_right.transform.localScale = Vector3.one;
							dialogueImage_left.transform.localScale = Vector3.one;
							dialogueImage_right = characterImagesDic[dialogueData[i].dialogueTotalList[x].dialogueTalkerName];
							for (int y = 1; y <= 5; y++)
							{
								dialogueImage_right.GetComponentsInChildren<Image>()[y].color = new Color(1f, 1f, 1f, 1f);
								dialogueImage_left.GetComponentsInChildren<Image>()[y].color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
							}
							break;
						case TalkerShow.兩人_一起:
							dialogueImage_right.transform.localScale = Vector3.one;
							dialogueImage_left.transform.localScale = Vector3.one;
							break;
					}
					/*if (dialogueData[i].dialogueTotalList[x].characterPos == TalkerShow.兩人_左邊)
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
					*/

					// 第三個迴圈跑第i個對話資料中的對話總表的第x個對話數 總共有幾個對話內容_j
					// 迴圈初始值不可為重複
					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].dialogueContents.Length; j++)
					{
						canCancel = true;
						cancelTyping = false;

						// 第四個迴圈跑第i個對話資料中的對話總表的第x個對話數的第j個對話內容中 總共有幾個字_k
						// 逐字顯示
						for (int k = 0; k < dialogueData[i].dialogueTotalList[x].dialogueContents[j].Length; k++)
						{
							//Debug.Log(dialogueData[i].dialogueTotalList[x].dialogueContents[j][k]); // 顯示字
							// 更新對話內容
							textContent.text += dialogueData[i].dialogueTotalList[x].dialogueContents[j][k];
							// 如果取消打字為false 就間隔一段時間才打字 
							if (!cancelTyping)
							{
								yield return new WaitForSeconds(interval);
							}
						}

						// 每段對話完成後顯示繼續圖示
						continueIcon.SetActive(true);

						// 如果 沒有隱藏對話框的話 才進行顯示對話內容
						if (DialogueManager.instance.isHideDialogue == false)
						{
							// 如果 正在進行自動播放的話
							if (DialogueManager.instance.isAutoplay == true)
							{
								yield return new WaitForSeconds(intervalAuto);
							}

							isStop = true;
							canCancel = false;
							// 等待玩家按下指定的按鍵 來繼續下段對話
							// 沒按下指定的按鍵 且 正在對話中 且 沒有自動播放時 等待玩家按下繼續鍵
							while (!isContinueing && isTalking && !DialogueManager.instance.isAutoplay)
							{
								// null 為每一幀的時間
								yield return null;
							}
							isStop = false;
							isContinueing = false;
						}
						// 否則就隱藏
						else
						{
							Debug.Log("對話框隱藏中，停止顯示對話");
							yield break;
						}

						// 玩家按下繼續按鈕後 清空對話內容
						textContent.text = "";
						// 隱藏繼續圖示
						continueIcon.SetActive(false);
					}

					// 第五個迴圈跑第i個對話資料中的對話總表的第x個對話數 總共有幾個要跳轉至的對話或選項ID_j
					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID.Length; j++)
					{
						// 當前的對話ID = 第i個對話資料.第x個對話數.要跳轉至的對話或選項ID
						currentDialogueID = dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j];
						Debug.Log("當前ID：" + currentDialogueID);
					}
				}

				// 如果第i個對話資料.第x個對話數.對話類別為"選項" 且 當前ID 等於 第i個對話資料.第x個對話數.對話編號 的話 才執行
				if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.選項 && currentDialogueID ==
						 dialogueData[i].dialogueTotalList[x].dialogueID)
				{
					//Debug.Log("<color=orange>這是「選項」</color>");
					// 隱藏繼續圖示
					continueIcon.SetActive(false);
					// 顯示對話選項按鈕
					optionButton.SetActive(true);
					DialogueManager.instance.optionUI.alpha = 1f;

					ani.SetTrigger("openOption");
					// 刪除前次生成的選項
					foreach (GameObject t in garbageCan)
					{
						Destroy(t.gameObject);
					}
					garbageCan.Clear();

					// 第i個對話資料.第x個對話數.對話/選項內容的個數
					for (int j = 0; j < dialogueData[i].dialogueTotalList[x].dialogueContents.Length; j++)
					{
						// 欲跳轉的編號個數需與選項內容的個數一致
						if (dialogueData[i].dialogueTotalList[x].dialogueContents.Length != dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID.Length)
						{
							Debug.Log("欲跳轉的編號個數需與選項內容的個數一致");
							break;
						}
						// 指定欲跳轉的選項編號
						int tempID = dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j];
						// 生成對話選項
						GameObject tempOption = Instantiate(optionButton, dialoguePos);
						// 更新選項內容
						tempOption.GetComponentInChildren<TextMeshProUGUI>().text =
							dialogueData[i].dialogueTotalList[x].dialogueContents[j].ToString();

						Debug.Log($"<color=orange>欲跳轉的對話/選項編號：{dialogueData[i].dialogueTotalList[x].toDialogueOrOptionID[j]}</color>");
						tempOption.GetComponent<Button>().onClick.AddListener
							(
								delegate
								{
									OptionManager.instance.OnClickToDialogueOrOption(tempID);
								}
							);
						garbageCan.Add(tempOption);
					}
					Debug.Log($"<color=orange>當前ID：{currentDialogueID}</color>");
				}
			}
		}

		isTalking = false;
	}

	/// <summary>
	/// 繼續對話功能
	/// </summary>
	void ContinueDialogue()
	{
		foreach (KeyCode btns in continueBtns)
		{
			// 如果按下繼續鍵的話
			if (Input.GetKeyDown(btns))
			{
				// 如果是否繼續為false 且 正在對話中
				// 是否繼續變為true
				if (!isContinueing && isTalking && isStop)
				{
					isContinueing = true;
				}
				// 還在進行對話中
				else if (canCancel == true && cancelTyping == false)
				{
					cancelTyping = true;
				}
			}
		}
	}

	/// <summary>
	/// 對話UI與角色逐漸消失
	/// </summary>
	/// <param name="_vanishMultiple">消失倍數</param>
	void vanishDialogueUI(float _vanishMultiple)
	{
		optionButton.SetActive(false);

		_vanishMultiple = vanishMultiple;
		for (int i = 0; i < dialogueData.Length; i++)
		{
			for (int x = 0; x < dialogueData[i].dialogueTotalList.Count; x++)
			{
				if (dialogueData[i].dialogueTotalList[x].dialogueType == DialogueType.結束 && currentDialogueID ==
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

					for (int y = 1; y <= 5; y++)
					{
						// 讓角色圖示慢慢消失
						dialogueImage_left.GetComponentsInChildren<Image>()[y].color -= new Color(0f, 0f, 0f, _vanishMultiple * Time.unscaledDeltaTime);
						dialogueImage_right.GetComponentsInChildren<Image>()[y].color -= new Color(0f, 0f, 0f, _vanishMultiple * Time.unscaledDeltaTime);
					}

					// 如果對話已結束 就讓對話介面慢慢消失
					for (int j = 0; j < dialogueData[i].dialogueTotalList.Count; j++)
					{
						if (j == dialogueData[i].dialogueTotalList[j].toDialogueOrOptionID.Length)
						{
							DialogueManager.instance.dialogieUI.alpha -= (_vanishMultiple * Time.unscaledDeltaTime);
							DialogueManager.instance.optionUI.alpha -= (_vanishMultiple * Time.unscaledDeltaTime);
						}
					}
				}
			}
		}
	}
}
