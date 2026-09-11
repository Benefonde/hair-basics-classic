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
	}

	public void UpdateSettings()
	{
		PlayerPrefs.SetInt("easyMath", easyMath.isOn ? 1 : 0);
		PlayerPrefs.SetInt("3dCam", tdCam.isOn ? 1 : 0);
		PlayerPrefs.SetInt("minimap", minimap.isOn ? 1 : 0);
		PlayerPrefs.SetInt("captions", captions.isOn ? 1 : 0);
		PlayerPrefs.SetInt("fullscreen", fullscreen.isOn ? 1 : 0);
		Screen.fullScreenMode = fullscreen.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
		PlayerPrefs.SetInt("shake", shake.isOn ? 1 : 0);
		PlayerPrefs.SetInt("yellow", yellow.isOn ? 1 : 0);
		PlayerPrefs.SetInt("math", noMath.isOn ? 0 : 1);
		PlayerPrefs.SetInt("scaleMode", scaleAutomatically.isOn ? 1 : 0);
		PlayerPrefs.SetFloat("audio", this.volume.value);
		AudioListener.volume = this.volume.value;
		PlayerPrefs.SetFloat("MouseSensitivity", this.sliderSensetivity.value);
		PlayerPrefs.SetInt("heldItemShow", itemHeld.isOn ? 1 : 0);
		PlayerPrefs.SetInt("timer", timer.isOn ? 1 : 0);
		QualitySettings.vSyncCount = vsync.isOn ? 1 : 0;
		PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount); 
		fps.enabled = !vsync.isOn;
		PlayerPrefs.SetInt("fastRestart", fastRestart.isOn ? 1 : 0);
		Savey();
	}

	void Update()
    {
		if (this.confirmation.activeSelf) this.audioDevice.Pause();
		else this.audioDevice.UnPause();

		if (Input.GetKeyDown(KeyCode.I)) Load();
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
		Invoke(nameof(Load), 0.1f);
	}

	void Load()
    {
		//if (PlayerPrefs.HasKey("OptionsSet"))
		{
			sliderSensetivity.value = PlayerPrefs.GetFloat("MouseSensitivity");
			easyMath.isOn = PlayerPrefs.GetInt("easyMath", 1) == 1;
			print($"easy math {PlayerPrefs.GetInt("easyMath", 1) == 1}");
			fullscreen.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
			print($"fullcreen {PlayerPrefs.GetInt("fullscreen", 1) == 1}");
			shake.isOn = PlayerPrefs.GetInt("shake", 1) == 1;
			print($"shaky {PlayerPrefs.GetInt("shake", 1) == 1}");
			minimap.isOn = PlayerPrefs.GetInt("minimap", 0) == 1;
			print($"minmap {PlayerPrefs.GetInt("minimap", 0) == 1}");
			yellow.isOn = PlayerPrefs.GetInt("yellow", 0) == 1;
			print($"yellow {PlayerPrefs.GetInt("yellow", 0) == 1}");
			noMath.isOn = PlayerPrefs.GetInt("math", 1) == 0;
			print($"no math {PlayerPrefs.GetInt("math", 1) == 0}");
			tdCam.isOn = PlayerPrefs.GetInt("3dCam", 0) == 1;
			print($"teardrop cam {PlayerPrefs.GetInt("3dCam", 0) == 1}");
			scaleAutomatically.isOn = PlayerPrefs.GetInt("scaleMode", 1) == 1;
			print($"scale auto {PlayerPrefs.GetInt("scaleMode", 1) == 1}");
			captions.isOn = PlayerPrefs.GetInt("captions", 0) == 1;
			print($"captions {PlayerPrefs.GetInt("captions", 0) == 1}");
			itemHeld.isOn = PlayerPrefs.GetInt("heldItemShow", 0) == 1;
			print($"itemhels {PlayerPrefs.GetInt("heldItemShow", 0) == 1}");
			timer.isOn = PlayerPrefs.GetInt("timer", 0) == 1;
			print($"timer {PlayerPrefs.GetInt("timer", 0) == 1}");
			fastRestart.isOn = PlayerPrefs.GetInt("fastRestart", 0) == 1;
			print($"fastten {PlayerPrefs.GetInt("fastRestart", 0) == 1}");
			fps.text = PlayerPrefs.GetInt("fps", 60).ToString();
			scaleFactor.text = PlayerPrefs.GetFloat("scaleFactor", 1.5f).ToString();
			SetFps();
		}
		PlayerPrefs.SetInt("OptionsSet", 1);
	}

    public void Savey()
	{
		/*if (wait > 0)
        {
			print("woah there buddy...");
			return;
        }
		PlayerPrefs.Save();
		print("hey im saving trust");*/
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
