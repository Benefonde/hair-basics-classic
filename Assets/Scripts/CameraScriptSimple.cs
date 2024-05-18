using System.Collections;
using UnityEngine;

public class CameraScriptSimple : MonoBehaviour
{
	public GameObject player;

	public GameObject panino;

	public PlayerScriptSimple ps;

	public int lookBehind;

	public float verticalLook;

	public Vector3 offset;

	public Vector3 originalPosition;

	public Camera cam;

	public float camYoffset;
	public float camYdefault;

	private void Start()
	{
		offset = base.transform.position - player.transform.position;
		originalPosition = offset;
		camYoffset = base.transform.position.y - player.transform.position.y;
		camYdefault = offset.y;
	}

	private void Update()
	{
		offset = new Vector3(offset.x, camYoffset, offset.z);
		if (Input.GetButton("Look Behind"))
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
	}

	private void LateUpdate()
	{
		verticalLook = Mathf.Clamp(verticalLook, -60, 60);
		if (PlayerPrefs.GetInt("3dCam", 0) == 0)
		{
			verticalLook = 0;
		}
		if (!ps.gameOver)
		{
			base.transform.position = player.transform.position + offset;
			base.transform.rotation = player.transform.rotation * Quaternion.Euler(verticalLook, lookBehind, 0f);
		}
        else
        {
			base.transform.position = panino.transform.position + panino.transform.forward * 2f + new Vector3(0f, 4.25f, 0f) + offset;
			base.transform.LookAt(new Vector3(panino.transform.position.x, panino.transform.position.y + 4.25f, panino.transform.position.z) + offset);
		}
	}
}
