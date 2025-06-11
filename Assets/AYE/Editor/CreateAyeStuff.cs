using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CreateAyeStuff : Editor
{
    [MenuItem("GameObject/AYE/Sensor", priority = -1)]
    static public void CreateSensor()
    {
        // 在選擇的物件上建立Sensor
        GameObject go = Selection.activeGameObject;
        if (go != null)
        {
            GameObject sensor = new GameObject("Sensor");
            sensor.transform.SetParent(go.transform);
            sensor.transform.localPosition = Vector3.zero;
            sensor.transform.localRotation = Quaternion.identity;
            sensor.transform.localScale = Vector3.one;
            sensor.AddComponent<Sensor>();
            // 設定Dirty
            EditorUtility.SetDirty(go);
            EditorUtility.SetDirty(sensor);
        }
        else
        {
            GameObject sensor = new GameObject("Sensor");
            sensor.transform.localPosition = Vector3.zero;
            sensor.transform.localRotation = Quaternion.identity;
            sensor.transform.localScale = Vector3.one;
            sensor.AddComponent<Sensor>();
            EditorUtility.SetDirty(sensor);
        }
    }
}
