using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MathGameScript : MonoBehaviour
{
	public GameControllerScript gc;

	public BaldiScript baldiScript;
	public MikoScript mikoScript;
	public AlgerScript algerScript;

	public Vector3 playerPosition;

	public GameObject mathGame;

	public RawImage[] results = new RawImage[3];

	public Texture correct;

	public Texture incorrect;

	public TMP_InputField playerAnswer;

	public TMP_Text questionText;

	public TMP_Text questionText2;

	public TMP_Text questionText3;

	public Animator baldiFeed;

	public Transform baldiFeedTransform;

	public AudioClip bal_plus;

	public AudioClip bal_minus;

	public AudioClip bal_times;

	public AudioClip bal_divided;

	public AudioClip bal_equals;

	public AudioClip bal_howto;

	public AudioClip bal_intro;

	public AudioClip bal_screech;

	public AudioClip[] bal_praises = new AudioClip[6];

	public AudioClip[] bal_problems = new AudioClip[3];

	public Button firstButton;

	private float endDelay;

	private int problem;

	private int audioInQueue;

	private float num1;

	private float num2;

	private float num3;

	private float num4;

	private int sign;

	private float solution;

	private string[] hintText = new string[2] { "I GET ANGRIER FOR EVERY JOKE YOU LAUGH AT", "I HEAR EVERY BALL YOU DUPLICATE" };

	private string[] endlessHintText = new string[2] { "I will ball you.", "Keep up the good work or face the wrath of my ball." };

	private bool questionInProgress;

	private bool impossibleMode;

	private int problemsWrong;

	private AudioClip[] audioQueue = new AudioClip[20];

	public AudioSource baldiAudio;

	public PlayerScript playerScript;

	public AudioClip YCTP_Stupid;

	public AudioSource audioDevice;

	private void Start()
	{
		if (PlayerPrefs.GetInt("fps", 60) == 2763)
        {
			SceneManager.LoadScene("2763");
        }
		gc.ActivateLearningGame();
		if (gc.math == 0)
		{
			if (gc.notebooks == 1)
			{
				gc.DeactivateLearningGame(gameObject);
			}
			if (gc.mode != "endless")
			{
				if (gc.notebooks > 1)
				{
					problemsWrong++;
					baldiScript.GetAngry(0.6f);
					if (!gc.spoopMode)
					{
						gc.ActivateSpoopMode();
					}
					gc.DeactivateLearningGame(gameObject);
				}
			}
			else
			{
				if (gc.notebooks < 2)
				{
					gc.DeactivateLearningGame(gameObject);
				}
				if (gc.notebooks == 2)
				{
					problemsWrong++;
					baldiScript.GetAngry(0.7f);
					if (!gc.spoopMode)
					{
						gc.ActivateSpoopMode();
					}
					gc.DeactivateLearningGame(gameObject);
				}
				if (gc.notebooks > 2)
				{
					baldiScript.GetAngry(-1);
					gc.DeactivateLearningGame(gameObject);
				}
			}
		}
		if (gc.notebooks == 1 && gc.math == 1)
		{
			string[] strings = { "Now it's time for a cool subject, which is math", "So basically put goo answer in box for big prize and answer them correctly or I will hit you with my ball" };
			float[] floats = { bal_intro.length + 0.5f, bal_howto.length };
			Color[] colors = { Color.cyan, Color.cyan };
			QueueAudio(bal_intro);
			QueueAudio(bal_howto);
			if (!gc.spoopMode)
			{
				FindObjectOfType<SubtitleManager>().AddChained2DSubtitle(strings, floats, colors);
			}
		}
		NewProblem();
		if (gc.spoopMode)
		{
			baldiFeedTransform.position = new Vector3(-1000f, -1000f, 0f);
		}
		if (gc.mode == "triple")
        {
			baldiScript.GetAngry(2);
			mikoScript.GetAngry(2);
			gc.DeactivateLearningGame(gameObject);
        }
	}

	private void Update()
	{
		if (!baldiAudio.isPlaying)
		{
			if ((audioInQueue > 0) & !gc.spoopMode)
			{
				PlayQueue();
			}
			baldiFeed.SetBool("talking", value: false);
		}
		else
		{
			baldiFeed.SetBool("talking", value: true);
		}
		if ((Input.GetKeyDown("return") || Input.GetKeyDown("enter")) & questionInProgress)
		{
			questionInProgress = false;
			CheckAnswer();
		}
		if (problem > 3)
		{
			endDelay -= 1f * Time.unscaledDeltaTime;
			if (endDelay <= 0f)
			{
				GC.Collect();
				ExitGame();
			}
		}
	}

	private void NewProblem()
	{
		playerAnswer.text = string.Empty;
		problem++;
		playerAnswer.ActivateInputField();
		if (problem <= 3)
		{
			QueueAudio(bal_problems[problem - 1]);
			if (((gc.mode == "story" || gc.mode == "pizza" || gc.mode == "classic") & (problem <= 2 || gc.notebooks <= 1)) || ((gc.mode == "endless") & (problem <= 2 || gc.notebooks != 2)))
			{
				num1 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 12f));
				num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 12f));
				num3 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 120f));
				num4 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 130f));
				int easyMath = PlayerPrefs.GetInt("easyMath");
				if (easyMath == 1)
				{
					sign = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				}
				else
				{
					sign = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
				}
				if (sign == 0)
				{
					solution = num1 + this.num2;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "+" + this.num2 + "=";
				}
				else if (sign == 1)
				{
					solution = num1 - this.num2;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "-" + this.num2 + "=";
				}
				else if (sign == 2)
				{
					solution = num1 * this.num2;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "x" + this.num2 + "=";
				}
				else if (sign == 3)
				{
					solution = num1 * this.num2 + num3 - num4;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "x" + this.num2 + "+" + num3 + "-" + num4 + "=";
				}
				else if (sign == 4)
				{
					solution = num1 * this.num2 - num3 + num4 - num3;
					questionText.text = "SOLVE MATH Q" + problem + ": \n \n" + num1 + "x" + this.num2 + "-" + num3 + "+" + num4 + "-" + num3 + "=";
				}
			}
			else
			{
				impossibleMode = true;
				num1 = UnityEngine.Random.Range(1f, 9999f);
				this.num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				QueueAudio(bal_screech);
				if (sign == 0)
				{
					questionText.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + this.num2 + "X" + num3 + "=";
				}
				else if (sign == 1)
				{
					questionText.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + this.num2 + ")+" + num3 + "=";
				}
				num1 = UnityEngine.Random.Range(1f, 9999f);
				this.num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				if (sign == 0)
				{
					questionText2.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + this.num2 + "X" + num3 + "=";
				}
				else if (sign == 1)
				{
					questionText2.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + this.num2 + ")+" + num3 + "=";
				}
				num1 = UnityEngine.Random.Range(1f, 9999f);
				num2 = UnityEngine.Random.Range(1f, 9999f);
				num3 = UnityEngine.Random.Range(1f, 9999f);
				sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				if (sign == 0)
				{
					questionText3.text = "SOLVE MATH Q" + problem + ": \n" + num1 + "+(" + this.num2 + "X" + num3 + "=";
				}
				else if (sign == 1)
				{
					questionText3.text = "SOLVE MATH Q" + problem + ": \n (" + num1 + "/" + this.num2 + ")+" + num3 + "=";
				}
			}
			questionInProgress = true;
			return;
		}
		endDelay = 2.5f;
		if (!gc.spoopMode)
		{
			questionText.text = "WOW! YOU BREATHE AIR!";
		}
		else if ((gc.mode == "endless") & (problemsWrong <= 0))
		{
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
			questionText.text = endlessHintText[num];
		}
		else if ((gc.mode == "story" || gc.mode == "classic") & (problemsWrong >= 3))
		{
			questionText.text = "Stupid idiot stupid";
			questionText2.text = string.Empty;
			gc.failedNotebooks++;
			questionText3.text = (gc.failedNotebooks) + "/" + gc.notebooks + " are all wrong";
			if (baldiScript.isActiveAndEnabled)
			{
				baldiScript.Hear(playerPosition, 7f);
			}
			audioDevice.PlayOneShot(YCTP_Stupid);
			FindObjectOfType<SubtitleManager>().Add2DSubtitle("Stupid idiot stupid", YCTP_Stupid.length, Color.white);
		}
		else
		{
			int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
			questionText.text = hintText[num2];
			questionText2.text = string.Empty;
			questionText3.text = string.Empty;
		}
	}

	public void OKButton()
	{
		CheckAnswer();
		if (gc.notebooks == 1)
		{
			FindObjectOfType<SubtitleManager>().StopChainedSubtitles();
		}
	}

	public void CheckAnswer()
	{
		if (playerAnswer.text == "3872643")
		{
			StartCoroutine(CheatText("What have you done!?"));
			PlayerPrefs.SetInt("did3872643", 1);
			SceneManager.LoadSceneAsync("EsteSecret");
		}
		else if (playerAnswer.text == "84775")
		{
			StartCoroutine(CheatText("balls"));
			problem = 4;
			gc.notebooks = -2763;
			gc.finaleMode = true;
			gc.timer.timeLeft = 1;
			this.baldiScript.baldiSpeedScale += 3f;
			playerScript.runSpeed += 40;
			playerScript.walkSpeed = 2;
			playerScript.stamina = 20;
		}
		if (problem > 3)
		{
			return;
		}
		if ((playerAnswer.text == solution.ToString()) & !impossibleMode)
		{
			SubtitleManager sm = FindObjectOfType<SubtitleManager>();
			results[problem - 1].texture = correct;
			baldiAudio.Stop();
			ClearAudioQueue();
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 5f));
			QueueAudio(bal_praises[num]);
			if (!gc.spoopMode)
			{
				switch (num)
				{
					case 0: sm.Add2DSubtitle("Woah awesome", bal_praises[num].length, Color.cyan); break;
					case 1: sm.Add2DSubtitle("Cool job", bal_praises[num].length, Color.cyan); break;
					case 2: sm.Add2DSubtitle("Woah, very smart", bal_praises[num].length, Color.cyan); break;
					case 3: sm.Add2DSubtitle("Maybe I won't ball you!", bal_praises[num].length, Color.cyan); break;
					case 4: sm.Add2DSubtitle("the j", bal_praises[num].length, Color.cyan); break;
					case 5: sm.Add2DSubtitle("Awe some", bal_praises[num].length, Color.cyan); break;
				}
			}
			NewProblem();
			return;
		}
		problemsWrong++;
		results[problem - 1].texture = incorrect;
		if (!gc.spoopMode)
		{
			baldiFeed.SetTrigger("angry");
			gc.ActivateSpoopMode();
		}
		if (gc.mode == "story" || gc.mode == "pizza" || gc.mode == "classic")
		{
			if (problem == 3)
			{
				if (gc.mode != "classic")
				{
					mikoScript.GetAngry(0.5f);
				}
				baldiScript.GetAngry(0.6f);
			}
			else
			{
				if (gc.mode != "classic")
				{
					mikoScript.GetAngry(0.15f);
				}
				baldiScript.GetTempAngry(0.1f);
				baldiScript.GetAngry(0.5f);
			}
		}
		else
		{
			if (gc.mode != "classic")
			{
				mikoScript.GetAngry(0.5f);
			}
			baldiScript.GetAngry(0.5f);
		}
		ClearAudioQueue();
		baldiAudio.Stop();
		NewProblem();
	}

	private void QueueAudio(AudioClip sound)
	{
		audioQueue[audioInQueue] = sound;
		audioInQueue++;
	}

	private void PlayQueue()
	{
		if (problem < 4) 
		{
			if (!gc.spoopMode && gc.math == 1 && audioQueue[0] == bal_problems[problem - 1])
			{
				SubtitleManager sm = FindObjectOfType<SubtitleManager>();
				switch (problem)
				{
					case 1: sm.Add2DSubtitle("Problem first", bal_problems[0].length, Color.cyan); break;
					case 2: sm.Add2DSubtitle("Problem two", bal_problems[1].length, Color.cyan); break;
					case 3: sm.Add2DSubtitle("Problem last", bal_problems[2].length, Color.cyan); break;
				}
			}
		}
		baldiAudio.PlayOneShot(audioQueue[0]);
		UnqueueAudio();
	}

	private void UnqueueAudio()
	{
		for (int i = 1; i < audioInQueue; i++)
		{
			audioQueue[i - 1] = audioQueue[i];
		}
		audioInQueue--;
	}

	private void ClearAudioQueue()
	{
		audioInQueue = 0;
	}

	private void ExitGame()
	{
		if ((problemsWrong <= 0) & (gc.mode == "endless"))
		{
			baldiScript.GetAngry(-1f);
		}
		gc.DeactivateLearningGame(base.gameObject);
	}

	public void ButtonPress(int value)
	{
		if (value >= 0 && value <= 9)
		{
			playerAnswer.text += value;
		}
		else if (value == -1)
		{
			playerAnswer.text += "-";
		}
		else
		{
			playerAnswer.text = string.Empty;
		}
	}

	private IEnumerator CheatText(string text)
	{
		while (true)
		{
			questionText.text = text;
			questionText2.text = string.Empty;
			questionText3.text = string.Empty;
			yield return new WaitForEndOfFrame();
		}
	}
}
