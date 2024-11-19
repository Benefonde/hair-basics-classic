using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BaldiPlayerScript : MonoBehaviour
{
	public float baseTime;

	public float speed;

	public float timeToMove;

	public float baldiAnger;

	public float baldiTempAnger;

	public float baldiWait;

	public float baldiSpeedScale;

	private float moveFrames;

	public bool antiHearing;

	public float antiHearingTime;

	public float timeToAnger;

	public bool sensitivityActive;

	private float sensitivity;

	public float mouseSensitivity;

	public Transform player;

	private AudioSource baldiAudio;

	public AudioClip slap;

	public AudioClip[] speech = new AudioClip[3];

	public Animator baldiAnimator;

	float speedRn;

	public GameControllerScript gc;

	Quaternion playerRotation;

	CharacterController cc;

	List<Collider> squee = new List<Collider>();

	public Slider paninisSlider;

	public GameObject soundThing;

	private void Start()
	{
		playerRotation = transform.rotation;
		baldiAudio = GetComponent<AudioSource>();
		cc = GetComponent<CharacterController>();
		timeToMove = baseTime;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2);
		Hear(player.position);
		baldiWait = -3f * baldiAnger / (baldiAnger + 2f / baldiSpeedScale) + 3f;
	}

	public void FindSquees()
	{
		squee.Clear();
		for (int i = 0; i < FindObjectsOfType<SqueeScript>().Length; i++)
		{
			squee.Add(FindObjectsOfType<SqueeScript>()[i].GetComponent<Collider>());
		}
	}
	private void MouseMove()
	{
		playerRotation.eulerAngles = new Vector3(playerRotation.eulerAngles.x, playerRotation.eulerAngles.y);
		playerRotation.eulerAngles += Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale;
		if (PlayerPrefs.GetInt("3dCam", 0) == 1)
		{
			gc.camScript.verticalLook -= 0.8f * Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale;
		}
		base.transform.rotation = playerRotation;
	}

	private void Update()
	{
		if (timeToMove > 0f)
		{
			timeToMove -= 1f * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.W))
		{
			Move();
		}
		MouseMove();
		paninisSlider.value = timeToMove;
		paninisSlider.maxValue = baldiWait;
		if (baldiTempAnger > 0f)
		{
			baldiTempAnger -= 0.02f * Time.deltaTime;
		}
		else
		{
			baldiTempAnger = 0f;
		}
		if (antiHearingTime > 0f)
		{
			antiHearingTime -= Time.deltaTime;
		}
		else
		{
			antiHearing = false;
		}
	}

	private void FixedUpdate()
	{
		if (moveFrames > 0f)
		{
			moveFrames -= 1f;
			cc.Move(speed * Time.deltaTime * transform.forward);
		}
	}

	private void Move()
	{
		moveFrames = 8f;
		timeToMove = baldiWait - baldiTempAnger;
		baldiAudio.PlayOneShot(slap);
		if (gc.isActiveAndEnabled)
		{
			FindObjectOfType<SubtitleManager>().Add2DSubtitle("balls", slap.length, Color.cyan);
		}
		baldiAnimator.SetTrigger("slap");
	}

	public void GetAngry(float value)
	{
		baldiAnger += value;
		if (baldiAnger < 0.5f)
		{
			baldiAnger = 0.5f;
		}
		baldiWait = -3f * baldiAnger / (baldiAnger + 2f / baldiSpeedScale) + 3f;
	}

	public void GetTempAngry(float value)
	{
		baldiTempAnger += value;
	}

	public void Hear(Vector3 soundLocation)
	{
		if (squee.Count != 0)
		{
			for (int i = 0; i < squee.Count; i++)
			{
				if (squee[i].bounds.Contains(soundLocation))
				{
					return;
				}
			}
		}
		if (FindObjectsOfType<SoundScript>().Length > 0)
        {
			for (int i = 0; i > FindObjectsOfType<SoundScript>().Length; i++)
            {
				Destroy(FindObjectsOfType<SoundScript>()[i].gameObject);
            }
        }
		GameObject a = Instantiate(soundThing, soundLocation, Quaternion.identity);
		a.GetComponent<SoundScript>().panino = transform;
	}

	public void ActivateAntiHearing(float t)
	{
		antiHearing = true;
		antiHearingTime = t;
	}
	public void Die()
	{
		gc.SomeoneTied(gameObject);
		cc.enabled = false;
		mouseSensitivity = 0;
		transform.position = new Vector3(transform.position.x, -5, transform.position.z);
		gc.hud.SetActive(false);
		Invoke("Tie", 2);
	}

	void Tie()
    {
		SceneManager.LoadScene("MainMenu");
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Yellow Face")
		{
			Die();
		}
	}
}
