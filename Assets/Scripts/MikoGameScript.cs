using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MikoGameScript : MonoBehaviour
{
	public GameControllerScript gc;
	public MikoScript mikoScript;

	public Vector3 playerPosition;

	public GameObject mathGame;

	public TMP_Text questionText;

	public TMP_Text questionText2;

	public TMP_Text questionText3;

	public string[] hintText;

	public PlayerScript playerScript;

	private void Start()
	{
		if (PlayerPrefs.GetInt("fps", 60) == 2763)
		{
			SceneManager.LoadScene("2763");
		}
		gc.ActivateLearningGame();
		if (gc.math == 0)
        {
			ExitGame();
		}
		int rng = Mathf.FloorToInt(Random.Range(0, hintText.Length - 1));
		questionText.text = hintText[rng];
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) || PlayerPrefs.GetInt("math", 1) == 0)
        {
			ExitGame();
        }
	}
	private void ExitGame()
	{
		if (mikoScript.isActiveAndEnabled)
		{
			mikoScript.GetAngry(1.65f);
			mikoScript.Hear(playerPosition, 20);
		}
		gc.DeactivateLearningGame(base.gameObject);
	}
}
