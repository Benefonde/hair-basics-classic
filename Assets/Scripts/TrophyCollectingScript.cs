using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrophyCollectingScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gc = GetComponent<GameControllerScript>();
        for (int i = 0; i < trophyName.Length; i++)
        {
            if (PlayerPrefs.GetInt(trophyName[i]) == 1)
            {
                dontCheckAga[i] = true;
            }
        }

        if (PlayerPrefs.GetInt("jammerUnlocked") == 1 && !dontCheckAga[5])
        {
            GetTrophy(5);
        }
        if (PlayerPrefs.GetInt("pizzaLapsBest") >= 7 && PlayerPrefs.GetString("pizzaRankNew") == "P" && !dontCheckAga[2])
        {
            GetTrophy(2);
        }
        if ((PlayerPrefs.GetInt("piecesFound", 0) == 4 || PlayerPrefs.GetInt("mikoUnlocked") == 1) && SceneManager.GetActiveScene().name == "EsteSecret")
        {
            GetTrophy(14);
        }
        if (SceneManager.GetActiveScene().name == "2763")
        {
            GetTrophy(15);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gc != null)
        {
            if (gc.notebooks == gc.maxNoteboos && !dontCheckAga[0])
            {
                GetTrophy(0);
            }
            if (windowsCleaned >= windowCleanAmount && gc.mode == "story" && !dontCheckAga[1])
            {
                GetTrophy(1);
            }
            if (collectedToppings >= collectToppingsNeeded && gc.mode == "pizza" && !dontCheckAga[3])
            {
                GetTrophy(3);
            }
            if (gc.secretsFound >= 3 && !dontCheckAga[8])
            {
                GetTrophy(8);
            }
            if (gc.notebooks < 0 && !dontCheckAga[13])
            {
                GetTrophy(13);
            }
        }
        if (esteEaten >= 1 && zestyEaten >= 3 && !dontCheckAga[11])
        {
            GetTrophy(11);
        }
    }

    public void GetTrophy(int i)
    {
        if (!dontCheckAga[i] && !gc.ModifierOn())
        {
            Image a = Instantiate(gotTrophy).GetComponentInChildren<Image>();
            a.sprite = trophies[i];
            PlayerPrefs.SetInt(trophyName[i], 1);
            dontCheckAga[i] = true;
        }
    }

    public GameControllerScript gc;

    public GameObject gotTrophy;

    public Sprite[] trophies;

    public string[] trophyName;

    private bool[] dontCheckAga = new bool[16];

    public int zestyEaten;
    public int esteEaten;

    public bool babaGotPushed;
    public bool usedItem;

    public int collectedToppings;
    public int collectToppingsNeeded;

    public int windowsCleaned;
    [SerializeField]
    private int windowCleanAmount;
}
