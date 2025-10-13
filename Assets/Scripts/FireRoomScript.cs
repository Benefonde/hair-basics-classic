using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRoomScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        button.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    return;
                }
                timer = Random.Range(6, 15);
                thatIdiot.SetTrigger($"Idle{Random.Range(1, 3)}");
                voiceline = Random.Range(0, 6);
                thatIdiotAud.PlayOneShot(thatIdiotVoicelines[voiceline]);
                string shutup = "Lol";
                switch (voiceline)
                {
                    case 0: shutup = "There is no universe where I'm not the smartest!"; break;
                    case 1: shutup = "They should let me walk around freely."; break;
                    case 2: shutup = "I'm not meant to be here!"; break;
                    case 3: shutup = "Let me out of here, wont you?"; break;
                    case 4: shutup = "I don't know why I'm trapped here, my genius must frighten them!"; break;
                    case 5: shutup = "Don't be fooled, this is smart jail!"; break;
                }
                FindObjectOfType<SubtitleManager>().Add3DSubtitle(shutup, thatIdiotVoicelines[voiceline].length, new Color(0.3f, 0.35f, 0.45f), thatIdiot.transform, thatIdiotAud);
                break;
            case 1:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    return;
                }
                thatIdiotAud.PlayOneShot(thatIdiotVoicelines[6]);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("What does that do?", thatIdiotVoicelines[6].length, new Color(0.3f, 0.35f, 0.45f), thatIdiot.transform, thatIdiotAud);
                thatIdiot.enabled = false;
                thatIdiot.enabled = true;                
                thatIdiot.SetTrigger("Idle1");
                state = 2;
                timer = thatIdiotVoicelines[6].length + 2;
                break;
            case 2:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    if (timer < 0.5f && !thatIdiotAud.isPlaying)
                    {
                        thatIdiotAud.PlayOneShot(thatIdiotVoicelines[8]);
                        FindObjectOfType<TrophyCollectingScript>().GetTrophy(43);
                    }
                    return;
                }
                thatIdiot.SetTrigger("BURN");
                state = 3;
                timer = 1;
                break;
            case 3:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    return;
                }
                thatIdiotAud.PlayOneShot(thatIdiotVoicelines[7]);
                FindObjectOfType<SubtitleManager>().Add3DSubtitle("EAAAAAAGH!", thatIdiotVoicelines[7].length, new Color(0.3f, 0.35f, 0.45f), thatIdiot.transform, thatIdiotAud);
                state = -1; // * Stop Everything
                break;
        }
    }

    public void Press()
    {
        state = 1;
        timer = 0.5f;
        button.SetTrigger("Press");
        thatIdiotAud.Stop();
        string shutup = "EAAAAAAGH!";
        switch (voiceline)
        {
            case 0: shutup = "There is no universe where I'm not the smartest!"; break;
            case 1: shutup = "They should let me walk around freely."; break;
            case 2: shutup = "I'm not meant to be here!"; break;
            case 3: shutup = "Let me out of here, wont you?"; break;
            case 4: shutup = "I don't know why I'm trapped here, my genius must frighten them!"; break;
            case 5: shutup = "Don't be fooled, this is smart jail!"; break;
        }
        FindObjectOfType<SubtitleManager>().RemoveSubtitle(shutup);
        button.gameObject.GetComponent<AudioSource>().Play();
    }

    public Animator thatIdiot;
    public AudioClip[] thatIdiotVoicelines;
    public AudioSource thatIdiotAud;

    float timer;

    int state;
    int voiceline;
    public Animator button;
}
