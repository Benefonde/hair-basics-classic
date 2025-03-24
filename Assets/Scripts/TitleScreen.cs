using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private void OnEnable()
    {
        if (!chess) //if (System.DateTime.Now.Month == 4 && System.DateTime.Now.Day == 1 && !chess)
        {
            CHESStitle.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (System.DateTime.Now.Month == 4 && System.DateTime.Now.Day == 1)
        {
            primaaprilis.SetActive(true);
        }

        if (evil && PlayerPrefs.GetInt("duplicatedBalls") == 1)
        {
            ebs.ExitGame();
        }
        else if (((System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7)) || PlayerPrefs.GetInt("duplicatedBalls") == 1)
        {
            if (evil || chess)
            {
                return;
            }
            EVILtitle.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public GameObject EVILtitle;
    public GameObject CHESStitle;
    public bool evil;
    public bool chess;
    public ExitButtonScript ebs;

    public GameObject primaaprilis;
}
