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
        if (PlayerPrefs.GetInt("duplicatedBalls", 0) == 1)
        {
            benefondCreates.SetActive(false);
            aud = GetComponent<AudioSource>();
            aud.Play();
            t = GetComponent<TMP_Text>();
        }
    }

    private void Update()
    {
        if (sorryTime)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= 300)
        {
            sorryTime = true;
            StartCoroutine(Sorry("Are you sorry for what you've done?"));
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
            aud.PlayOneShot(blip);
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

    public void No()
    {
        Application.Quit();
        StartCoroutine(Sorry("GET OUT:bangbang:", true));
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
}
