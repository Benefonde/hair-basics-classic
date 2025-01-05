using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public void Restart()
    {
        SceneManager.LoadScene("Minigames");
    }

    public void Pause()
    {
        if (!pause[0].activeSelf)
        {
            Time.timeScale = 0;
            pause[0].SetActive(true);
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            pause[0].SetActive(false);
            AudioListener.pause = false;
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
    public GameObject[] pause;

    bool pauseHidden;
}
