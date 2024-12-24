using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaninoTV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        exclamationSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (TestMode)
        {
            StartCoroutine(EventTime(TestValue));
            return;
        }
        if (((gc.mode == "story" || gc.mode == "pizza" || gc.mode == "triple" || gc.mode == "free") && Random.Range(1, 3) == 2) || gc.mode == "endless")
        {
            eventWillHappne = true;
            print("event happens");
            timmer = Random.Range(70, 350);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.spoopMode)
        {
            if (eventWillHappne)
            {
                timmer -= Time.deltaTime;
            }
            if (timmer <= 0 && eventWillHappne)
            {
                StartCoroutine(EventTime(2));
                eventWillHappne = false;
            }
        }
    }

    public IEnumerator EventTime(int thing)
    {
        if (!stillBlabbering)
        {
            anim.SetTrigger("neverGoDown");
        }
        panino.GetComponent<AudioSource>().clip = paninoAnnounce[thing];
        if (thing != 0)
        {
            exclamationSound.Play();
            if (stillBlabbering)
            {
                queued = true;
                yield return new WaitUntil(() => !stillBlabbering);
            }
        }
        if (thing != 0)
        {
            if (stillBlabbering)
            {
                queued = true;
                yield return new WaitUntil(() => !stillBlabbering);
            }
            exclamation.SetActive(true);
            yield return new WaitForSeconds(2.5f);
        }
        else
        {
            yield return new WaitForSeconds(1); 
            if (stillBlabbering)
            {
                queued = true;
                yield return new WaitUntil(() => !stillBlabbering);
            }
        }
        queued = false;
        panino.SetActive(false);
        exclamation.SetActive(false);
        tvStatic.SetActive(true);
        stillBlabbering = true;
        yield return new WaitForSeconds(0.25f);
        tvStatic.SetActive(false);
        panino.SetActive(true);
        switch (thing)
        {
            case 0: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber0, duration0, colore0); break;
            case 1: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber1, duration1, colore1); break;
            case 2: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber2, duration2, colore0); break;
        }
        if (thing == 2)
        {
            prisonDoor.ItemsAreNowGoingToJail();
        }
        yield return new WaitForSeconds(paninoAnnounce[thing].length + 0.5f);
        tvStatic.SetActive(true);
        panino.SetActive(false);
        stillBlabbering = false;
        yield return new WaitForSeconds(0.25f);
        tvStatic.SetActive(false);
        if (!queued)
        {
            anim.SetTrigger("alwaysGoUp");
        }
    }

    AudioSource exclamationSound;
    public GameObject exclamation;
    public GameObject panino;
    public GameObject tvStatic;
    Animator anim;

    public GameControllerScript gc;
    [SerializeField]
    float timmer;
    [SerializeField]
    bool eventWillHappne;

    public AudioClip[] paninoAnnounce; // 0 - congrattation, 1 - pillar john, 2 - jailed items

    [SerializeField]
    int TestValue;
    public bool TestMode;

    public PrisonDoor prisonDoor;

    bool stillBlabbering;
    bool queued;

    string[] blabber0 = { "Congrattation!", "You found all 7 Dwaynes,", "now all you need to do is...", "GET OUT." };
    float[] duration0 = { 1.8f, 3f, 2.8f, 1.935f };
    Color[] colore0 = { Color.white, Color.white, Color.white, Color.white };
    string[] blabber1 = { "The Pillar John door in the first cafeteria has just been unlocked,", "and we don't know why.", "You should totes stay out of there bro, please" };
    float[] duration1 = { 4.609f, 2.05f,  2.192f};
    Color[] colore1 = { Color.white, Color.white, Color.white };
    string[] blabber2 = { "I have decided that any items that you have are a big meanie", "and I will put them into the.", "Jail.", "Good luck breaking them out, assuming you have any." };
    float[] duration2 = { 3.61f, 2.64f, 1f, 2.5f };
}
