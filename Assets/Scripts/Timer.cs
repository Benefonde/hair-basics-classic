using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public float timeLeft = 99f;

	public TMP_Text text;

	public bool isActivated;

	public BaldiScript baldiScript;

	public Transform player;

	public PlayerScript playerScript;

	public GameObject pizzaface;
	public GameObject pizzaTimeSlider;

	public GameControllerScript gc;

	public TMP_FontAsset ptFont;
	public TextAlignmentOptions a;

	public Slider pizzaTimeTimer;
	public Animator pissface;


	private void Start()
    {
        if (gc.mode == "pizza")
        {
			text.color = Color.white;
			RectTransform rt = GetComponent<RectTransform>();
			transform.SetParent(pizzaTimeSlider.transform);
			rt.localScale = new Vector3(1, 1, 1);
			rt.anchoredPosition = new Vector3(0, -5, 0);
			rt.anchorMin = new Vector2(0.5f, 0.5f);
			rt.anchorMax = new Vector2(0.5f, 0.5f);
			text.font = ptFont;
			text.alignment = a;
			text.fontSize = 52;
		}
    }

    private void Update()
	{
		if (isActivated)
		{
			timeLeft -= Time.deltaTime;
			if (gc.mode != "pizza")
            {
				text.text = "Escape from Hair BASICS! " + Mathf.Ceil(timeLeft) + " seconds left!";
				if (timeLeft < 0f)
				{
					text.text = "Uh oh.. 0 seconds left.";
					if (this.gc.mode == "speedy")
					{
						this.playerScript.gameOver = true;
					}
					else
					{
						baldiScript.Hear(player.position, 8f);
						baldiScript.baldiWait = 0.1f;
						baldiScript.speed = 50f;
						playerScript.walkSpeed = 12f;
						playerScript.runSpeed = 18f;
						isActivated = false;
					}
				}
			}
			if (gc.mode == "pizza")
			{
				if (timeLeft > pizzaTimeTimer.maxValue)
				{
					pizzaTimeTimer.maxValue = Mathf.Ceil(timeLeft);
				}
				pizzaTimeTimer.value = pizzaTimeTimer.maxValue - timeLeft;
				var ts = TimeSpan.FromSeconds(timeLeft + 0.5f);
				text.text = string.Format("{0:0}:{1:00}", ts.Minutes, ts.Seconds);
				if (timeLeft < 0f)
                {
                    text.text = "0:00";
					pissface.SetTrigger("Wake Up");
					pizzaface.transform.position = player.position;
					pizzaface.SetActive(true);
					isActivated = false;
					Invoke("PissTimerGoDown", 2);
				}
			}
		}
	}

	void PissTimerGoDown()
    {
		pizzaTimeSlider.GetComponent<Animator>().SetBool("up", false);
	}
}
