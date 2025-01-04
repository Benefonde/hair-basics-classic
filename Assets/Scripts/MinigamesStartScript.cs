using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesStartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        minigames[PlayerPrefs.GetInt("CurrentMinigame", 3)].SetActive(true);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public GameObject[] minigames;
}
