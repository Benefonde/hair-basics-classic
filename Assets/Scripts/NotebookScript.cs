using System;
using UnityEngine;

public class NotebookScript : MonoBehaviour
{
	private void Start()
	{
		this.up = true;
		base.transform.Find("DwayneMap").gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.gc.mode == "endless")
		{
			if (this.respawnTime > 0f)
			{
				if ((base.transform.position - this.player.position).magnitude > 10f)
				{
					this.respawnTime -= Time.deltaTime;
				}
			}
			else if (!this.up)
			{
				base.transform.position = new Vector3(base.transform.position.x, 4f, base.transform.position.z);
				this.up = true;
				base.transform.Find("DwayneMap").gameObject.SetActive(true);
				this.audioDevice.Play();
			}
		}
		RaycastHit raycastHit;
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || this.gc.playerScript.crazyAppleTimer > 0f) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f)), out raycastHit) && (raycastHit.transform.tag == "Notebook" & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance))
		{
			if (this.gc.mode != "miko")
			{
				base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
				this.up = false;
				base.transform.Find("DwayneMap").gameObject.SetActive(false);
				this.respawnTime = 99f;
				this.gc.CollectNotebook();
				GameObject gameObject = Instantiate<GameObject>(this.learningGame);
				gameObject.GetComponent<MathGameScript>().gc = this.gc;
				gameObject.GetComponent<MathGameScript>().baldiScript = this.bsc;
				gameObject.GetComponent<MathGameScript>().playerPosition = this.player.position;
				gameObject.GetComponent<MathGameScript>().playerScript = this.psc;
				gameObject.GetComponent<MathGameScript>().mikoScript = this.msc;
				return;
			}
			base.transform.position = new Vector3(base.transform.position.x, -20f, base.transform.position.z);
			this.up = false;
			base.transform.Find("DwayneMap").gameObject.SetActive(false);
			this.gc.CollectNotebook();
			GameObject gameObject2 = Instantiate<GameObject>(this.mikoYCTP);
			gameObject2.GetComponent<MikoGameScript>().gc = this.gc;
			gameObject2.GetComponent<MikoGameScript>().mikoScript = this.msc;
			gameObject2.GetComponent<MikoGameScript>().playerPosition = this.player.position;
			gameObject2.GetComponent<MikoGameScript>().playerScript = this.psc;
		}
	}

	public float openingDistance;

	public GameControllerScript gc;

	public BaldiScript bsc;

	public MikoScript msc;

	public float respawnTime;

	public bool up;

	public Transform player;

	public GameObject learningGame;

	public GameObject mikoYCTP;

	public AudioSource audioDevice;

	public PlayerScript psc;
}