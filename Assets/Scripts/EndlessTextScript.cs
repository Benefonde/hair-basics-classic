using TMPro;
using UnityEngine;

public class EndlessTextScript : MonoBehaviour
{
	public TMP_Text text;

	private void Start()
	{
		text.text = text.text + "\nBest: " + PlayerPrefs.GetInt("HighBooks") + " Dwaynes";
	}
}
