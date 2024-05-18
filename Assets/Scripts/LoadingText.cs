using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    void Start()
    {
        music.Play();
        StartCoroutine(TextChange());
    }
    IEnumerator TextChange()
    {
        int loadingText = Mathf.RoundToInt(Random.Range(0f, loadText.Length - 1));
        text.text = loadText[loadingText];
        float time = Random.Range(0.5f, 2.0f);
        yield return new WaitForSeconds(time);
        StartCoroutine(TextChange());
    }

    public AudioSource music;

    public TMP_Text text;

    public string[] loadText;

}
