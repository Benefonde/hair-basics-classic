using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (PlayerPrefs.GetInt(trophy, 0) == 1)
        {
            image.color = Color.white;
        }
    }

    private Image image;

    public string trophy;
}
