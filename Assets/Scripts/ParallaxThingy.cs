using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxThingy : MonoBehaviour
{
    void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (posyshynObjecD != null)
        {
            posyshyn = posyshynObjecD.localPosition;
        }
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
    public Transform posyshynObjecD;
}
