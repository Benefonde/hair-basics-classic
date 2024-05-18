using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaninocatorOff : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (bsc.antiHearing)
        {
            paninocator.SetActive(false);
        }
        else if (!bsc.antiHearing)
        {
            paninocator.SetActive(true);
        }
    }

    public GameObject paninocator;
    public BaldiScript bsc;
}
