using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
	public Slider sliderSensetivity;
	public Slider volume;

	public Toggle easyMath;
	public Toggle fullscreen;
	public Toggle old;
	public Toggle shake;
	public Toggle yellow;
	public Toggle noMath;
	public Toggle tdCam;
	public Toggle scaleAutomatically;
	public Toggle captions;
	public Toggle minimap;
	public Toggle itemHeld;
	public Toggle timer;
	public Toggle vsync;
	public Toggle fastRestart;
	public TMP_InputField fps;
	public TMP_InputField scaleFactor;

	private void Start()
	{
		if (PlayerPrefs.HasKey("OptionsSet"))
		{
			this.sliderSensetivity.value = PlayerPrefs.GetFloat("MouseSensitivity");
			if (PlayerPrefs.GetInt("easyMath", 1) == 0) this.easyMath.isOn = false;
			else this.easyMath.isOn = true;
			if (PlayerPrefs.GetInt("fullscreen", 1) == 0) this.fullscreen.isOn = false;
			else this.fullscreen.isOn = true;
			if (PlayerPrefs.GetInt("shake", 1) == 0) this.shake.isOn = false;
			else this.shake.isOn = true;
			if (PlayerPrefs.GetInt("minimap", 0) == 0) this.minimap.isOn = false;
			else this.minimap.isOn = true;
			if (PlayerPrefs.GetInt("yellow", 0) == 0) this.yellow.isOn = false;
			else this.yellow.isOn = true;
			if (PlayerPrefs.GetInt("math", 0) == 0) this.noMath.isOn = true;
			else this.noMath.isOn = false;
			if (PlayerPrefs.GetInt("3dCam", 0) == 1) this.tdCam.isOn = true;
			else this.tdCam.isOn = false;
			if (PlayerPrefs.GetInt("scaleMode", 0) == 0) scaleAutomatically.isOn = true;
			else scaleAutomatically.isOn = false;
			if (PlayerPrefs.GetInt("captions", 0) == 1) this.captions.isOn = true;
			else this.captions.isOn = false;
			if (PlayerPrefs.GetInt("heldItemShow", 0) == 1) this.itemHeld.isOn = true;
			else this.itemHeld.isOn = false;
			if (PlayerPrefs.GetInt("timer", 0) == 1) this.timer.isOn = true;
			else this.timer.isOn = false;
			if (PlayerPrefs.GetInt("fastRestart", 0) == 1) this.fastRestart.isOn = true;
			else this.fastRestart.isOn = false;
			this.fps.text = PlayerPrefs.GetInt("fps", 60).ToString();
			this.scaleFactor.text = PlayerPrefs.GetFloat("scaleFactor", 1.5f).ToString();
			//I'm forced to do it like this.
			this.SetFps();
		}
		PlayerPrefs.SetInt("OptionsSet", 1);
	}

	public void UpdateSettings()
	{
		Savey();
	}

	void Update() //I can't move it out of update or else the options break. :(
	{
		if (this.easyMath.isOn) PlayerPrefs.SetInt("easyMath", 1);
		else PlayerPrefs.SetInt("easyMath", 0);
		if (this.tdCam.isOn) PlayerPrefs.SetInt("3dCam", 1);
		else PlayerPrefs.SetInt("3dCam", 0);
		if (this.minimap.isOn) PlayerPrefs.SetInt("minimap", 1);
		else PlayerPrefs.SetInt("minimap", 0);
		if (this.captions.isOn) PlayerPrefs.SetInt("captions", 1);
		else PlayerPrefs.SetInt("captions", 0);
		if (this.fullscreen.isOn)
		{
			PlayerPrefs.SetInt("fullscreen", 1);
			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
		}
		else
		{
			PlayerPrefs.SetInt("fullscreen", 0);
			Screen.fullScreenMode = FullScreenMode.Windowed;
		}
		if (this.shake.isOn) PlayerPrefs.SetInt("shake", 1);
		else PlayerPrefs.SetInt("shake", 0);
		if (this.confirmation.activeSelf) this.audioDevice.Pause();
		else this.audioDevice.UnPause();
		if (this.yellow.isOn) PlayerPrefs.SetInt("yellow", 1);
		else PlayerPrefs.SetInt("yellow", 0);
		if (this.noMath.isOn) PlayerPrefs.SetInt("math", 0);
		else PlayerPrefs.SetInt("math", 1);
		if (this.scaleAutomatically.isOn) PlayerPrefs.SetInt("scaleMode", 0);
		else PlayerPrefs.SetInt("scaleMode", 1);
		PlayerPrefs.SetFloat("audio", this.volume.value);
		AudioListener.volume = this.volume.value;
		PlayerPrefs.SetFloat("MouseSensitivity", this.sliderSensetivity.value);
		if (PlayerPrefs.GetInt("mikoBeat") == 1) this.yellow.interactable = true;
		else
		{
			yellow.gameObject.SetActive(false);
			this.yellow.interactable = false;
			PlayerPrefs.SetInt("yellow", 0);
			this.yellow.isOn = false;
		}
		if (this.itemHeld.isOn) PlayerPrefs.SetInt("heldItemShow", 1);
		else PlayerPrefs.SetInt("heldItemShow", 0);
		if (this.timer.isOn) PlayerPrefs.SetInt("timer", 1);
		else PlayerPrefs.SetInt("timer", 0);
		if (this.vsync.isOn)
		{
			QualitySettings.vSyncCount = 1;
			PlayerPrefs.SetInt("vsync", 1);
		}
		else
		{
			QualitySettings.vSyncCount = 0;
			PlayerPrefs.SetInt("vsync", 0);
		}
		if (QualitySettings.vSyncCount == 1) fps.enabled = false;
		else fps.enabled = true;
		if (fastRestart.isOn) PlayerPrefs.SetInt("fastRestart", 1);
		else PlayerPrefs.SetInt("fastRestart", 0);
		if (this.confirmation.activeSelf) this.audioDevice.Pause();
		else this.audioDevice.UnPause();
	}

    private void OnEnable()
	{
		if (PlayerPrefs.GetInt("mikoBeat") == 1)
		{
			this.yellow.interactable = true;
		}
		else
		{
			yellow.gameObject.SetActive(false);
			this.yellow.interactable = false;
			PlayerPrefs.SetInt("yellow", 0);
			this.yellow.isOn = false;
		}
	}

    public void Savey()
	{
		PlayerPrefs.Save();
		print("hey im saving trust");
    }

	public void SetFps()
	{
		if (!int.TryParse(this.fps.text, out int num) || num < 5)
		{
			this.fps.text = "5";
			num = 5;
		}
		PlayerPrefs.SetInt("fps", num);

		if (vsync.isOn)
		{
			QualitySettings.vSyncCount = 1;
			return;
		}

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = num;
	}

	public void SetFactor()
	{
		if (!float.TryParse(this.scaleFactor.text, out float num) || num < 0f)
		{
			this.scaleFactor.text = "0";
			num = 0f;
		}

		PlayerPrefs.SetFloat("scaleFactor", num);
	}


	public void ClearData()
    {
		for (int i = 1; i < 5; i++)
		{
			PlayerPrefs.SetInt($"foundPiece{i}", 0);
		}
		for (int i = 0; i < playerPrefInts.Count; i++) PlayerPrefs.DeleteKey(playerPrefInts[i]);
		PlayerPrefs.Save();
		SceneManager.LoadScene("BenefondCrates");
	}

	public void ClearAllData()
    {
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("BenefondCrates");
    }
	
	public void ClearOptionData()
    {
		PlayerPrefs.DeleteKey("easyMath");
		PlayerPrefs.DeleteKey("fullscreen");
		PlayerPrefs.DeleteKey("shake");
		PlayerPrefs.DeleteKey("3dCam");
		PlayerPrefs.SetInt("MouseSensitivity", 2);
		PlayerPrefs.DeleteKey("math");
		PlayerPrefs.DeleteKey("OptionsSet");
		PlayerPrefs.DeleteKey("vsync");
		PlayerPrefs.SetInt("fps", 60);
		PlayerPrefs.SetFloat("volume", 0.5f);
		PlayerPrefs.DeleteKey("scaleMode");
		PlayerPrefs.DeleteKey("captions");
		PlayerPrefs.SetFloat("scaleFactor", 1.5f);
		PlayerPrefs.DeleteKey("heldItemShow");
		PlayerPrefs.DeleteKey("timer");
		PlayerPrefs.DeleteKey("fastRestart");
		PlayerPrefs.Save();
		SceneManager.LoadScene("BenefondCrates");
	}

	public AudioSource globalAudio;
	public AudioSource audioDevice;
	public GameObject confirmation;
	public AudioClip boowomp;

	float wait = 0.5f;
	public List<string> playerPrefInts = new List<string>();
}
