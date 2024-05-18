using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreenScript : MonoBehaviour
{
	private void Update()
	{
		if (timer > 0)
        {
			timer -= 1 * Time.deltaTime;
        }
        else
        {
			if (Input.anyKeyDown)
			{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}

	public float timer;
}
