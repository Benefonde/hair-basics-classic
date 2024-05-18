using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowItemSlide : MonoBehaviour
{
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rectTransform.anchoredPosition += Vector2.down * force * Time.deltaTime;

            if (rectTransform.anchoredPosition.y < -boundEnd)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -boundEnd);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rectTransform.anchoredPosition += Vector2.up * force * Time.deltaTime;

            if (rectTransform.anchoredPosition.y > boundEnd)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, boundEnd);
            }
        }
    }
    public float boundEnd;
    public int force;
    private RectTransform rectTransform;
}
