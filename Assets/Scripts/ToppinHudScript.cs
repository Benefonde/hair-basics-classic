using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToppinHudScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            toppin[i].color = Color.black;
        }
    }

    public void ToppinRecruit(int thatone)
    {
        toppin[thatone].color = Color.white;
        me.PlayOneShot(toppinAdded);
    }

    public Image[] toppin = new Image[5];
    public AudioClip toppinAdded;
    public AudioSource me;
}
