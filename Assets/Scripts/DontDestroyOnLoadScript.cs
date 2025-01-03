using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (die)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        if (unscaled)
        {
            yield return new WaitForSecondsRealtime(dieTime);
        }
        else
        {
            yield return new WaitForSeconds(dieTime);
        }
        Destroy(gameObject);
    }

    public bool die;

    public float dieTime = 3.5f;
    public bool unscaled;
}
