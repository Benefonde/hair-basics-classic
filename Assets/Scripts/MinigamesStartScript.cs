using System.Collections;
using System.Collections.Generic;
using FluidMidi;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MinigamesStartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        minigames[PlayerPrefs.GetInt("CurrentMinigame", 3)].SetActive(true);
        if (minigames[2].activeSelf)
        {
            avoidObstacles.SetActive(true);
        }
        chalkboard.transform.GetChild(PlayerPrefs.GetInt("CurrentMinigame", 3)).gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
        chalkboard.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Minigames");
    }

    public void Play()
    {
        GetComponent<AudioSource>().Play();
        chalkboard.GetComponent<Animator>().SetTrigger("game start");
        switch (PlayerPrefs.GetInt("CurrentMinigame", 3))
        {
            case 2: avoidObstacles.GetComponentInChildren<AvoidObstaclesPlayer>().enabled = true; break;
            case 3: minigames[3].GetComponent<TriviaMinigame>().enabled = true; break;
        }
    }

    public void Lose()
    {
        chalkboard.transform.GetChild(PlayerPrefs.GetInt("CurrentMinigame", 3)).gameObject.SetActive(false);
        chalkboard.GetComponent<Animator>().SetTrigger("game done");
        chalkboard.transform.GetChild(chalkboard.transform.childCount - 1).gameObject.SetActive(true);
        switch (PlayerPrefs.GetInt("CurrentMinigame", 3))
        {
            case 2:
                if (avoidObstacles.GetComponentInChildren<AvoidObstaclesPlayer>().score > PlayerPrefs.GetInt("obstaclesScore"))
                {
                    aftermath.text = $"Nice job! You got {Mathf.RoundToInt(avoidObstacles.GetComponentInChildren<AvoidObstaclesPlayer>().score)} score, a new record!";
                    PlayerPrefs.SetInt("obstaclesScore", Mathf.RoundToInt(avoidObstacles.GetComponentInChildren<AvoidObstaclesPlayer>().score));
                }
                else
                {
                    aftermath.text = $"You didn't get a high score this time... there's always next time though!";
                }
                break;
            case 3: 
                if (minigames[3].GetComponent<TriviaMinigame>().questionText.color == Color.green)
                {
                    aftermath.text = $"Nice job! You got {minigames[3].GetComponent<TriviaMinigame>().strike} strikes.";
                }
                else
                {
                    aftermath.text = $"Aww, you lost... Better luck next time!";
                }
                break;
        }
        // a
    }

    public void Pause()
    {
        if (!pause[0].activeSelf)
        {
            Time.timeScale = 0;
            pause[0].SetActive(true);
            AudioListener.pause = true;
            for (int i = 0; i < FindObjectsOfType<SongPlayer>().Length; i++)
            {
                FindObjectsOfType<SongPlayer>()[i].Pause();
            }
        }
        else
        {
            Time.timeScale = 1;
            pause[0].SetActive(false);
            AudioListener.pause = false;
            for (int i = 0; i < FindObjectsOfType<SongPlayer>().Length; i++)
            {
                FindObjectsOfType<SongPlayer>()[i].Resume();
            }
        }
    }

    public void HidePause()
    {
        if (!pauseHidden)
        {
            for (int i = 0; i < pause.Length; i++)
            {
                pause[i].SetActive(false);
            }
            pauseHidden = !pauseHidden;
        }
        else
        {
            for (int i = 0; i < pause.Length; i++)
            {
                pause[i].SetActive(true);
            }
            pauseHidden = !pauseHidden;
        }
    }

    public GameObject[] minigames;
    public GameObject avoidObstacles;

    public GameObject chalkboard;
    public TMP_Text aftermath;
    public GameObject[] pause;

    bool pauseHidden;
}
