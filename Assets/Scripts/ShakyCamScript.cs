using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakyCamScript : MonoBehaviour
{
    private void Start()
    {
        enableShaky = PlayerPrefs.GetInt("shaky", 1);
    }

    void Update()
    {
        if (enableShaky == 1)
        {
            float ran1 = Random.Range(-intensity.x, intensity.x);
            float ran2 = Random.Range(-intensity.y, intensity.y);
            float ran3 = Random.Range(-intensity.z, intensity.z);

            if (uiElement)
            {
                Vector3 randomOffset = new Vector3(ran1, ran2, ran3);
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.anchoredPosition = offset + randomOffset;
            }
            else
            {
                Vector3 randomOffset = new Vector3(ran1, ran2, ran3);
                transform.position = offset + randomOffset;
            }
        }
    }

    public Vector3 offset;

    public Vector3 intensity;

    private int enableShaky;

    public bool uiElement;
}
