using UnityEngine;
using TMPro;

public class VersionScript : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = Application.version;
    }
}
