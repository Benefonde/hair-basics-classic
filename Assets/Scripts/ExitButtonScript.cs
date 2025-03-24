using System.Collections;
using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
	public void ExitGame()
	{
        if (instaQuit)
        {
            Application.Quit();
            return;
        }
        this.balls.SetTrigger("balls");
        this.audioDevice.PlayOneShot(this.walls);
        this.StartCoroutine(this.Valls());
    }
    
    public IEnumerator Valls()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public Animator balls;

    public AudioClip walls;

    public AudioSource audioDevice;

    public bool instaQuit = false;
}
