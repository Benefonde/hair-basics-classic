using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

public class MoveAllTheTiem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.Translate(amount);
        if (goingBackGexualTime)
        {
            if (rt.anchoredPosition.x < tpWhen)
            {
                rt.anchoredPosition = new Vector2(tpTo, rt.anchoredPosition.y);
            }
        }
        else
        {
            if (rt.anchoredPosition.x > tpWhen)
            {
                rt.anchoredPosition = new Vector2(tpTo, rt.anchoredPosition.y);
            }
        }
    }

    public Vector3 amount;
    public int tpWhen;
    public int tpTo;

    public bool goingBackGexualTime;

    private RectTransform rt;
}
