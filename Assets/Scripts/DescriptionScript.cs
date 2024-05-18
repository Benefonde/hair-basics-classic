using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionScript : MonoBehaviour
{
    private void Start()
    {
        txt = GetComponent<TMP_Text>();
    }

    public void SetText(string a)
    {
       txt.text = a;
    }

    private TMP_Text txt;
}
