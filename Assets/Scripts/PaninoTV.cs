using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaninoTV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("state", 3);
        exclamationSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (TestMode)
        {
            StartCoroutine(EventTime());
            return;
        }
        if ((gc.mode == "story" || gc.mode == "pizza") && Random.Range(1, 20) == 5)
        {
            eventWillHappne = true;
            print("event happens");
            timmer = Random.Range(70, 700);
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
                StartCoroutine(EventTime());
                eventWillHappne = false;
            }
        }
    }

    IEnumerator EventTime()
    {
        anim.SetFloat("state", 1); // 0 - stay, 1 - go down, 2 - go up, 3 nothing
        exclamationSound.Play();
        exclamation.SetActive(true);
        yield return new WaitForEndOfFrame();
        anim.SetFloat("state", 0);
        yield return new WaitForSeconds(2.5f);
        exclamation.SetActive(false);
        tvStatic.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        tvStatic.SetActive(false);
        panino.SetActive(true);
        yield return new WaitForSeconds(10);
        tvStatic.SetActive(true);
        panino.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        tvStatic.SetActive(false);
        anim.SetFloat("state", 2);
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

    public bool TestMode;
}
