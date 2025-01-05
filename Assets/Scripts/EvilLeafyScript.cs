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

	public bool screamed;

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
			if (!screamed && agent.speed == 0)
            {
				screamed = true;

				if (Vector3.Distance(player.position, transform.position) <= 30)
				{
					gc.audioDevice.PlayOneShot(gc.bfdiScream, 0.6f);
				}
			}
		}
		else
		{
			Move();
			screamed = false;
		}
		float a = Mathf.Clamp(Vector3.Distance(player.position, transform.position) / 75, 0.3f, 1f);
		RenderSettings.ambientLight = new Color(a, a, a);
		RenderSettings.fogColor = new Color(0, 0, 0);
	}

	private void FixedUpdate()
	{
		if (moveFrames > 0f)
		{
			moveFrames -= 1;
			agent.acceleration = speed * 200;
			agent.speed = speed;
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
		if (gc.mode == "classic")
        {
			baldiWait -= 0.01f;
		}
		baldiWait -= 0.0125f;
		if (baldiWait < 0.15f)
        {
			baldiWait = 0.15f;
        }
		timeToMove = baldiWait;
		baldiAudio.PlayOneShot(slap);
		moveFrames = 2;
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
