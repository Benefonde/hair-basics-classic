using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        eventCooldown = Random.Range(100, 200);
        events = Random.Range(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (eventCooldown > 0 && events > 1)
        {
            eventCooldown -= Time.deltaTime;
        }
        else if (events > 1)
        {
            StartCoroutine(RingBell());
            eventCooldown = Random.Range(100, 200);
        }
    }

    IEnumerator RingBell()
    {
        if (events <= 0)
        {
            yield break;
        }
        events--;
        aud.loop = false;
        aud.clip = ring;
        aud.Play();
        yield return new WaitForSeconds(1.5f);
        int ev = Random.Range(1, 5);
        if (eventDone[ev - 1] == true)
        {
            for (; ; )
            {
                ev = Random.Range(1, 5);
                if (eventDone[ev - 1] == false)
                {
                    break;
                }
            }
        }
        if (Random.Range(1, 30) == 6)
        {
            ev = 6;
        } 
        eventDone[ev - 1] = true;
        switch (ev)
        {
            default: StartCoroutine(ShowEventDesc($"Event number {ev} doesn't exist! Report to developer now!")); break;
            case 1: StartCoroutine(ShowEventDesc("Looks like the leak is leaking again. Watch out for the whirlpools!")); break;
            case 2: StartCoroutine(ShowEventDesc("Baldi's fog machine malfunctioned again! Ack! Hack!")); break;
            case 3: StartCoroutine(ShowEventDesc("Party at the Principal's office! Come and get your present!")); break;
            case 4: StartCoroutine(ShowEventDesc("Test procedure imminent! We use these doors when students try to escape!")); break;
            case 5: StartCoroutine(ShowEventDesc("Uh oh, sounds like Baldi is going to need a new ruler...")); break;
            case 6: StartCoroutine(ShowEventDesc("What the...!? Baldi's releasing the locusts!")); break;
        }
        StartEvent(ev);
    }

    IEnumerator ShowEventDesc(string text)
    {
        eventTextBg.SetActive(true);
        eventText.text = text;
        yield return new WaitForSeconds(5);
        eventTextBg.SetActive(false);
    }

    void StartEvent(int ev)
    {
        switch (ev)
        {
            default: return;
            case 1: flood.SetActive(true); break;
            case 2: 
                RenderSettings.fog = true; 
                RenderSettings.fogColor = Color.white; 
                RenderSettings.fogMode = FogMode.Exponential; 
                RenderSettings.fogDensity = Mathf.Lerp(0, 0.75f, 1.5f);
                aud.clip = fog;
                StartCoroutine(FogStart());
                break;
            case 3: StartCoroutine(PartyStart()); break;
            case 4: StartCoroutine(TestProcedure()); break;
            //case 5: gc.baldiScript.BreakRuler(); break;
            case 6: 
                for (int i = 0; i < 31; i++)
                {
                    LocustScript locuste = Instantiate(locust).GetComponent<LocustScript>();
                    locuste.wanderer = gc.baldiScript.wanderer;
                    locuste.wanderTarget = gc.baldiScript.wanderTarget;
                    locuste.player = gc.playerTransform;
                    locuste.gc = gc;
                }
                break;
        }
    }

    IEnumerator FogStart()
    {
        yield return new WaitForSeconds(fog.length);
        RenderSettings.fogDensity = Mathf.Lerp(0.75f, 0, 1.5f);
        yield return new WaitForSeconds(1.5f);
        RenderSettings.fog = false;
    }

    IEnumerator PartyStart()
    {
        itemParty.SetActive(true);
        itemParty.GetComponent<PickupScript>().ID = itemIdsForParty[Random.Range(0, 2)];
        Texture itemTexture = gc.itemTextures[itemParty.GetComponent<PickupScript>().ID];
        Sprite itemSprite = Sprite.Create((Texture2D)itemTexture, new Rect(0, 0, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), itemTexture.width * 1.55f);
        itemParty.GetComponent<PickupScript>().GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;
        aud.clip = party;
        aud.loop = true;
        aud.Play();
        yield return new WaitForSeconds(Random.Range(50, 80));
        itemParty.SetActive(false);
        aud.clip = null;
        aud.loop = false;
        aud.Stop();
    }

    IEnumerator TestProcedure()
    {
        for (int i = 0; i < countdown.Length; i++)
        {
            aud.PlayOneShot(countdown[i]);
            yield return new WaitForSeconds(countdown[i].length);
        }
        for (int i = 0; i < lockdownDoors.Length; i++)
        {
            lockdownDoors[i].SetTrigger("goDown");
        }
        yield return new WaitForSeconds(Random.Range(30, 60));
        for (int i = 0; i < lockdownDoors.Length; i++)
        {
            lockdownDoors[i].SetTrigger("goUp");
        }
    }

    [SerializeField]
    private float eventCooldown;
    [SerializeField]
    private int events;

    public GameObject flood;

    public GameControllerScript gc;
    public GameObject itemParty;

    public AudioSource aud;
    public AudioClip ring;
    public AudioClip fog;
    public AudioClip party;

    public GameObject eventTextBg;
    public TMP_Text eventText;

    [SerializeField]
    private float fogTimer;

    private bool[] eventDone = new bool[6] 
    { false, false, false, false, false, false };

    private int[] itemIdsForParty = { 4, 10, 11 };

    public AudioClip[] countdown;
    public Animator[] lockdownDoors;

    public GameObject locust;
}
