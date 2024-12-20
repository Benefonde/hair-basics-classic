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
        if (((gc.mode == "story" || gc.mode == "pizza" || gc.mode == "triple" || gc.mode == "free") && Random.Range(1, 10) == 5) || gc.mode == "endless")
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
        if (thing == 2)
        {
            prisonDoor.ItemsAreNowGoingToJail();
        }
        yield return new WaitForSeconds(paninoAnnounce[thing].length);
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
}
