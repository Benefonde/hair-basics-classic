using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CompletionAnimationText : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        aud = GetComponent<AudioSource>();
        aud2 = gameObject.AddComponent<AudioSource>();
        txt = GetComponent<TMP_Text>();
        StartCoroutine(PercentGoUp());
        aud.pitch = 1;
        aud2.clip = meatophobia;
        aud2.loop = true;
        aud2.Play();
        
    }

    IEnumerator PercentGoUp()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PercentActuallyGoUp());
        yield return new WaitForSeconds(5);
        yield return new WaitUntil(() => percent >= PlayerPrefs.GetInt("completion", 0));
        if (percent == 100)
        {
            GetComponent<TrophyCollectingScript>().GetTrophy(23);
        }
        yield return new WaitForSeconds(1.5f);
        StartRankScreen();
        yield return new WaitForSeconds(5);
        inputToGoBarack = true;
    }

    IEnumerator PercentActuallyGoUp()
    {
        while (percent < PlayerPrefs.GetInt("completion", 0))
        {
            aud.PlayOneShot(blip);
            percent++;
            aud.pitch += 0.02f;
            yield return new WaitForSeconds(0.049f - (percent / 1000));
            if (percent >= 80 && percent < 95)
            {
                yield return new WaitForSeconds(percent / 2000f);
            }
            else if (percent >= 95)
            {
                yield return new WaitForSeconds(percent / 1000f);
            }
        }
    }

    void Update()
    {
        txt.text = $"Completion:\n{percent}%";
        if (Input.anyKey && inputToGoBarack)
        {
            SceneManager.LoadScene("Warning");
        }
    }

    void StartRankScreen()
    {
        aud2.Stop();
        aud2.clip = music;
        aud2.Play();
        rankBg.gameObject.SetActive(true);
        print(percentages.Length);
        for (int i = 0; i < percentages.Length - 1; i++)
        {
            if (percent < percentages[i + 1])
            {
                ranks[i].SetActive(true);
                rankBg.color = bgColors[i];
                print($"{percentages[i]} percent");
                break;
            }
        }
        if (percent == 100)
        {
            ranks[6].SetActive(true);
            rankBg.color = bgColors[6];
            print($"{percentages[6]} percent");
        }
    }

    TMP_Text txt;

    int percent;

    AudioSource aud;
    AudioSource aud2;
    public AudioClip blip;
    public AudioClip meatophobia;
    public AudioClip music;
    public GameObject[] ranks;
    public Image rankBg;
    public Color[] bgColors;

    int[] percentages = { 0, 50, 61, 72, 83, 94, 100, 100 };

    bool inputToGoBarack;
}
