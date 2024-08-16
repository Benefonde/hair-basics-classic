using UnityEngine;
using UnityEngine.AI;

public class MikoScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
		if ((other.transform.name == "BSODA_Spray(Clone)" && (baldiAudio.time > 2 || !baldiAudio.isPlaying)) && !YellowFace)
		{
			baldiAudio.PlayOneShot(stopIt);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("stop it", 1.5f, Color.gray, transform);
		}
		if (other.transform.name == "Yellow Face" && !YellowFace)
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
		if (YellowFace && other.transform.name == "Alger")
        {
			Destroy(gameObject);
        }
	}
    private void Start()
	{
		if (!YellowFace)
		{
			baldiAudio = GetComponent<AudioSource>();
		}
        else
        {
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Wonderful noise*", Mathf.Infinity, Color.yellow, transform);
		}
		agent = GetComponent<NavMeshAgent>();
		this.TargetPlayer();
		head.SetTrigger("notice");
	}

	private void Update()
	{
		if (disableTime < 0)
		{
			Move();
		}
        else
        {
			disableTime -= Time.deltaTime;
        }
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (!YellowFace)
        {
			if (PlayerPrefs.GetInt("slowerKrillers", 0) == 1)
			{
				speed = baldiAnger * 1f;
			}
            else
			{
				speed = baldiAnger * 1.3f;
			}
		}
	}

	private void FixedUpdate()
	{
		if (disableTime < 0)
		{
			agent.speed = speed;
		}
        else
        {
			agent.speed = 0;
        }
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
		{
			if ((!db && (baldiAudio.time > 2 || !baldiAudio.isPlaying)) && (!YellowFace))
			{
				int rng = Mathf.RoundToInt(Random.Range(0, found.Length));
				baldiAudio.PlayOneShot(found[rng]);
				if (rng == 0)
                {
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("I have found you", 1.5f, Color.gray, transform);
				}
				if (rng == 1)
				{
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("You have been found", 1.5f, Color.gray, transform);
				}
				if (rng == 2)
				{
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("There you are!", 1.5f, Color.gray, transform);
				}
			}
			db = true;
			if ((YellowFace && gc.mode == "speedy") || (!YellowFace))
			{
				TargetPlayer();
			}
		}
		else
		{
			if ((db & (baldiAudio.time > 2 | !baldiAudio.isPlaying)) && !YellowFace)
            {
				int rng = Mathf.RoundToInt(Random.Range(0, 3));
				if (rng == 2 && (baldiAudio.time > 2 || !baldiAudio.isPlaying))
                {
					baldiAudio.PlayOneShot(no);
					FindObjectOfType<SubtitleManager>().Add3DSubtitle("NOOOOoo", 2f, Color.gray, transform);
				}
            }
			db = false;
		}
	}

	private void Wander()
	{
		if (YellowFace)
        {
			speed = 40;
			if (gc.mode == "speedy")
            {
				speed += gc.playerScript.walkSpeed - 40;
            }
        }
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
		int rng = Mathf.RoundToInt(Random.Range(1, 15));
		if ((rng == 7 & (baldiAudio.time > 2 | !baldiAudio.isPlaying)) && !YellowFace)
        {
			rng = Mathf.RoundToInt(Random.Range(0, speech.Length));
			baldiAudio.PlayOneShot(speech[rng]);
			if (rng == 0)
            {
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("I have not found you", 1.5f, Color.gray, transform);
			}
			if (rng == 1)
			{
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("I am looking for you", 1.5f, Color.gray, transform);
			}
			if (rng == 2)
			{
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("I am trying to find you", 1.5f, Color.gray, transform);
			}
		}
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		currentPriority = 10f;
	}

	private void Move()
	{
		if (timeToMove < 0)
        {
			if ((base.transform.position == previous) & (coolDown < 0f))
			{
				Wander();
			}
			previous = base.transform.position;
		}
        else
        {
			timeToMove -= 1 * Time.deltaTime;
        }
	}

	public void GetAngry(float value)
	{
		baldiAnger += value;
		if (baldiAnger < 0.5f)
		{
			baldiAnger = 0.5f;
		}
	}

	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!antiHearing && priority >= currentPriority)
		{
			agent.SetDestination(soundLocation);
			currentPriority = priority;
			head.SetTrigger("notice");
		}
		else
		{
			head.SetTrigger("confused");
		}
	}

	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearing = true;
		antiHearingTime = t;
		if ((baldiAudio.time > 2 || !baldiAudio.isPlaying) && !YellowFace)
		{
			baldiAudio.PlayOneShot(no);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("NOOOOoo...", 2, Color.white, transform);
		}
	}

    public bool db;

	public GameControllerScript gc;

	public bool YellowFace;

	public float baseTime;

	public float speed;

	public float timeToMove;

	public float baldiAnger;

	public float baldiSpeedScale;

	private float currentPriority;

	public bool antiHearing;

	public float antiHearingTime;

	public float vibrationDistance;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public AudioSource baldiAudio;

	public AudioClip[] speech;
	public AudioClip[] found;
	public AudioClip no;
	public AudioClip stopIt;

	public float coolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public Animator head;

	public float disableTime;
}
