using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BenefondCratesSoCoolBro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(1, 21) == 2 || TestMode == 1)
        {
            benefondCreatesAud.clip = benefondCratesEasterEggs[Random.Range(0, benefondCratesEasterEggs.Length - 1)];
        }
        benefondCreatesAud.Play();

        if (PlayerPrefs.GetInt("duplicatedBalls", 0) == 1 || TestMode == 2)
        {
            benefondCreates.SetActive(false);
            cam = Camera.main.transform;
            aud = GetComponent<AudioSource>();
            aud.Play();
            aud.loop = true;
            t = GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("duplicatedBalls", 0) == 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("BenefondCreates");
            }
        }

        if (sorryTime)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= 300 || TestMode == 2)
        {
            sorryTime = true;
            StartCoroutine(Sorry("Are you sorry for what you've done?"));
            aud.Stop();
            aud.clip = cooLSong;
            aud.Play();
        }
    }

    IEnumerator Sorry(string text, bool choice = true)
    {
        choices.SetActive(false);
        t.text = string.Empty;
        char[] c = text.ToCharArray();
        for (int i = 0; i < text.Length; i++)
        {
            t.text += c[i];
            if (!char.IsWhiteSpace(c[i]))
            {
                aud.PlayOneShot(blip);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        if (!choice)
        {
            SceneManager.LoadScene("BenefondCrates");
            yield break;
        }
        aud.PlayOneShot(appear);
        choices.SetActive(true);
    }

    public void Yes()
    {
        PlayerPrefs.SetInt("duplicatedBalls", 0);
        StartCoroutine(Sorry("Glad to hear it.", false));
        FindObjectOfType<TrophyCollectingScript>().GetTrophy(26);
    }

    public void NoForButtons()
    {
        StartCoroutine(No());
    }

    IEnumerator No()
    {
        StartCoroutine(Sorry("                                                                                                                    ", false));
        for (int i = 0; i < 24; i++)
        {
            GameObject AAAAAAAAAAAAAAA = Instantiate(balls, transform.parent);
            AAAAAAAAAAAAAAA.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-40, 40), Random.Range(-480, -12), 0);
            AAAAAAAAAAAAAAA.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("BenefondCrates");
        print("it dies succ ess fully");
    }

    public GameObject benefondCreates;
    [SerializeField]
    float timer;
    TMP_Text t;
    bool sorryTime;
    
    AudioSource aud;
    [SerializeField]
    AudioClip blip;
    [SerializeField]
    AudioClip appear;

    public GameObject choices;

    public int TestMode;

    public GameObject balls;

    Transform cam;

    public AudioClip cooLSong;

    public AudioSource benefondCreatesAud;
    public AudioClip[] benefondCratesEasterEggs;
}
