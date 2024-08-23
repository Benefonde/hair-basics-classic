using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChecklistScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        percent.text = $"{cs.percent}%";
    }

    public CompletionScript cs;

    public TMP_Text percent;
}
