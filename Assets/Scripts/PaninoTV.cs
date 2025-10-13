using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaninoTV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < paninoAnnounce.Length; i++)
        {
            eventsDone.Add(0);
        }
        exclamationSound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (TestMode)
        {
            StartCoroutine(EventTime(TestValue));
            return;
        }
        if ((gc.mode == "story" || gc.mode == "pizza" || gc.mode == "triple" || gc.mode == "free") || gc.mode == "endless")
        {
            eventWillHappne = true;
            timmer = Random.Range(30, 140);
            print($"event happens in {timmer} seconds");
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
                ItsTimeForARandomEventBrother();
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (TestMode)
            {
                StartCoroutine(EventTime(TestValue));
            }
            else
            {
                eventWillHappne = false;
                TestMode = true;
            }
        }
    }

    void ItsTimeForARandomEventBrother()
    {
        if (totalEventsDone >= 2 && gc.mode != "endless")
        {
            eventWillHappne = false;
            return; //ENOUGH!! thats ENOUGH
        }
        int eventee = Random.Range(3, paninoAnnounce.Length);
        if (gc.IsAprilFools() || Random.Range(1, 280) == 4)
        {
            eventee = 2;
        }
        if (eventsDone[eventee] == 0)
        {
            StartCoroutine(EventTime(eventee));
        }
        if (gc.mode != "endless")
        {
            eventsDone[eventee] = 1;
            totalEventsDone++;
        }
        if (Random.Range(1, 8) != 3)
        {
            timmer = Random.Range(50, 120);
            if (gc.mode == "endless")
            {
                timmer += Random.Range(15, 45);
            }
            return;
        }
        eventWillHappne = false;
    }

    public IEnumerator EventTime(int thing)
    {
        if (!stillBlabbering)
        {
            anim.SetTrigger("neverGoDown");
        }
        if (thing != 0)
        {
            exclamationSound.Play();
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
        panino.GetComponent<AudioSource>().clip = paninoAnnounce[thing];
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
            case 2: FindObjectOfType<SubtitleManager>().Add2DSubtitle("How do I get him off", paninoAnnounce[2].length, Color.white); break;
            case 3: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber2, duration2, colore0); break;
            case 4: FindObjectOfType<SubtitleManager>().Add2DSubtitle("I agree, let's release the angry bees", paninoAnnounce[4].length, Color.white); break;
            case 5: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber3, duration3, colore3); break;
            case 6: FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(blabber4, duration4, colore1); break;
                //mandatory break i promise you its very madatory
                //for something that ruined counting for me
            case 7: FindObjectOfType<SubtitleManager>().Add2DSubtitle("Look at this", paninoAnnounce[7].length, Color.white); break;
        }
        switch (thing)
        {
            case 2: washeewashee.SetActive(true); break;
            case 3: prisonDoor.ItemsAreNowGoingToJail(); break;
            case 4: StartCoroutine(RollOutTheAngryBees()); break;
            case 5:
                for (int i = 0; i < 79; i++)
                {
                    GameObject a = Instantiate(gc.locust);
                    a.SetActive(true);
                    a.transform.name = "Locust";
                }
                break;
            case 6: StartCoroutine(PizzaTime()); break;
                //mandatory break i promise you its very madatory
                //for something that ruined counting for me
            case 7: StartCoroutine(TobyFoxReferenceNoWay()); break;
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

    IEnumerator RollOutTheAngryBees()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(angryBees, gc.AILocationSelector.GetNewTargetQuick(false), Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator PizzaTime()
    {
        pizzaMusic.volume = 0.7f;
        pizzaHud.SetActive(true);
        pizzaHudText.text = "8";
        count = 8;
        pizzaMusic.Play();
        pizzaSlices.SetActive(true);
        gc.playerScript.walkSpeed += 10;
        gc.playerScript.runSpeed += 15;
        for (int i = 0; i < 48; i++)
        {
            yield return new WaitForSeconds(1);
            pizzaHud.GetComponent<Image>().color -= Color.white / 48;
            pizzaHud.GetComponent<Image>().color += Color.black;
            if (pizzaHudText.text == "0")
            {
                if (gc.HasItemInInventory(0))
                {
                    gc.CollectItem(gc.CollectItemExcluding(0, 2, 3, 6, 8, 7, 9, 10, 14, 15, 16, 18, 22, 23, 24, 25, 26));
                }
                else
                {
                    gc.player.health += 50;
                    gc.player.stamina += 50;
                }
                gc.player.stamina += 100;
                break; 
            }
        }
        pizzaHud.SetActive(false);
        gc.playerScript.walkSpeed -= 8;
        gc.playerScript.runSpeed -= 12;
        pizzaSlices.SetActive(false);
        for (int i = 0; i < 20; i++)
        {
            pizzaMusic.volume -= 0.035f;
            yield return new WaitForSeconds(1 / 30);
        }
        pizzaMusic.Stop();
    }
    IEnumerator TobyFoxReferenceNoWay()
    {
        for (int i = 0; i < 6; i++)
        {
            fox.SetTrigger("hi");
            yield return new WaitForSeconds(Random.Range(7, 28));
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

    public GameObject washeewashee;

    public AudioClip[] paninoAnnounce;

    [SerializeField]
    int TestValue;
    public bool TestMode;

    List<int> eventsDone = new List<int>();

    int totalEventsDone;

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
    string[] blabber3 = { "Ummm we kinda angered a god and now we have to deal with locusts.", "Sorry." };
    float[] duration3 = { 5f, 1f };
    Color[] colore3 = { Color.white, Color.white };
    string[] blabber4 = { "There's a pizza party going on!", "Get the slices of pizza to earn yourself a free item.", "Be quick though!" };
    float[] duration4 = { 2f, 3.5f, 1f };

    public GameObject angryBees;

    public GameObject pizzaSlices;
    public AudioSource pizzaMusic;
    public GameObject pizzaHud;
    public TMP_Text pizzaHudText;
    public int count;

    public Animator fox;
}
