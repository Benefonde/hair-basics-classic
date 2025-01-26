using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    public void Toggle()
    {
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            return;
        }
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
