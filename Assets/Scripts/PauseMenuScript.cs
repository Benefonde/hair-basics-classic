using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{
	public GameControllerScript gc;

	public GameObject[] pauseThings;


	private void Update()
	{
		if (gc.mouseLocked)
		{
			gc.UnlockMouse();
		}
	}

	public void HideShowButtons()
    {
		for (int i = 0; i < pauseThings.Length; i++)
		{
			pauseThings[i].SetActive(!pauseThings[i].activeSelf);
		}
	}
}
