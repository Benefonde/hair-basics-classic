using UnityEngine;
using UnityEngine.AI;

public class SweepScript : MonoBehaviour
{
	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	public float waitTime;

	public int wanders;

	public bool active;

	private Vector3 origin;

	public GameControllerScript gc;

	public AudioClip aud_Sweep;

	public AudioClip aud_Intro;

	private NavMeshAgent agent;

	private AudioSource audioDevice;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		origin = base.transform.position;
		waitTime = Random.Range(30f, 90f);
	}

	private void Update()
	{
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (waitTime > 0f)
		{
			waitTime -= Time.deltaTime;
		}
		else if (!active)
		{
			active = true;
			GetComponent<CapsuleCollider>().enabled = true;
			wanders = 0;
			Wander();
			audioDevice.PlayOneShot(aud_Intro);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Let's blast through with Sonic speed!", aud_Intro.length, Color.blue, transform);
		}
	}

	private void FixedUpdate()
	{
		if (((double)agent.velocity.magnitude <= 0.1) & (coolDown <= 0f) & (wanders < 10) & active)
		{
			Wander();
		}
		else if (wanders >= 10)
		{
			GoHome();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		wanders++;
	}

	private void GoHome()
	{
		agent.SetDestination(origin);
		waitTime = Random.Range(120f, 180f);
		wanders = 0;
		active = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
		{
			GoHome();
			GetComponent<CapsuleCollider>().enabled = false;
		}
		if (other.tag == "NPC" || other.tag == "Player")
		{
			audioDevice.PlayOneShot(aud_Sweep);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Blast away!", aud_Sweep.length, Color.blue, transform);
		}
		if (other.transform.name == "Yellow Face")
		{
			audioDevice.Stop();
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}
}
