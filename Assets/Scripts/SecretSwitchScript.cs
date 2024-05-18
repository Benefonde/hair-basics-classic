using System;
using UnityEngine;

public class SecretSwitchScript : MonoBehaviour
{
	private void Update()
	{
		RaycastHit raycastHit;
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f)), out raycastHit) && (raycastHit.transform.tag == "Interactable" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
		{
			gameObject.tag = "Untagged";
			RenderSettings.ambientLight = Color.gray;
			gc.audioDevice.Stop();
			gc.audioDevice.PlayOneShot(switchOn);
			StartCoroutine(gc.SwitchPressed());
			sm.Add3DSubtitle("*Switch on*", switchOn.length, Color.white, transform, gc.audioDevice);
		}
	}

	public AudioClip switchOn;

	public float openingDistance;

	public GameControllerScriptSimple gc;

	public Transform player;

	public SubtitleManager sm;
}
