﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RankScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        letter = PlayerPrefs.GetString("pizzaRankNeww", "D");
        score = PlayerPrefs.GetInt("pizzaScoreNeww", 0);
        if (score < 2000)
        {
            rank.clip = D;
            text.color = new Color32(48, 80, 120, 255);
            text.text = "D";
        }
        if (score >= 2000 && score < 4000)
        {
            rank.clip = C;
            text.color = new Color32(96, 208, 72, 255);
            text.text = "C";
        }
        if (score >= 4000 && score < 8000)
        {
            rank.clip = C;
            text.color = new Color32(48, 168, 248, 255);
            text.text = "B";
        }
        if (score >= 8000 && score < 16000)
        {
            rank.clip = A;
            text.color = new Color32(248, 0, 0, 255);
            text.text = "A";
        }
        if (score >= 16000 && letter == "S")
        {
            rank.clip = S;
            text.color = new Color32(244, 144, 0, 255);
            text.text = "S";
        }
        if (score >= 16000 && letter == "P")
        {
            rank.clip = P;
            text.color = new Color32(152, 80, 248, 255);
            text.text = "P";
        }
        scoreText.text = score.ToString();
        scoreText.color = text.color;
        rank.Play();
        Invoke("CheckForConfettiTime", 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rank.isPlaying == false)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void CheckForConfettiTime()
    {
        if (letter == "A")
        {
            confetti[0].SetActive(true);
        }
        if (letter == "S")
        {
            confetti[0].SetActive(true);
            confetti[1].SetActive(true);
        }
        if (letter == "P")
        {
            confetti[0].SetActive(true);
            confetti[1].SetActive(true);
            confetti[2].SetActive(true);
        }
    }

    public AudioSource rank;

    public AudioClip D;
    public AudioClip C;
    public AudioClip A;
    public AudioClip S;
    public AudioClip P;

    private string letter;
    public TMP_Text text;
    public TMP_Text scoreText;
    private int score;

    public GameObject[] confetti;
}
