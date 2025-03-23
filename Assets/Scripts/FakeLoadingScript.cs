using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoadingScript : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<CursorControllerScript>().UnlockCursor();
        Time.timeScale = 0;
        StartCoroutine(LoadingTime());
    }

    IEnumerator LoadingTime()
    {
        yield return new WaitForSecondsRealtime(Random.Range(0.1f, 0.4f));
        while (progess.value != progess.maxValue && !IMIMPATIENTRAAAAHHHHHH)
        {
            progess.value += Random.Range(0.004f, 0.1f);
            if (Random.Range(1, 9) == 4)
            {
                yield return new WaitForSecondsRealtime(Random.Range(0f, 1.4f));
            }
            else
            {
                yield return new WaitForSecondsRealtime(Random.Range(0f, 0.2f));
            }
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
        music.Play();
    }

    public AudioSource music;
    public Slider progess;

    public bool IMIMPATIENTRAAAAHHHHHH;
}
