using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudSizerScript : MonoBehaviour
{
    void Start()
    {
        canvas = GetComponent<CanvasScaler>();
        scaleMode = PlayerPrefs.GetInt("scaleMode", 0);
        if (scaleMode == 0)
        {
            canvas.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.matchWidthOrHeight = scaleWidthAndHeight;
            canvas.referenceResolution = refRes;
        }
        if (scaleMode == 1)
        {
            canvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvas.scaleFactor = PlayerPrefs.GetFloat("scaleFactor", 1);
        }
    }

    void Update()
    {
        if (scaleMode != PlayerPrefs.GetInt("scaleMode", 0))
        {
            scaleMode = PlayerPrefs.GetInt("scaleMode", 0);
            if (scaleMode == 0)
            {
                canvas.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvas.matchWidthOrHeight = scaleWidthAndHeight;
                canvas.referenceResolution = refRes;
            }
            if (scaleMode == 1)
            {
                canvas.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                canvas.scaleFactor = PlayerPrefs.GetFloat("scaleFactor", 1);
            }
        }
        if (PlayerPrefs.GetFloat("scaleFactor", 1) != canvas.scaleFactor && scaleMode == 1)
        {
            canvas.scaleFactor = PlayerPrefs.GetFloat("scaleFactor", 1);
        }
    }

    private CanvasScaler canvas;

    private int scaleMode;

    public Vector2 refRes;
    public float scaleWidthAndHeight;
}
