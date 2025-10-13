using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextboxSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public void Spawn()
    {
        if (!susieIdea)
        {
            Transform canvas = transform.GetComponentInParent<Canvas>().transform;
            GameObject a = Instantiate(textbox, canvas);
            a.SetActive(true);
            a.GetComponent<UtTextboxScript>().dialogue = dialogue;
            a.GetComponent<UtTextboxScript>().specialEvent = special;
            if (special == 3)
            {
                a.GetComponent<UtTextboxScript>().takingTooLong = guy;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("seenSusie") >= 1)
            {
                textbox.GetComponent<UtTextboxScript>().dialogue = dialogueAlt[0];
            }
            if (PlayerPrefs.GetInt("jackensteinBeat") == 1)
            {
                textbox.GetComponent<UtTextboxScript>().dialogue = dialogueAlt[1];
            }
            textbox.SetActive(true);
        }
    }
    public GameObject textbox;
    public Dialogue dialogue;
    public Dialogue[] dialogueAlt;
    public int special;
    public GameObject guy;
    public bool susieIdea;
}
