using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonScript : MonoBehaviour
{
	public void ExitGame()
	{
        this.balls.SetTrigger("balls");
        this.audioDevice.PlayOneShot(this.walls);
        this.StartCoroutine(this.Valls());
    }
    
    public IEnumerator Valls()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("BenefondCrates");
    }

    public Animator balls;

    public AudioClip walls;

    public AudioSource audioDevice;
}
