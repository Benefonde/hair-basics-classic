using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayerScript : MonoBehaviour
{
    void Start()
    {
        audioDevice = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        print(dropdown.value);
        paused = false;
        audioDevice.clip = music[dropdown.value];
        audioDevice.time = 0;
        audioDevice.Play();
        slider.maxValue = 1;
    }

    public void StopMusic()
    {
        audioDevice.Stop();
    }

    public void SkipTo(float value)
    {
        if (-value > audioDevice.time)
        {
            print("detected going to far so now setting time to 0");
            audioDevice.time = 0;
        }
        else if (audioDevice.time + value > audioDevice.clip.length)
        {
            print("detected going to far so now setting time to" + audioDevice.clip.length);
            audioDevice.time = audioDevice.clip.length;
        }
        else
        {
            audioDevice.time += value;
        }
    }

    public void LoopToggle()
    {
        if (audioDevice.loop)
        {
            audioDevice.loop = false;
            loopText.text = "Loop OFF";
        }
        else
        {
            audioDevice.loop = true;
            loopText.text = "Loop ON";
        }
    }

    private void Update()
    {
        if (audioDevice.clip != null)
        {
            var plaeing = TimeSpan.FromSeconds(audioDevice.time);
            var timelol = TimeSpan.FromSeconds(audioDevice.clip.length);
            slider.value = audioDevice.time / audioDevice.clip.length;

            timeElapsedAndLeft.text = string.Format(@"{0:m\:ss\:ff}/{1:m\:ss\:ff}", plaeing, timelol);
        }
    }



    public void PauseClicked()
    {
        if (paused)
        {
            audioDevice.UnPause();
        }
        else
        {
            audioDevice.Pause();
        }
        paused = !paused;
    }

    public Slider slider;
    public TMP_Text timeElapsedAndLeft;
    public TMP_Dropdown dropdown;

    private AudioSource audioDevice;
    public AudioClip[] music;

    private bool paused;

    public TMP_Text loopText;
}
