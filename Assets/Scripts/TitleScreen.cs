using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (evil && PlayerPrefs.GetInt("duplicatedBalls") == 1)
        {
            ebs.ExitGame();
        }
        else if (((System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7)) || PlayerPrefs.GetInt("duplicatedBalls") == 1)
        {
            if (evil)
            {
                return;
            }
            EVILtitle.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public GameObject EVILtitle;
    public bool evil;
    public ExitButtonScript ebs;
}
