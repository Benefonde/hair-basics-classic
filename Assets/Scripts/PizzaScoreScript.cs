using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PizzaScoreScript : MonoBehaviour
{
    void Start()
    {
        score = 0;
        UpdateRankSlider();
    }

    void RunUpdateScore()
    {
        UpdateRankSlider();
    }

    private void Update()
    {
        if (score >= aRankScore)
        {
            toppings[0].SetActive(true);
            toppings[1].SetActive(true);
            toppings[2].SetActive(true);
            toppings[3].SetActive(true);
        }
        else if (score >= aRankScore && score < sRankScore)
        {
            rank = "A";
            toppings[0].SetActive(false);
            toppings[1].SetActive(true);
            toppings[2].SetActive(true);
            toppings[3].SetActive(true);
        }
        else if (score >= cRankScore)
        {
            rank = "B";
            toppings[0].SetActive(false);
            toppings[1].SetActive(false);
            toppings[2].SetActive(true);
            toppings[3].SetActive(true);
        }
        else if (score >= dRankScore)
        {
            rank = "C";
            toppings[0].SetActive(false);
            toppings[1].SetActive(false);
            toppings[2].SetActive(false);
            toppings[3].SetActive(true);
        }
        else
        {
            rank = "D";
            toppings[0].SetActive(false);
            toppings[1].SetActive(false);
            toppings[2].SetActive(false);
            toppings[3].SetActive(false);
        }

        if (score < 0)
        {
            score = 0;
        }
        scoreText[0].text = score.ToString();
        scoreText[1].text = score.ToString();

        if (!ranUpdateScore)
        {
            RunUpdateScore();
            ranUpdateScore = true;
        }

        if (!lostScore && gc.secretsFound == 3 && gc.laps > 1)
        {
            bool a;
            if (!canP)
            {
                a = true;
            }
            else
            {
                a = false;
            }
            canP = true;
            if (a)
            {
                ranUpdateScore = false;
            }
        }
        else
        {
            canP = false;
        }
    }

    public void UpdateRankSlider()
    {
        int drank = dRankScore;
        int crank = cRankScore;
        int brank = bRankScore;
        int arank = aRankScore;
        int srank = sRankScore;

        if (score < drank)
        {
            rankSlider.maxValue = drank;
            rankSlider.value = score;
            baseRank.sprite = ranks[0];
            sliderBg.color = new Color32(48, 80, 120, 255);
            sliderFill.color = new Color32(96, 208, 72, 255);
        }
        else if (score >= drank && score < crank)
        {
            rankSlider.maxValue = crank - drank;
            rankSlider.value = score - drank;
            baseRank.sprite = ranks[1];
            sliderBg.color = new Color32(96, 208, 72, 255);
            sliderFill.color = new Color32(48, 168, 248, 255);
        }
        else if (score >= crank && score < brank)
        {
            rankSlider.maxValue = brank - crank;
            rankSlider.value = score - crank;
            baseRank.sprite = ranks[2];
            sliderBg.color = new Color32(48, 168, 248, 255);
            sliderFill.color = new Color32(248, 0, 0, 255);
        }
        else if (score >= brank && score < srank)
        {
            rankSlider.maxValue = srank - brank;
            rankSlider.value = score - brank;
            baseRank.sprite = ranks[3];
            sliderBg.color = new Color32(248, 0, 0, 255);
            if (!canP)
            {
                sliderFill.color = new Color32(244, 144, 0, 255);
            }
            else
            {
                sliderFill.color = new Color32(152, 80, 248, 255);
            }
        }
        else if (score > srank)
        {
            rankSlider.maxValue = srank * 1.95f;
            rankSlider.value = score - srank;
            if (!canP)
            {
                if (rankSlider.value >= rankSlider.maxValue - 150)
                {
                    rankSlider.maxValue *= 1.25f;
                    gc.tc.GetTrophy(12);
                }
                baseRank.sprite = ranks[4];
                rank = "S";
                sliderBg.color = new Color32(244, 144, 0, 255);
                sliderFill.color = new Color32(255, 184, 0, 255);
            }
            else
            {
                if (rankSlider.value >= rankSlider.maxValue - 150)
                {
                    rankSlider.maxValue *= 1.25f;
                    gc.tc.GetTrophy(12);
                }
                baseRank.sprite = ranks[5]; 
                rank = "P";
                sliderBg.color = new Color32(152, 80, 248, 255);
                sliderFill.color = new Color32(192, 120, 255, 255);
            }
        }
    }

    public void AddPoints(int points, float textDieTime)
    {
        StopAllCoroutines();
        score += points;
        if (points > -1)
        {
            pointAnim.color = Color.cyan;
        }
        else
        {
            pointAnim.color = Color.red;
            pointAnim.text = points.ToString();
        }
        StartCoroutine(PointAnimThingy(textDieTime));
        UpdateRankSlider();
        if (points < -5)
        {
            lostScore = true;
        }
    }

    IEnumerator PointAnimThingy(float timer)
    {
        yield return new WaitForSeconds(timer);
        pointAnim.text = string.Empty;
    }

    public GameObject[] toppings;
    public int score;
    public TMP_Text pointAnim;
    public TMP_Text[] scoreText;

    public GameControllerScript gc;

    public Slider rankSlider;
    public Image sliderBg;
    public Image sliderFill;
    public Image baseRank;
    public Sprite[] ranks;

    public bool lostScore;

    public bool canP;

    public int sRankScore;
    public int aRankScore;
    public int bRankScore;
    public int cRankScore;
    public int dRankScore;

    public bool ranUpdateScore;

    public string rank;
}
