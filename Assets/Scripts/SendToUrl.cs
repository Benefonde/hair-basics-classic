using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToUrl : MonoBehaviour
{
    public void Open()
    {
        Application.OpenURL(url);
    }

    public string url;
}
