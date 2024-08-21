using UnityEngine;
using TMPro;

public class CompletionScript : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (requirements[i] == "pizzaRankBest")
            {
                if (PlayerPrefs.GetString(requirements[i]) == "P")
                {
                    percent += amountToGive;
                    print($"thje player has that requirement (number {i})! lets give him {amountToGive} on that");
                }
            }
            else if (requirements[i] == "HighBooks" || requirements[i] == "pizzaLapsBest")
            {
                if (PlayerPrefs.GetInt(requirements[i]) >= valueInt[i])
                {
                    percent += amountToGive;
                    print($"thje player has that requirement (number {i})! lets give him {amountToGive} on that");
                }
            }
            else
            {
                if (PlayerPrefs.GetInt(requirements[i]) == valueInt[i])
                {
                    percent += amountToGive;
                    print($"thje player has that requirement (number {i})! lets give him {amountToGive} on that");
                }
            }
        }
        print(percent);
        percent = Mathf.CeilToInt(percent - 0.1f);
        print(percent);
        percentText.text = $"{percent}%";
        PlayerPrefs.SetInt("completion", Mathf.CeilToInt(percent));
        if (percent >= 100)
        {
            FindObjectOfType<TrophyCollectingScript>().GetTrophy(24);
        }
        TheWorstCodeEverSoThisCanActuallyWorkLmao();
    }

    public bool DoesBroHaveIt(int a)
    {
        if (requirements[a] == "pizzaRankBest")
        {
            if (PlayerPrefs.GetString(requirements[a]) == "P")
            {
                return true;
            }
        }
        else if (requirements[a] == "HighBooks" || requirements[a] == "pizzaLapsBest")
        {
            if (PlayerPrefs.GetInt(requirements[a]) >= valueInt[a])
            {
                return true;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt(requirements[a]) == valueInt[a])
            {
                return true;
            }
        }
        return false;
    }

    void TheWorstCodeEverSoThisCanActuallyWorkLmao()
    {
        if (percent == 0)
        {
            judgementText.text = judgements[0];
        }
        else if (percent <= 9)
        {
            judgementText.text = judgements[1];
        }
        else if (percent <= 18)
        {
            judgementText.text = judgements[2];
        }
        else if (percent <= 27)
        {
            judgementText.text = judgements[3];
        }
        else if (percent <= 36)
        {
            judgementText.text = judgements[4];
        }
        else if (percent <= 46)
        {
            judgementText.text = judgements[5];
        }
        else if (percent <= 55)
        {
            judgementText.text = judgements[6];
        }
        else if (percent <= 64)
        {
            judgementText.text = judgements[7];
        }
        else if (percent <= 73)
        {
            judgementText.text = judgements[8];
        }
        else if (percent <= 82)
        {
            judgementText.text = judgements[9];
        }
        else if (percent <= 91)
        {
            judgementText.text = judgements[10];
        }
        else if (percent <= 99)
        {
            judgementText.text = judgements[11];
        }
        if (percent == 100)
        {
            judgementText.text = judgements[12];
        }
    }

    public float amountToGive;

    public float percent;
    public string[] requirements;
    public int[] valueInt;
    public string[] valueString;

    private int[] completionPercentages = { 9, 18, 27, 36, 46, 55, 64, 73, 82, 91, 99 };

    public TMP_Text percentText;
    public TMP_Text judgementText;
    public string[] judgements;
}
