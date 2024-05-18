using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSave : MonoBehaviour
{
	private void Start()
	{
		this.LoadValues();
	}

	public void SaveVolume()
	{
		PlayerPrefs.SetFloat("audio", this.volSlider.value);
		this.LoadValues();
		this.globalAudioSource.Stop();
		this.globalAudioSource.PlayOneShot(this.hi);
	}

	public void LoadValues()
	{
		float @float = PlayerPrefs.GetFloat("audio", 0.5f);
		this.volSlider.value = @float;
		AudioListener.volume = @float;
	}

	public Slider volSlider;

	public AudioClip hi;

	public AudioSource globalAudioSource;
}
