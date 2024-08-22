using UnityEngine.SceneManagement;
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

    public void SendToRank()
    {
        PlayerPrefs.SetInt("completion", Mathf.CeilToInt(percent));
        SceneManager.LoadScene("Completion");
    }

    public float amountToGive;

    public float percent;
    public string[] requirements;
    public int[] valueInt;
    public string[] valueString;

    public TMP_Text percentText;
    public string[] judgements;
}
