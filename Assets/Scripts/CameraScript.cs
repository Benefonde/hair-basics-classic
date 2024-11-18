using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public bool cutsceneCam;
	
	public GameObject player;

	public PlayerScript ps;
	public BaldiPlayerScript bps;

	public float velocity;
	public float pipeGameHeight;

	public int lookBehind;

	public float verticalLook;

	public Vector3 offset;
	public bool FuckingDead;

	public Quaternion camRotation;

	public float jumpHeight;

	public Vector3 jumpHeightV3;

	public Vector3 originalPosition;

	public Camera cam;

	public GameControllerScript gc;

	public float shakeTime;

	public float camYoffset;
	public float camYdefault;

	public GameObject character;
	public Transform follow;

	public bool disable3Dcam;

	private int shake;

	private void Start()
	{
		offset = base.transform.position - player.transform.position;
		originalPosition = offset;
		camYoffset = base.transform.position.y - player.transform.position.y;
		camYdefault = 1;
		shake = PlayerPrefs.GetInt("shake", 1);
	}

	private void Update()
    {
		if (!ps.teleporting && !FuckingDead)
        {
			offset = new Vector3(offset.x, camYoffset, offset.z);
			if (Input.GetButton("Look Behind") && !ps.inSecret)
			{
				lookBehind = 180;
			}
			else
			{
				lookBehind = 0;
			}
			if (Input.GetKey(KeyCode.C))
			{
				cam.fieldOfView = 10;
			}
			else
			{
				cam.fieldOfView = 60;
			}
			if (gc.finaleMode && !ps.gameOver && shake == 1)
			{
				float ran1 = Random.Range(-0.05f, 0.05f);
				float ran2 = Random.Range(camYdefault, camYdefault + 0.1f);
				float ran3 = Random.Range(-0.05f, 0.05f);
				offset = new Vector3(ran1, ran2, ran3);
				camYoffset = ran2;
			}
		}
		if (ps.pipeGame && !ps.teleporting && !FuckingDead & !cutsceneCam && !ps.gameOver)
        {
			PipegameMove();
        }
	}

	private void LateUpdate()
	{
		verticalLook = Mathf.Clamp(verticalLook, -60, 60);
		if (PlayerPrefs.GetInt("3dCam", 0) == 0 || disable3Dcam)
		{
			verticalLook = 0;
		}
		if (!FuckingDead & !cutsceneCam)
        {
			base.transform.position = player.transform.position + offset;
		}

		if (FuckingDead & !cutsceneCam)
		{
			base.transform.position = follow.position + follow.forward * -15f + new Vector3(0f, 7.5f, 0f);
			cam.useOcclusionCulling = false;
			transform.LookAt(follow.position);
		}
		else
		{
			cam.useOcclusionCulling = true;
		}
		if (!ps.teleporting && !FuckingDead & !cutsceneCam)
		{
			if (!ps.gameOver & !ps.pipeGame)
			{
				base.transform.position = player.transform.position + offset;
				base.transform.rotation = player.transform.rotation * Quaternion.Euler(verticalLook, lookBehind, 0f);
			}
			else if (ps.pipeGame)
            {
				base.transform.position = player.transform.position + offset + new Vector3(0, pipeGameHeight, 0);
				base.transform.rotation = player.transform.rotation * Quaternion.Euler(verticalLook, lookBehind, 0f);
			}
			else if (ps.gameOver)
			{
				if (character.name == "Miko")
				{
					base.transform.position = character.transform.position + character.transform.forward * 2f + new Vector3(0f, 2.75f, 0f) + offset;
					base.transform.LookAt(new Vector3(character.transform.position.x, character.transform.position.y + 2.75f, character.transform.position.z) + offset);
				}
				else if (character.name == "Alger (Alger's Basics)")
				{
					base.transform.position = character.transform.position + character.transform.forward * -1f + new Vector3(2f, 3.85f, 0f) + offset;
					base.transform.LookAt(new Vector3(character.transform.position.x, character.transform.position.y + 3.85f, character.transform.position.z) + offset);
				}
				else if (character.name == "Bob")
				{
					ps.bob.gameObject.layer = 9;
					base.transform.position = character.transform.position + character.transform.forward * 1f + new Vector3(0f, 2.05f, 0f) + offset;
					base.transform.LookAt(new Vector3(character.transform.position.x, character.transform.position.y + 2.05f, character.transform.position.z) + offset);
				}
				else
				{
					base.transform.position = character.transform.position + character.transform.forward * 2f + new Vector3(0f, 4.25f, 0f) + offset;
					base.transform.LookAt(new Vector3(character.transform.position.x, character.transform.position.y + 4.25f, character.transform.position.z) + offset);
				}
				if (!gc.finaleMode && shake == 1)
				{
					float ran1 = Random.Range(-0.1f, 0.1f);
					float ran2 = Random.Range(camYdefault - 0.1f, camYdefault - 0.9f);
					float ran3 = Random.Range(-0.1f, 0.1f);
					offset = new Vector3(ran1, ran2, ran3);
					camYoffset = ran2;
				}
				else if (shake == 1)
				{
					float ran1 = Random.Range(-1f, 1f);
					float ran2 = Random.Range(camYdefault - 0.5f, camYdefault - 1.5f);
					float ran3 = Random.Range(-1f, 1f);
					offset = new Vector3(ran1, ran2, ran3);
					camYoffset = ran2;
				}
			}
		}
	}
	public void ShakeNow(Vector3 intensity, int frames)
	{
		StartCoroutine(ShakeCoroutine(intensity, frames));
	}

	void PipegameMove()
	{
		if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
		{
			if (pipeGameHeight == 0)
			{
				ps.pipeGameGravity = -6;
			}
		}
		if (ps.pipeGameGravity != 0)
		{
			pipeGameHeight += ps.pipeGameGravity * Time.deltaTime;
			ps.pipeGameGravity += 16 * Time.deltaTime;
		}
		if (pipeGameHeight >= 0)
		{
			ps.pipeGameGravity = 0;
			pipeGameHeight = 0;
		}
	}

	private IEnumerator ShakeCoroutine(Vector3 intensity, int frames)
	{
		if (shake == 1)
        {
			Vector3 accumulatedOffset = Vector3.zero;

			for (int i = 0; i < frames; i++)
			{
				float ran1 = Random.Range(-intensity.x, intensity.x);
				float ran2 = Random.Range(camYdefault - intensity.y, camYdefault + intensity.y);
				float ran3 = Random.Range(-intensity.z, intensity.z);
				Vector3 frameOffset = new Vector3(ran1, ran2, ran3);

				accumulatedOffset += frameOffset;
				offset = frameOffset;
				camYoffset = ran2;

				yield return null;
			}

			transform.position += accumulatedOffset;
			offset = originalPosition;
			camYoffset = originalPosition.y;
		}
    }

	public void SetCharacter(GameObject killer)
	{
		character = killer;
		Debug.Log("Character set: " + character.name);
	}
}
