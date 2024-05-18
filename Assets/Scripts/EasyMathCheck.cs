using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyMathCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public void Check()
    {
        if (easyMath == 1)
        {
            PlayerPrefs.SetInt(saveThing, 0);
            PlayerPrefs.Save();
        }
        else if (easyMath == 0)
        {
            PlayerPrefs.SetInt(saveThing, 1);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        easyMath = PlayerPrefs.GetInt(saveThing, 0);
        if (easyMath == 1)
        {
            sprite.SetActive(true);
        }
        else if (easyMath == 0)
        {
            sprite.SetActive(false);
        }
    }

    public int easyMath;

    public string saveThing;

    public GameObject sprite;
}
