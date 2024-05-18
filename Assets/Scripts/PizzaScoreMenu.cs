using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PizzaScoreMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("pizzaLapsNew") > 0)
        {
            PlayerPrefs.SetInt("pizzaScoreBest", PlayerPrefs.GetInt("pizzaScoreNew"));
            PlayerPrefs.DeleteKey("pizzaScoreNew");
            PlayerPrefs.SetString("pizzaRankBest", PlayerPrefs.GetString("pizzaRankNew"));
            PlayerPrefs.DeleteKey("pizzaRankNew");
            PlayerPrefs.SetInt("pizzaLapsBest", PlayerPrefs.GetInt("pizzaLapsNew"));
            PlayerPrefs.DeleteKey("pizzaLapsNew");
        }
        else
        {
            rank = PlayerPrefs.GetString("pizzaRankBest", "D");
            score = PlayerPrefs.GetInt("pizzaScoreBest", 0);
            laps = PlayerPrefs.GetInt("pizzaLapsBest", 0);
        }

        text.text = $"Rank: {rank}, Score: {score}, Laps: {laps}";
    }

    public TMP_Text text;

    public string rank;
    public int score;
    public int laps;
}
