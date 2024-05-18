using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingCountedScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        tc.collectToppingsNeeded++;
    }

    public TrophyCollectingScript tc;
}
