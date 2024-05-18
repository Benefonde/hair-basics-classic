using UnityEngine;

public class AmbienceScript : MonoBehaviour
{
	public Transform aiLocation;

	public AudioClip[] sounds;

	public AudioSource audioDevice;

	public void PlayAudio()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 49f));
		if (PlayerPrefs.GetString("CurrentMode") == "miko")
		{
			num = Mathf.RoundToInt(Random.Range(0f, 19f));
		}
		if (PlayerPrefs.GetString("CurrentMode", "story") == "stealthy")
        {
			num = 0;
        }
		if (!audioDevice.isPlaying && num == 0)
		{
			base.transform.position = aiLocation.position;
			if (PlayerPrefs.GetString("CurrentMode") == "miko")
			{
				audioDevice.PlayOneShot(sounds[1]);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Keyboard sounds*", sounds[1].length, Color.gray, transform);
				return;
			}
			audioDevice.PlayOneShot(sounds[0]);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Noise sounds*", sounds[0].length, Color.yellow, transform);
		}
	}
}
