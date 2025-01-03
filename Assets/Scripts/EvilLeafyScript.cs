using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EvilLeafyScript : MonoBehaviour
{
	public bool db;

	public float speed;

	public float timeToMove;

	public float baldiWait;

	public int moveFrames;

	public Transform player;

	private AudioSource baldiAudio;

	public AudioClip slap;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	private void Start()
	{
		baldiAudio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		this.TargetPlayer();
		if (timeToMove > 0f)
		{
			timeToMove -= 1f * Time.deltaTime;
		}
		else
		{
			Move();
		}
	}

	private void FixedUpdate()
	{
		if (moveFrames > 0f)
		{
			moveFrames -= 1;
			agent.acceleration = speed * 200;
			agent.speed = speed;
			if (Vector3.Distance(player.position, transform.position) <= 40)
            {
				gc.audioDevice.PlayOneShot(gc.bfdiScream, 0.6f);
            }
		}
		else
		{
			agent.speed = 0f;
		}
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.name == "Player"))
		{
			db = true;
			TargetPlayer();
		}
		else
		{
			db = false;
		}
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
	}

	private void Move()
	{
		moveFrames = 1;
		baldiWait -= 0.01015f;
		timeToMove = baldiWait;
		baldiAudio.PlayOneShot(slap);
		if (gc.isActiveAndEnabled)
		{
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*BOOM*", 2, Color.red, transform);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
		{
			timeToMove += 5;
		}
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}
}
