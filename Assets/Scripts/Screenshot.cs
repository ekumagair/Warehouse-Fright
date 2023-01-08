using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F5) && GlobalScript.debug)
        {
            ScreenCapture.CaptureScreenshot("WarehouseFrightScreenshot_" + SceneManager.GetActiveScene().name + " " + Random.Range(0, 10000).ToString() + ".png");
        }
    }
}
