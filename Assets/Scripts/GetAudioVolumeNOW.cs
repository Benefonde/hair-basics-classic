using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioVolumeNOW : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("audio", 0.5f);
        print(AudioListener.volume);
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("audio", 0.5f);
    }
}
