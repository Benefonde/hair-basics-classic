using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectPos : MonoBehaviour
{
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (UI)
        {
            nonPixelPerfectPos = rectTrans.localPosition;
        }
        else
        {
            nonPixelPerfectPos = trans.localPosition;
        }
    }

    void LateUpdate()
    {
        if (UI)
        {
            rectTrans.localPosition = new Vector2(Mathf.RoundToInt(rectTrans.localPosition.x), Mathf.RoundToInt(rectTrans.localPosition.y));
        }
        else
        {
            trans.localPosition = new Vector3(Mathf.RoundToInt(trans.localPosition.x), Mathf.RoundToInt(trans.localPosition.y), Mathf.RoundToInt(trans.localPosition.z));
        }
    }

    public bool UI;

    RectTransform rectTrans;
    Transform trans;

    public Vector2 nonPixelPerfectPos;
}
