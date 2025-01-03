using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TriviaMinigame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        NewQuestion();
    }

    private void Update()
    {
        if (questionInProgress)
        {
            timer += Time.deltaTime;
            timerSlider.value = Mathf.FloorToInt(timer * 2);
            if (timer >= 15)
            {
                Answered(0);
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                NewQuestion();
            }
        }
    }

    void NewQuestion()
    {
        timer = 0;
        questionsAsked++;
        questionInProgress = true;
        question = NewQuestionNum(); 
        if (questionsAsked >= 9 || strike == 4)
        {
            return;
        }
        questionText.color = Color.white;
        questionsAlreadyAsked[question] = question;
        questionText.text = questions[question];
        answersText[0].text = answers1[question];
        answersText[1].text = answers2[question];
        answersText[2].text = answers3[question];
        answersText[3].text = answers4[question];
    }

    public void Answered(int answer)
    {
        if (!questionInProgress)
        {
            return;
        }
        questionInProgress = false;
        timer = 0;
        if (answer == rightAnswer[question])
        {
            aud.PlayOneShot(result[0]);
            questionText.color = Color.green;
        }
        else
        {
            aud.PlayOneShot(result[1]);
            questionText.color = Color.red;
            strikes[strike].SetActive(true);
            strike++;
        }
    }

    int NewQuestionNum()
    {
        System.Random random = new System.Random();

        List<int> validNumbers = Enumerable.Range(0, 9).Except(questionsAlreadyAsked).ToList();

        int randomIndex = random.Next(validNumbers.Count);
        if (questionsAsked >= 9 || strike == 4)
        {
            questionText.text = "You win!"; 
            if (strike == 4)
            {
                questionText.color = Color.red;
                questionText.text = "You lose...";
            }
            SceneManager.LoadScene("MainMenu");
        }
        return validNumbers[randomIndex];
    }

    public TMP_Text questionText;
    public TMP_Text[] answersText;
    int question;
    int strike;
    public Slider timerSlider;
    int questionsAsked;

    public AudioClip[] result; // 0 - right, 1 - wrong
    AudioSource aud;

    public string[] questions;

    public string[] answers1;
    public string[] answers2;
    public string[] answers3;
    public string[] answers4;

    public int[] rightAnswer;

    public GameObject[] strikes;

    int[] questionsAlreadyAsked = new int[10];

    float timer;
    bool questionInProgress;
}
