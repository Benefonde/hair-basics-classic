using UnityEngine;
using TMPro;

public class VersionScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Application.version;
        if ((System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7))
        {
            GetComponent<TMP_Text>().text = $"EVIL {GetComponent<TMP_Text>().text}";
        }
    }
}
