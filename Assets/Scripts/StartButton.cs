using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
	public enum Mode
	{
		Story = 0,
		Endless = 1,
		Speedy = 2,
		Miko = 3,
		Triple = 4,
		FJOTMY = 5,
		Pizza = 6,
		FreeRun = 7,
		Alger = 8,
		Dark = 9,
		Stealthy = 10,
		Chaos = 11,
		Classic = 12,
		Zombie = 13,
		Panino = 14
	}

	public Mode currentMode;

	public void StartGame()
	{
		PlayerPrefs.SetString("CurrentMode", currentMode.ToString().ToLower());
		print(currentMode.ToString().ToLower());
		if (currentMode != Mode.Classic)
		{
			SceneManager.LoadSceneAsync("School");
		}
        else
        {
			SceneManager.LoadSceneAsync("thing");
		}
	}
}
