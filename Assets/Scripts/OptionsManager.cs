using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;

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
			if (PlayerPrefs.GetInt("easyMath", 1) == 0)
			{
				this.easyMath.isOn = false;
			}
			else
			{
				this.easyMath.isOn = true;
			}
			if (PlayerPrefs.GetInt("shake", 1) == 0)
			{
				this.shake.isOn = false;
			}
			else
			{
				this.shake.isOn = true;
			}
			if (PlayerPrefs.GetInt("minimap", 0) == 0)
			{
				this.minimap.isOn = false;
			}
			else
			{
				this.minimap.isOn = true;
			}
			if (PlayerPrefs.GetInt("yellow", 0) == 0)
			{
				this.yellow.isOn = false;
			}
			else
			{
				this.yellow.isOn = true;
			}
			if (PlayerPrefs.GetInt("math", 0) == 0)
			{
				this.noMath.isOn = true;
			}
			else
			{
				this.noMath.isOn = false;
			}
			if (PlayerPrefs.GetInt("3dCam", 0) == 1)
			{
				this.tdCam.isOn = true;
			}
			else
			{
				this.tdCam.isOn = false;
			}
			if (PlayerPrefs.GetInt("scaleMode", 0) == 0)
			{
				scaleAutomatically.isOn = true;
			}
			else
			{
				scaleAutomatically.isOn = false;
			}
			if (PlayerPrefs.GetInt("captions", 0) == 1)
			{
				this.captions.isOn = true;
			}
			else
			{
				this.captions.isOn = false;
			}
			if (PlayerPrefs.GetInt("heldItemShow", 0) == 1)
			{
				this.itemHeld.isOn = true;
			}
			else
			{
				this.itemHeld.isOn = false;
			}
			if (PlayerPrefs.GetInt("timer", 0) == 1)
			{
				this.timer.isOn = true;
			}
			else
			{
				this.timer.isOn = false;
			}
			if (PlayerPrefs.GetInt("fastRestart", 0) == 1)
			{
				this.fastRestart.isOn = true;
			}
			else
			{
				this.fastRestart.isOn = false;
			}
			this.fps.text = PlayerPrefs.GetInt("fps", 60).ToString();
			this.scaleFactor.text = PlayerPrefs.GetFloat("scaleFactor", 1.5f).ToString();
			
			this.SetFps();
		}
		PlayerPrefs.SetInt("OptionsSet", 1);
	}

	private void Update()
	{
		if (this.easyMath.isOn)
		{
			PlayerPrefs.SetInt("easyMath", 1);
		}
		else
		{
			PlayerPrefs.SetInt("easyMath", 0);
		}
		if (this.tdCam.isOn)
		{
			PlayerPrefs.SetInt("3dCam", 1);
		}
		else
		{
			PlayerPrefs.SetInt("3dCam", 0);
		}
		if (this.minimap.isOn)
		{
			PlayerPrefs.SetInt("minimap", 1);
		}
		else
		{
			PlayerPrefs.SetInt("minimap", 0);
		}
		if (this.captions.isOn)
		{
			PlayerPrefs.SetInt("captions", 1);
		}
		else
		{
			PlayerPrefs.SetInt("captions", 0);
		}
		if (this.shake.isOn)
		{
			PlayerPrefs.SetInt("shake", 1);
		}
		else
		{
			PlayerPrefs.SetInt("shake", 0);
		}
		if (this.confirmation.activeSelf)
		{
			this.audioDevice.Pause();
		}
		else
		{
			this.audioDevice.UnPause();
		}
		if (this.yellow.isOn)
		{
			PlayerPrefs.SetInt("yellow", 1);
		}
		else
		{
			PlayerPrefs.SetInt("yellow", 0);
		}
		if (this.noMath.isOn)
		{
			PlayerPrefs.SetInt("math", 0);
		}
		else
		{
			PlayerPrefs.SetInt("math", 1);
		}
		if (this.scaleAutomatically.isOn)
		{
			PlayerPrefs.SetInt("scaleMode", 0);
		}
		else
		{
			PlayerPrefs.SetInt("scaleMode", 1);
		}
		PlayerPrefs.SetFloat("audio", this.volume.value);
		AudioListener.volume = this.volume.value;
		PlayerPrefs.SetFloat("MouseSensitivity", this.sliderSensetivity.value);
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
		if (this.itemHeld.isOn)
		{
			PlayerPrefs.SetInt("heldItemShow", 1);
		}
		else
		{
			PlayerPrefs.SetInt("heldItemShow", 0);
		}
		if (this.timer.isOn)
		{
			PlayerPrefs.SetInt("timer", 1);
		}
		else
		{
			PlayerPrefs.SetInt("timer", 0);
		}
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
		if (fastRestart.isOn)
		{
			PlayerPrefs.SetInt("fastRestart", 1);
		}
		else
		{
			PlayerPrefs.SetInt("fastRestart", 0);
		}
		this.CheckInput();
		
	}

	public void SaveOptions()
    {
		PlayerPrefs.Save();
    }

	private void CheckInput()
	{
		if (this.fps.isActiveAndEnabled)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				TMP_InputField tmp_InputField = this.fps;
				tmp_InputField.text += "1";
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				TMP_InputField tmp_InputField2 = this.fps;
				tmp_InputField2.text += "2";
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				TMP_InputField tmp_InputField3 = this.fps;
				tmp_InputField3.text += "3";
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				TMP_InputField tmp_InputField4 = this.fps;
				tmp_InputField4.text += "4";
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				TMP_InputField tmp_InputField5 = this.fps;
				tmp_InputField5.text += "5";
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				TMP_InputField tmp_InputField6 = this.fps;
				tmp_InputField6.text += "6";
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				TMP_InputField tmp_InputField7 = this.fps;
				tmp_InputField7.text += "7";
			}
			if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				TMP_InputField tmp_InputField8 = this.fps;
				tmp_InputField8.text += "8";
			}
			if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				TMP_InputField tmp_InputField9 = this.fps;
				tmp_InputField9.text += "9";
			}
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				TMP_InputField tmp_InputField10 = this.fps;
				tmp_InputField10.text += "0";
			}
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				this.fps.text = this.fps.text.Remove(this.fps.text.Length - 1, 1);
			}
			if (Input.GetKeyDown(KeyCode.Return))
			{
				this.SetFps();
			}
		}
		if (this.scaleFactor.isActiveAndEnabled)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.scaleFactor.text += "1";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				this.scaleFactor.text += "2";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				this.scaleFactor.text += "3";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				this.scaleFactor.text += "4";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				this.scaleFactor.text += "5";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				this.scaleFactor.text += "6";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				this.scaleFactor.text += "7";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				this.scaleFactor.text += "8";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				this.scaleFactor.text += "9";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.scaleFactor.text += "0";
			}
			else if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Comma))
			{
				this.scaleFactor.text += ",";
			}
			else if (Input.GetKeyDown(KeyCode.Backspace))
			{
				if (this.scaleFactor.text.Length > 0)
				{
					this.scaleFactor.text = this.scaleFactor.text.Remove(this.scaleFactor.text.Length - 1, 1);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Return))
			{
				this.SetFactor();
			}
		}
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
		PlayerPrefs.SetInt("mikoUnlocked", 0);
		for (int i = 1; i < 5; i++)
		{
			PlayerPrefs.SetInt($"foundPiece{i}", 0);
		}
		PlayerPrefs.SetInt("piecesFound", 0);
		PlayerPrefs.DeleteKey("yellow");
		PlayerPrefs.SetInt("speedyUnlocked", 0);
		PlayerPrefs.SetInt("mikoBeat", 0);
		PlayerPrefs.SetInt("tripleBeat", 0);
		PlayerPrefs.SetInt("pSecretFound", 0);
		PlayerPrefs.SetInt("algerBeat", 0);
		PlayerPrefs.SetInt("speedyBeat", 0);
		PlayerPrefs.SetInt("HighBooks", 0);
		PlayerPrefs.SetInt("secretEnd", 0);
		PlayerPrefs.SetInt("did3872643", 0);
		PlayerPrefs.SetInt("pizzaBeat", 0);
		PlayerPrefs.SetInt("pizzaScoreBest", 0);
		PlayerPrefs.SetString("pizzaRankBest", "D");
		PlayerPrefs.SetInt("pizzaLapsBest", 0);
		if (PlayerPrefs.GetInt("pizzaLapsNew", 0) > 0)
        {
			PlayerPrefs.DeleteKey("pizzaScoreNew");
			PlayerPrefs.DeleteKey("pizzaRankNew");
			PlayerPrefs.DeleteKey("pizzaLapsNew");
		}
		PlayerPrefs.SetInt("stealthyBeat", 0);
		PlayerPrefs.SetInt("classicBeat", 0);
		PlayerPrefs.SetInt("zombieBeat", 0);
		PlayerPrefs.SetInt("paninoBeat", 0);
		PlayerPrefs.SetInt("speedBoost", 0);
		PlayerPrefs.SetInt("extraStamina", 0);
		PlayerPrefs.SetInt("slowerKriller", 0);
		PlayerPrefs.SetInt("walkThrough", 0);
		PlayerPrefs.SetInt("blockPath", 0);
		PlayerPrefs.SetInt("infItem", 0);
		PlayerPrefs.SetInt("jammerUnlocked", 0);
		PlayerPrefs.SetInt("jammer", 0);
		PlayerPrefs.SetInt("cautionTrophy", 0);
		PlayerPrefs.SetInt("cleanTrophy", 0);
		PlayerPrefs.SetInt("congrattationTrophy", 0);
		PlayerPrefs.SetInt("giveAllTrophy", 0);
		PlayerPrefs.SetInt("jamTrophy", 0);
		PlayerPrefs.SetInt("noiseTrophy", 0);
		PlayerPrefs.SetInt("blueTrophy", 0);
		PlayerPrefs.SetInt("bareTrophy", 0);
		PlayerPrefs.SetInt("secretTrophy", 0);
		PlayerPrefs.SetInt("speedTrophy", 0);
		PlayerPrefs.SetInt("horribleTrophy", 0);
		PlayerPrefs.SetInt("lightTrophy", 0);
		PlayerPrefs.SetInt("mamaMiaTrophy", 0);
		PlayerPrefs.SetInt("debtTrophy", 0);
		PlayerPrefs.SetInt("codeTrophy", 0);
		PlayerPrefs.SetInt("bfdiTrophy", 0);
		PlayerPrefs.SetInt("stealthyBlueTrophy", 0);
		PlayerPrefs.SetInt("secretsTrophy", 0);
		PlayerPrefs.SetInt("desperateTrophy", 0);
		PlayerPrefs.SetInt("prankedTrophy", 0);
		PlayerPrefs.SetInt("hurtTrophy", 0);
		PlayerPrefs.SetInt("modeTrophy", 0);
		PlayerPrefs.SetInt("looreTrophy", 0);
		PlayerPrefs.SetInt("raTrophy", 0);
		PlayerPrefs.SetInt("ruleTrophy", 0);
		PlayerPrefs.SetInt("problemTrophy", 0);
		PlayerPrefs.SetInt("sorryTrophy", 0);
		PlayerPrefs.SetInt("congratulationsTrophy", 0);
		PlayerPrefs.SetInt("paninoTrophy", 0);
		PlayerPrefs.SetInt("timeTrophy", 0);
		PlayerPrefs.SetInt("sorgyTrophy", 0);
		PlayerPrefs.SetInt("slowTrophy", 0);
		PlayerPrefs.Save();
		SceneManager.LoadScene("Warning");
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
		PlayerPrefs.SetInt("MouseSensetivity", 2);
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
}
