using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
	private void Start()
	{
        if (rig == 0)
        {
            int rng = Mathf.FloorToInt(Random.Range(0, 50.5f));
            Debug.LogWarning(rng);
            if (rng < 10)
            {
                audioDevice.Play();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else if (rig == 1)
        {
            audioDevice.Play();
        }
        else if (rig == 2)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}

    private void Update()
    {
        if (audioDevice.isPlaying && Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public AudioSource audioDevice;

    public int rig;
}
