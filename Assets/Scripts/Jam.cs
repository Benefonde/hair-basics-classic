using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jam : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("jammerUnlocked", 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tc.GetTrophy(5);
            SceneManager.LoadScene("ChallengeBeat");
            PlayerPrefs.SetInt("jammerUnlocked", 1);
            PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"JAMMER\" powerup. Use in modifier tab.");
        }
    }

    public TrophyCollectingScript tc;
}
