using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField, Header("角色圖示 左")] Image 角色圖示_左 = null;
    [SerializeField, Header("角色圖示 右")] Image 角色圖示_右 = null;
    [SerializeField, Header("對話人名 左")] Text 對話人名_左 = null;
    [SerializeField, Header("對話人名 右")] Text 對話人名_右 = null;
    [SerializeField, Header("對話內容")] Text 對話內容 = null;
    [SerializeField, Header("提示繼續圖示")] Transform 繼續提示 = null;
    [SerializeField, Header("對話框")] CanvasGroup talkUI;

    //對話文本 當前文本;
    public bool 對話中 = false;    // 是否在對話
    bool pressEnter = false;      // 是否按了繼續
    bool wait = false;            // 是否在等待

    private void Awake()
    {
        instance = this;    // 讓單例等於自己
        talkUI.alpha = 0f;  // 一開始隱藏對話框 α值為0
    }
}
