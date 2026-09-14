using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
#if UNITY_STANDALONE && !UNITY_WEBGL
    // Attach this component to any GameObject in the scene to allow screenshots to be taken.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            string filename = Application.productName + "_v" + Application.version.ToString() + "_screenshot " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";

            ScreenCapture.CaptureScreenshot(Path.Combine(Application.persistentDataPath, filename));

            if (Debug.isDebugBuild == true)
            {
                Debug.Log("Took screenshot. Saved it as: " + filename);
            }
        }
    }
#endif
}
