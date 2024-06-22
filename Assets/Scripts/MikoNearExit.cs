using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MikoNearExit : MonoBehaviour
{
	public Animator cameraTime;

	public GameObject player;
	public PlayerScript ps;
	public Transform mikoTransform;

	public EntranceScript es;

	public AudioClip mikoDoesntLikeMeWaaaaaa;

	public bool failsafe;

	private void OnTriggerEnter(Collider other)
	{
		string[] a = { "Muahaha!", "I am not letting you beat this mode, because", "uhh...", "I don't like you!" };
		float[] b = { 1.9f, 3.1959f, 1.997f, 1.51252f };
		Color[] c = { Color.blue, Color.blue, Color.blue, Color.blue };
		if (other.tag == "Player" && PlayerPrefs.GetString("CurrentMode") == "miko" && !failsafe)
		{
			for (int i = 0; i < 4; i++)
            {
				ps.gc.item[i] = 0;
            }
			mikoTransform.gameObject.GetComponent<MikoScript>().enabled = false;
			mikoTransform.gameObject.GetComponent<NavMeshAgent>().Warp(new Vector3(-35, 2, 335));
			mikoTransform.gameObject.GetComponent<AudioSource>().PlayOneShot(mikoDoesntLikeMeWaaaaaa);
			FindObjectOfType<SubtitleManager>().AddChained3DSubtitle(a, b, c, mikoTransform);
			player.transform.position = new Vector3(-65, 4, 335);
			ps.bob.gameObject.layer = 9;
			ps.enabled = false;
			ps.gc.audioDevice.PlayOneShot(ps.gc.aud_Switch);
			es.Lower();
			failsafe = true;
			gameObject.GetComponent<BoxCollider>().enabled = false;
			cameraTime.gameObject.GetComponent<CameraScript>().cutsceneCam = true;
			cameraTime.SetTrigger("miko cutscene time");
			StartCoroutine(StartCutscene());
		}
	}

	IEnumerator StartCutscene()
    {
		player.transform.LookAt(mikoTransform);
		yield return new WaitForSeconds(9.65f);
		ps.gc.Objection();
		es.Raise();
		yield return new WaitForSeconds(5);
		if (ps.gc.ModifierOn())
        {
			ps.camscript.cutsceneCam = false;
			ps.camscript.character = mikoTransform.gameObject;
			ps.gameOver = true;
			yield break;
        }
		PlayerPrefs.SetInt("mikoBeat", 1);
		PlayerPrefs.SetString("bonusTextString", "Wow! Panino is IMPRESSED! You're do Great! He gave you \"BLOCK PATH\" powerup. Use in modifier tab. Press T to use!");
		SceneManager.LoadScene("ChallengeBeat");
	}
}