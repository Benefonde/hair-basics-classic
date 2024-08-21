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
		Zombie = 13
	}

	public Mode currentMode;

	public void StartGame()
	{
		if (currentMode == Mode.Story)
		{
			PlayerPrefs.SetString("CurrentMode", "story");
		}
		else if (currentMode == Mode.Endless)
		{
			PlayerPrefs.SetString("CurrentMode", "endless");
		}
        else if (currentMode == Mode.Speedy)
        {
			PlayerPrefs.SetString("CurrentMode", "speedy");
		}
		else if (currentMode == Mode.Miko)
		{
			PlayerPrefs.SetString("CurrentMode", "miko");
		}
		else if (currentMode == Mode.Triple)
		{
			PlayerPrefs.SetString("CurrentMode", "triple");
		}
		else if (currentMode == Mode.Pizza)
		{
			PlayerPrefs.SetString("CurrentMode", "pizza");
		}
		else if (currentMode == Mode.FreeRun)
		{
			PlayerPrefs.SetString("CurrentMode", "free");
		}
		else if (currentMode == Mode.Alger)
		{
			PlayerPrefs.SetString("CurrentMode", "alger");
		}
		else if (currentMode == Mode.Dark)
		{
			PlayerPrefs.SetString("CurrentMode", "dark");
		}
		else if (currentMode == Mode.Stealthy)
		{
			PlayerPrefs.SetString("CurrentMode", "stealthy");
		}
		else if (currentMode == Mode.Chaos)
		{
			PlayerPrefs.SetString("CurrentMode", "chaos");
		}
		else if (currentMode == Mode.Classic)
		{
			PlayerPrefs.SetString("CurrentMode", "classic");
		}
		else if (currentMode == Mode.Zombie)
		{
			PlayerPrefs.SetString("CurrentMode", "zombie");
		}
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
