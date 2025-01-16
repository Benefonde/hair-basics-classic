using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonMinigames : MonoBehaviour
{
	public enum Mode
	{
		Volleyball = 0,
		TowerDefense = 1,
		AvoidObstacles = 2,
		Trivia = 3,
		Missing = 4
	}

	public Mode currentMode;

	public void StartGame()
	{
		PlayerPrefs.SetInt("CurrentMinigame", (int)currentMode);
		print(((int)currentMode).ToString());
		SceneManager.LoadScene("Minigames");
	}
}
