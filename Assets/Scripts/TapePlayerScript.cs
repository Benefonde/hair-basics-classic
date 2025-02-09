using UnityEngine;

public class TapePlayerScript : MonoBehaviour
{
	public Sprite closedSprite;

	public SpriteRenderer sprite;

	public BaldiScript baldi;
	public MikoScript miko;
	public AlgerScript alger;

	private AudioSource audioDevice;

	public bool blockSpriteChange;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		if (base.gameObject.name == "PayPhone")
		{
			blockSpriteChange = true;
		}
	}

	private void Update()
	{
		if (audioDevice.isPlaying & (Time.timeScale == 0f))
		{
			audioDevice.Pause();
		}
		else if ((Time.timeScale > 0f) & (baldi.antiHearingTime > 0f))
		{
			audioDevice.UnPause();
		}
	}

	public void Play()
	{
		if (!blockSpriteChange)
		{
			sprite.sprite = closedSprite;
		}
		audioDevice.Play();
		if (baldi.isActiveAndEnabled)
		{
			baldi.ActivateAntiHearing(30f);
		}
		if (miko.isActiveAndEnabled)
		{
			miko.ActivateAntiHearing(20f);
		}
		if (alger.isActiveAndEnabled)
        {
			alger.ActivateAntiHearing(30f);
        }
	}
}
