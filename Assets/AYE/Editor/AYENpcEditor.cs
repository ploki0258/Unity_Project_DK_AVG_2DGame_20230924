using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// 為AYENpc添加按鈕
[CustomEditor(typeof(AYENpc<>), true)]
[CanEditMultipleObjects]
public class AYENpcEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var npc = target as IAYENpcEditorSupport;
        if (npc.IsSetOK() == false)
        {
            if (GUILayout.Button("自動完成基礎設定"))
            {
                Debug.Log("替你裝好了！ -2023阿葉");
                npc.AutoStart();
                EditorUtility.SetDirty(target);
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("提示", GUILayout.Width(50f)))
        {
           EditorUtility.DisplayDialog("提示", "1. 這個系統仰賴Navigation運作，所以必須要在場地上使用NavMeshSurface物件Bake可走路徑。" +
               "\n\n2. Head子物件為所有視線判定的依據，請放在適當的高度並Z朝向正前方" +
               "\n\n3. 人形角色建議使用RootMotion移動，非人形角色自行在狀態機中撰寫移動。" +
               "\n\n4. 非人形角色可以使用LookTransformY替代Look的功能(戰車砲塔平轉)LookTransformXY則包含垂直跟水平轉向。", "OK");
        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
