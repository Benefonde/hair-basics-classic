using UnityEngine;
using UnityEngine.SceneManagement;

public class Script : MonoBehaviour
{
	public AudioSource audioDevice;

	public AudioClip itTworked;

	private bool played;

	public bool mikoUnlock;
	public bool paninis;

	float timmer = 2;

	public Animator timeMachine;

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
            else if (!paninis)
            {
				SceneManager.LoadScene("MainMenu");
            }
            else
            {
				if (audioDevice.clip != itTworked & played)
				{
					if (timmer > 0)
					{
						timmer -= Time.deltaTime;
						timeMachine.SetFloat("twork", 1);
						return;
					}
					played = false;
					timmer = 1;
					audioDevice.clip = itTworked;
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("It (t)worked! My time machine (t)worked!", audioDevice.clip.length, Color.cyan, transform);
					timeMachine.gameObject.GetComponent<CapsuleCollider>().enabled = true;
				}
				if (timeMachine.GetFloat("twork") == 1)
				{
					timeMachine.SetFloat("twork", 2);
				}
				return;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.name == "Player") & !played)
		{
			audioDevice.Play();
			if (paninis && audioDevice.clip != itTworked)
            {
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("Myyy time machine.", audioDevice.clip.length, Color.cyan, transform);
            }
			played = true;
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if ((other.name == "Player") & !played & audioDevice.clip == itTworked)
		{
			audioDevice.Play();
			played = true;
		}
	}
}
