using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class A : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        txt = GetComponent<TMP_Text>();
        thing = Random.Range(-28424, 54863);
        coolerThing = Random.Range(30, 170);
    }

    // Update is called once per frame
    void Update()
    {
        thing += Time.deltaTime;
        txt.fontSize = (Mathf.Sin(thing * 5) * 36) + coolerThing;
        rt.localPosition = new Vector2((Mathf.Sin(thing * 4.2716316f) * 100) + 100, (Mathf.Sin(thing) * 90) + 60);
        float thingy = Mathf.Clamp((Mathf.Sin(thing * 10) / 2 + 0.5f), 0.2f, 1f);
        txt.color = new Color(thingy, thingy + 0.2f, thingy - 0.2f);
    }

    RectTransform rt;
    TMP_Text txt;
    float thing;
    int coolerThing;
}
