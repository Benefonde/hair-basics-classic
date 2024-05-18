using UnityEngine;
using UnityEngine.SceneManagement;

public class Script : MonoBehaviour
{
	public AudioSource audioDevice;

	private bool played;

	public bool mikoUnlock;

	private void Update()
	{
		if (!audioDevice.isPlaying & played)
		{
			if (mikoUnlock == true)
            {
				PlayerPrefs.SetString("bonusTextString", "Wowie Zowie! You just unlocked Miko Mode!");
				PlayerPrefs.SetInt("mikoUnlocked", 1);
				SceneManager.LoadScene("BeatBonusMode");
			}
            else
            {
				SceneManager.LoadScene("MainMenu");
            }
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.name == "Player") & !played)
		{
			audioDevice.Play();
			played = true;
		}
	}
}
