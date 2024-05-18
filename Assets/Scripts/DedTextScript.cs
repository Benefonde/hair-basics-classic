using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DedTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    public void PickRandomText(string person)
    {
        CancelInvoke();
        int rng = Random.Range(0, deathMessages.Length - 1);
        text.text = $"{deathMessages[rng]} {person}{dendMessages[rng]}";
        Invoke("LmaoFreakingDIE", 3);
    }

    void LmaoFreakingDIE()
    {
        text.text = string.Empty;
    }

    private TMP_Text text;
    public string[] deathMessages;
    public string[] dendMessages;
}
