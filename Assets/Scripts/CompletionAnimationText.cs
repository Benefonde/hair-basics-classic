using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class CompletionAnimationText : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        txt = GetComponent<TMP_Text>();
        StartCoroutine(PercentGoUp());
    }

    IEnumerator PercentGoUp()
    {
        yield return new WaitForSeconds(0.5f);
        while (percent < 100)
        {
            percent++;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Update()
    {
        txt.text = $"Percent:\n{percent}%";
    }

    TMP_Text txt;

    int percent;
}
