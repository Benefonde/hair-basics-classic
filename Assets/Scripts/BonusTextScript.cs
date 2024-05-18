using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text.text = PlayerPrefs.GetString("bonusTextString");
    }

    public TMP_Text text;
}
