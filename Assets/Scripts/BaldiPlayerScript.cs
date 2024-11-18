using UnityEngine;
using UnityEngine.AI;
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

	private float currentPriority;

	public bool antiHearing;

	public float antiHearingTime;

	public float vibrationDistance;

	public float angerRate;

	public float angerRateRate;

	public float angerFrequency;

	public float timeToAnger;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	private AudioSource baldiAudio;

	public AudioClip slap;

	public AudioClip[] speech = new AudioClip[3];

	public Animator baldiAnimator;

	public float coolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	float speedRn;

	public GameControllerScript gc;

	public bool resetAnger;

	List<Collider> squee = new List<Collider>();

	private void Start()
	{
		baldiAudio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		timeToMove = baseTime;
	}

	public void FindSquees()
	{
		squee.Clear();
		for (int i = 0; i < FindObjectsOfType<SqueeScript>().Length; i++)
		{
			squee.Add(FindObjectsOfType<SqueeScript>()[i].GetComponent<Collider>());
		}
	}

	private void Update()
	{
		if (timeToMove > 0f)
		{
			timeToMove -= 1f * Time.deltaTime;
		}
		else
		{
			Move();
		}
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
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
			speedRn = speed;
		}
		else
		{
			speedRn = 0f;
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

	public void Hear(Vector3 soundLocation, float priority)
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
	}

	public void ActivateAntiHearing(float t)
	{
		antiHearing = true;
		antiHearingTime = t;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}
}
