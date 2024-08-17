using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public GameObject time;

	private void OnTriggerEnter(Collider other)
	{
		if ((gc.notebooks >= gc.maxNoteboos) & (other.tag == "Player"))
		{
			if (PlayerPrefs.GetInt("timer") == 1)
			{
				GameObject a = Instantiate(time);
				System.TimeSpan timee = System.TimeSpan.FromSeconds(gc.time);
				a.GetComponent<TimeScript>().t[0].text = string.Format("Time: {0:00}:{1:00}:{2:000}", timee.Minutes, timee.Seconds, timee.Milliseconds);
				a.GetComponent<TimeScript>().t[1].text = string.Format("Time: {0:00}:{1:00}:{2:000}", timee.Minutes, timee.Seconds, timee.Milliseconds);
			}
			if (gc.mode == "free" || gc.ModifierOn())
			{
				SceneManager.LoadScene("Anticheat");
				gc.tc.GetTrophy(22);
				return;
			}
			if (!gc.tc.babaGotPushed && gc.baba.activeSelf)
            {
				gc.tc.GetTrophy(4);
            }
			if (gc.yellowFaceOn == 1 && gc.yellowFace.activeSelf && gc.mode == "speedy")
            {
				gc.tc.GetTrophy(6);
            }
			if (!gc.tc.usedItem)
            {
				gc.tc.GetTrophy(7);
            }
			if (gc.mode == "speedy" && gc.time <= 150)
            {
				gc.tc.GetTrophy(9);
			}
			if (gc.pss.rank == "D" && gc.mode == "pizza")
            {
				gc.tc.GetTrophy(10);
            }
			if (gc.failedNotebooks >= gc.maxNoteboos && gc.mode == "story")
			{
				if (PlayerPrefs.GetInt("speedyUnlocked", 0) == 0)
                {
					PlayerPrefs.SetInt("speedyUnlocked", 1);
					SceneManager.LoadScene("Secret");
				}
                else
                {
					SceneManager.LoadScene("Secret");
				}
			}
			else if (gc.mode == "story")
			{
				SceneManager.LoadScene("Results");
			}
            else if (gc.mode == "speedy")
            {
				PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"SPEED BOOST\" powerup. Use in modifier tab.");
				PlayerPrefs.SetInt("speedyBeat", 1);
				SceneManager.LoadScene("ChallengeBeat");
            }
			else if (gc.mode == "miko")
            {
				PlayerPrefs.SetInt("mikoBeat", 1);
				PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"BLOCK PATH\" powerup. Use in modifier tab. Press T to use!");
				SceneManager.LoadScene("ChallengeBeat");
			}
			else if (gc.mode == "triple")
			{
				PlayerPrefs.SetInt("tripleBeat", 1);
				SceneManager.LoadScene("TripleBeat");
				PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"EXTRA STAMINA\" powerup. Use in modifier tab.");
			}
			else if (gc.mode == "pizza")
            {
				PlayerPrefs.SetString("pizzaRankNeww", gc.pss.rank);
				PlayerPrefs.SetInt("pizzaLapsNeww", gc.laps);
				PlayerPrefs.SetInt("pizzaScoreNeww", gc.pss.score);
				PlayerPrefs.SetInt("pizzaBeat", 1);
				if (gc.pss.rank != "P")
				{
					if (gc.pss.score > PlayerPrefs.GetInt("pizzaScoreBest"))
					{
						PlayerPrefs.SetString("pizzaRankBest", gc.pss.rank);
					}
				}
				else if (gc.pss.rank == "P")
				{
					PlayerPrefs.SetString("pizzaRankBest", gc.pss.rank);
					if (gc.laps >= 7)
                    {
						gc.tc.GetTrophy(2);
                    }
				}
				if (PlayerPrefs.GetInt("pizzaScoreBest") < gc.pss.score)
				{
					PlayerPrefs.SetInt("pizzaScoreBest", gc.pss.score);
				}
				if (gc.laps > PlayerPrefs.GetInt("pizzaLapsBest"))
				{
					PlayerPrefs.SetInt("pizzaLapsBest", gc.laps);
				}
				gc.FadeToWhite();
				gc.SetTime(0);
				SceneManager.LoadSceneAsync("Ranking");
			}
			else if (gc.mode == "stealthy")
			{
				SceneManager.LoadScene("ChallengeBeat");
				PlayerPrefs.SetInt("stealthyBeat", 1);
				PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"WALK THROUGH\" powerup. Use in modifier tab.");
			}
			else if (gc.mode == "classic")
			{
				SceneManager.LoadScene("ClassicEnding");
				PlayerPrefs.SetInt("classicBeat", 1);
			}
			else if (gc.mode == "zombie")
            {
				PlayerPrefs.SetInt("zombieBeat", 1);
				SceneManager.LoadScene("ChallengeBeat");
				PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you... uh, he didn't exactly give you anything. Have some completion percentage.");
			}
		}
	}
}
