using UnityEngine;
using TMPro;

public class VersionScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = prefix + Application.version + suffix;
        if ((System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7))
        {
            GetComponent<TMP_Text>().text = $"EVIL {prefix}{GetComponent<TMP_Text>().text}{suffix}";
        }

        if (chess)
        {
            GetComponent<TMP_Text>().text = prefix + Application.version + "\n" + suffix;
            if ((System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7))
            {
                GetComponent<TMP_Text>().text = $"EVIL {prefix}{GetComponent<TMP_Text>().text}\n{suffix}";
            }
        }
    }

    public string prefix, suffix;

    public bool chess;
}
