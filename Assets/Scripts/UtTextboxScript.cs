using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UtTextboxScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        portrait.sprite = null;
        txt.text = string.Empty;
        StartCoroutine(Text());
    }

    IEnumerator Text()
    {
        print(dialogue.text.Count);
        for (int i = 0; i < dialogue.text.Count ; i++)
        {
            txt.text = string.Empty;
            portrait.sprite = dialogue.sprites[i];
            for (int j = 0; j < dialogue.text[i].Length; j++)
            {
                string c = dialogue.text[i].ToCharArray()[j].ToString();
                if (c != "ą")
                {
                    if (c == "ć")
                    {
                        txt.text += "  ";
                    }
                    else
                    {
                        txt.text += c;
                    }
                    if (c != "ć")
                    {
                        if (!Input.GetKey(KeyCode.X))
                        {
                            GetComponent<AudioSource>().PlayOneShot(dialogue.blips[i]);
                            yield return new WaitForSeconds(1 / 30f);
                        }
                    }
                }
                if (c == "ż")
                {
                    txt.text += "<b>";
                }
                if (c == "ź")
                {
                    txt.text += "</b>";
                }
                if ((c == "," || c == "." || c == "?" || c == "ą") && !Input.GetKey(KeyCode.X))
                {
                    yield return new WaitForSeconds(1 / 6f);
                }
            }
            if (Input.GetKey(KeyCode.X))
            {
                GetComponent<AudioSource>().PlayOneShot(dialogue.blips[i]);
            }
            if (specialEvent != 2 && specialEvent != 3)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
                yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Z));
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }
        switch (specialEvent)
        {
            case 1: menus[0].SetActive(false); menus[1].SetActive(true); start.StartGame(); break;
            case 3: takingTooLong.SetActive(true); break;
        }
        Destroy(gameObject);
    }

    public Image portrait;
    public TMP_Text txt;

    public Dialogue dialogue;

    public int specialEvent;
    /*
    0 - nothing
    1 - load jackenstein mode
    2 - autoskip
    3 - autoskip + YOUR TAKING TOO LONG 
    */

    public GameObject[] menus; // 0 completion menu 1 load screen
    public StartButton start;

    public GameObject takingTooLong;
}
