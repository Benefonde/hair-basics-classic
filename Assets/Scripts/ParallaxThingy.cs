using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxThingy : MonoBehaviour
{
    void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (posyshynObject != null)
        {
            posyshyn = posyshynObject.localPosition;
        }
        trans.localPosition = new Vector2(Mathf.RoundToInt(posyshyn.x * markiplier), Mathf.RoundToInt(posyshyn.y * markiplier));
    }

    public float markiplier; // multiplier
    public Vector2 posyshyn;

    RectTransform trans;

    public RectTransform posyshynObject;
}
