using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWonScript : MonoBehaviour
{
	public float delay;

	public bool unlock;

	private void Update()
	{
		delay -= Time.deltaTime;
		if (delay <= 0f)
		{
			if (PlayerPrefs.GetInt("speedyUnlocked") == 0 & unlock)
            {
				PlayerPrefs.SetInt("speedyUnlocked", 1);
				PlayerPrefs.SetString("bonusTextString", "Wowie Zowie! You just unlocked SPEEDY!");
				SceneManager.LoadScene("BeatBonusMode");
			}
            else
			{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}
}
