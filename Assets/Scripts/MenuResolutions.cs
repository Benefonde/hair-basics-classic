using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuResolutions : MonoBehaviour
{
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredRes = new List<Resolution>();

        resOptions.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            filteredRes.Add(resolutions[i]);
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredRes.Count; i++)
        {
            string resolutionOption = filteredRes[i].width + "x" + filteredRes[i].height;
            options.Add(resolutionOption);
            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height)
            {
                print("Added " + resolutionOption + " to filtered resolutions");
                currentResolutionIndex = i;
            }
        }

        if (options.Count == 0)
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                filteredRes.Add(resolutions[i]);
            }
        }

        resOptions.AddOptions(options);
        resOptions.value = currentResolutionIndex;
        resOptions.RefreshShownValue();
    }

    public void SetRes(int resIndex)
    {
        Resolution res = filteredRes[resIndex];
        int FullscreenOption = PlayerPrefs.GetInt("fullscreen", 1);
        if (FullscreenOption == 1)
        {
            Screen.SetResolution(res.width, res.height, true);
            print("Setting resolution to " + res.width + "x" + res.height + " with fullscreen");
        }
        else
        {
            Screen.SetResolution(res.width, res.height, false);
            print("Setting resolution to " + res.width + "x" + res.height + " without fullscreen");
        }
    }

    public TMP_Dropdown resOptions;

    public Resolution[] resolutions;
    public List<Resolution> filteredRes;

    public float currentRefreshRate;
    public int currentResolutionIndex = 0;
}
