using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AYE
{
    public class FPS : MonoBehaviour
    {
        [SerializeField] Text fpsText = null;
        float fps = 0;
        int frameCount = 0;
        void LateUpdate()
        {
            fps += Time.unscaledDeltaTime;
            frameCount++;

            if (frameCount >= 1000)
            {
                //if (SettingWindows.instance.isOpen)
                {
                    //fpsText.text = (100f / fps).ToString("F0") + " fps";
                    // 顯示CPU使用率



                    fpsText.text = (1000f / fps).ToString("F0") + "fps RAM( CPU " + MegabytesToGB(SystemInfo.systemMemorySize).ToString("0.0") + " GPU " + MegabytesToGB(SystemInfo.graphicsMemorySize).ToString("0.0") + " )GB\n" + SystemInfo.processorType + "\n" + SystemInfo.graphicsDeviceName + "\n最大解析度" + Screen.currentResolution.width + "X" + Screen.currentResolution.height + " 目前解析度" + Screen.width + "X" + Screen.height;
                }
                frameCount = 0;
                fps = 0f; 
            }
        }
        public float MegabytesToGB(float megabytes)
        {
            return megabytes / 1024f;
        }
    }
}

// 2020 by 阿葉