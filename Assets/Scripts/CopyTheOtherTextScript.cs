using UnityEngine;
using TMPro;

public class CopyTheOtherTextScript : MonoBehaviour
{
    void Start()
    {
        myTxt = GetComponent<TMP_Text>();
    }

    void Update()
    {
        myTxt.text = otherTxt.text;
    }

    public TMP_Text otherTxt;
    TMP_Text myTxt;
}
