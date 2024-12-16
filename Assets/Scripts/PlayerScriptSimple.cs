using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScriptSimple : MonoBehaviour
{
	public GameControllerScriptSimple gc;

	public Quaternion playerRotation;

	public bool sensitivityActive;

	private float sensitivity;

	public float mouseSensitivity;

	public float walkSpeed;

	public float runSpeed;

	private Vector3 moveDirection;

	public bool gameOver;

	private float playerSpeed;

	public CharacterController cc;

	public float height;

	public GameObject panino;

	private void Start()
	{
		height = base.transform.position.y;
		playerRotation = base.transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
	}

	private void Update()
	{
		if (sensitivityActive)
		{
			mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
		}
		PlayerMove();
		MouseMove();
		if (cc.velocity.magnitude > 0f)
		{
			gc.LockMouse();
		}
		if (sensitivityActive)
		{
			mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
		}
	}

	private void MouseMove()
	{
		playerRotation.eulerAngles = new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y, playerRotation.eulerAngles.z);
		playerRotation.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale;
		if (PlayerPrefs.GetInt("3dCam", 0) == 1)
		{
			camscript.verticalLook -= 0.8f * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale;
		}
		if (!camscript.cutsceneCam)
		{
			camscript.transform.rotation = transform.rotation;
		}
		base.transform.rotation = playerRotation;
	}

	private void PlayerMove()
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		vector = base.transform.forward * Input.GetAxis("Forward");
		vector2 = base.transform.right * Input.GetAxis("Strafe");
		if (Input.GetButton("Run"))
		{
			playerSpeed = runSpeed;
			sensitivity = 1f;
		}
		else
		{
			playerSpeed = walkSpeed;
			if (sensitivityActive)
			{
				sensitivity = Mathf.Clamp((vector2 + vector).magnitude, 0f, 1f);
			}
			else
			{
				sensitivity = 1f;
			}
		}
		playerSpeed *= Time.deltaTime;
		moveDirection = (vector + vector2).normalized * playerSpeed * sensitivity;
		cc.Move(moveDirection);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Panino")
        {
			SceneManager.LoadScene("BenefondCrates");
        }
		if (other.transform.name == "TriggerPanino")
        {
			panino.SetActive(true);
        }
		if (other.transform.name == "My Time Machine")
        {
			walkSpeed = 0;
			runSpeed = 0;
			sensitivityActive = false;
			mouseSensitivity = 0;
			StartCoroutine(TimeMachine());
		}
    }

	IEnumerator TimeMachine()
	{
		fade.color = Color.clear;
		for (int i = 0; i < 25; i++)
        {
			fade.color = new Color(1, 1, 1, fade.color.a + 0.04f);
			yield return new WaitForSeconds(0.1667f); // 60fps basically
        }
		CutsceneAFTERTimeMachine();
    }

	void CutsceneAFTERTimeMachine()
    {
		camscript.gameObject.GetComponent<Animator>().SetTrigger("baldi dies time");
		camscript.enabled = false;
		fade.color = Color.clear;
		cutscene.SetActive(true);
	}

    public CameraScriptSimple camscript;

	public Image fade;

	public GameObject cutscene;
}
