using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrophyCollectingScript : MonoBehaviour
{
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
        if (windowCleanAmount == 2763)
        {
            GetTrophy(15);
        }
        if (windowCleanAmount == 282828)
        {
            GetTrophy(29);
        }
        if (PlayerPrefs.GetInt("algerBeat", 0) == 1)
        {
            GetTrophy(21);
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
            if (gc.laps >= 50)
            {
                GetTrophy(39);
            }
        }
        if (esteEaten >= 1 && zestyEaten >= 4 && !dontCheckAga[11])
        {
            GetTrophy(11);
        }
        if (PlayerPrefs.GetInt("pSecretFound", 0) == 1)
        {
            GetTrophy(17);
        }
        if (devinPipeHit >= 5 && !gc.camScript.FuckingDead)
        {
            GetTrophy(30);
        }
        if (pizzafaceTime >= 60)
        {
            GetTrophy(31);
        }
        if (promplyTime >= 90)
        {
            GetTrophy(32);
        }
        if (hideTime >= 120)
        {
            GetTrophy(38);
        }
    }

    public void GetTrophy(int i)
    {
        if (gc == null)
        {
            if (!dontCheckAga[i])
            {
                Image a = Instantiate(gotTrophy).GetComponentInChildren<Image>();
                a.sprite = trophies[i];
                PlayerPrefs.SetInt(trophyName[i], 1);
                dontCheckAga[i] = true;
            }
            return;
        }
        if ((!dontCheckAga[i] && !gc.ModifierOn()) || !dontCheckAga[i] && gc.ModifierOn() && i == 22)
        {
            Image a = Instantiate(gotTrophy).GetComponentInChildren<Image>();
            a.sprite = trophies[i];
            PlayerPrefs.SetInt(trophyName[i], 1);
            dontCheckAga[i] = true;
        }
    }

    public void QuarterCheck()
    {
        quarterCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (gc.item[i] == 5)
            {
                quarterCount++;
            }
            if (quarterCount == 4)
            {
                GetTrophy(33);
            }
        }
    }

    public GameControllerScript gc;

    public GameObject gotTrophy;

    public Sprite[] trophies;

    public string[] trophyName;

    private bool[] dontCheckAga = new bool[40];

    public int zestyEaten;
    public int esteEaten;

    public int devinPipeHit;
    public float pizzafaceTime;
    public float promplyTime;
    public float hideTime;

    public bool babaGotPushed;
    public bool usedItem;
    public bool onlyWooden;
    public bool ruleBreak;

    public int collectedToppings;
    public int collectToppingsNeeded;
    int quarterCount;

    public int windowsCleaned;
    [SerializeField]
    private int windowCleanAmount;
}
