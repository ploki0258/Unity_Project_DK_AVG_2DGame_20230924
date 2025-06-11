using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSayDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaySystem.instance.Say(對話結束了,
            "這是簡易版文本的對話示範，",
            "基於簡易方便使用的原則...",
            "這種寫法一次只允許顯示一行內容。",
            "另外如果要求回傳值，只會回傳0。",
            "如果需要更複雜的功能請建立SayStuff文本。");
    }
    void 對話結束了(int i)
    {
        Debug.Log("對話結束收到了 : " + i);
    }
}
