using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AlgerNullScript : MonoBehaviour
{
	private void Start()
	{
		baldiAudio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		this.TargetPlayer();
	}

	private void Update()
	{
		if (!bossMode)
		{
			Move();
			if (coolDown > 0f)
			{
				coolDown -= 1f * Time.deltaTime;
			}
		}
	}

	private void FixedUpdate()
	{
		agent.speed = speed;
		Vector3 direction = player.position - base.transform.position;
		if (!disableWanderOrTarget)
		{
				if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
				{
					if ((!db && (baldiAudio.time > 2 || !baldiAudio.isPlaying)))
					{
						int rng = Mathf.RoundToInt(Random.Range(0, found.Length));
						baldiAudio.PlayOneShot(found[rng]);
						if (rng == 0)
                        {
							FindObjectOfType<SubtitleManager>().Add3DSubtitle("Don't worry, I have some Zesty Lays for you!", found[rng].length, Color.blue, transform);
						}
						if (rng == 1)
                        {
							FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hi, do you wanna die?", found[1].length, Color.blue, transform);
						}
					}
					db = true;
					TargetPlayer();
				}
				else
				{
					db = false;
				}
		}
		if (bossMode)
		{
			TargetPlayer();
		}

		if (gc.exitsReached == 4 && !bc.activeSelf)
		{
			disableWanderOrTarget = true;
			agent.SetDestination(new Vector3(10, 2, 160));
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
		int rng = Mathf.RoundToInt(Random.Range(2, 8));
		if ((rng == 7 & (baldiAudio.time > 2 | !baldiAudio.isPlaying)))
		{
			rng = Mathf.RoundToInt(Random.Range(0, speech.Length));
			baldiAudio.PlayOneShot(speech[rng]);
			switch (rng)
            {
				case 0: FindObjectOfType<SubtitleManager>().Add3DSubtitle("Come out wherever you are!", speech[0].length, Color.blue, transform); break;
				case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("Let me krill you, please?", speech[1].length, Color.blue, transform); break;
				case 2: FindObjectOfType<SubtitleManager>().Add3DSubtitle("RAAAAAHHHHHH", speech[2].length, Color.blue, transform); break;
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
		if (gc.exitsReached != 4)
		{
			if (PlayerPrefs.GetInt("slowerKrillers", 0) == 1)
			{
				speed = (gc.notebooks + anger) * 2f;
			}
            else
			{
				speed = (gc.notebooks + anger * 1.5f) * 2.65f;
			}
		}
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

	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!antiHearing && priority >= currentPriority)
		{
			agent.SetDestination(soundLocation);
			currentPriority = priority;
		}
	}

	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearing = true;
		antiHearingTime = t;
	}

    private void OnTriggerEnter(Collider other)
    {
		if (other.transform.name == "UbrSpray(Clone)")
		{
			anger += 5;
		}
		if (other.transform.name == "HitObject(Clone)")
        {
			if (other.GetComponent<HitObjectBossScript>().state == 2 && canGetHit)
			{
				Destroy(other);
				StartCoroutine(GetHit());
			}
        }
    }

	public IEnumerator GetHit()
	{
		pauseTime += ow.length + 0.25f;
		string[] thing = { "Ow, ok, if you want REAL null bossfight im gonna null bossfight you then!", "AAAAAAAAAAAAAAAAA" };
		float[] thingie = { 4.9f, 0.949f };
		Color[] thingest = { Color.blue, Color.blue };
		canGetHit = true;
		if (health == 20)
		{
			FindObjectOfType<SubtitleManager>().StopChainedSubtitles();
			canGetHit = false;
		}
		baldiAudio.Stop();
		gc.camScript.ShakeNow(new Vector3(1f, 0.9f, 1f), 10);
		gc.playerScript.runSpeed += 1.3f;
		gc.playerScript.walkSpeed = gc.playerScript.runSpeed;
		speed = 0;
		baldiAudio.PlayOneShot(ow);
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("owie", ow.length, Color.blue, transform);
		if (bcs.lastSongOn == 2763)
        {
			bcs.lastSongOn = 0;
        }
		bcs.musicAud[bcs.lastSongOn].Tempo = 0.001f;
		health -= 1;
		while (pauseTime > 0)
        {
			pauseTime -= Time.deltaTime;
			yield return null;
		}
		bcs.musicAud[bcs.lastSongOn].Tempo = healthTimesThings[19 - health];
		if (healthTimesThings[19 - health] == 0.0001f)
        {
			bcs.musicAud[5].gameObject.SetActive(true);
        }
		switch (health)
		{
			case 19: StartCoroutine(bcs.ChangeMusic(1)); break;
			case 10: StartCoroutine(bcs.ChangeMusic(3)); break;
			case 4: StartCoroutine(bcs.ChangeMusic(4)); break;
		}
		speed = (70 - (health * 2.65f));
		if (PlayerPrefs.GetInt("slowerKrillers", 0) == 1)
		{
			speed = speed = (70 - (health * 2.3f));
		}

		if (health == 19)
        {
			disableWanderOrTarget = true;
			speed = 0;
			canGetHit = false;
			BossObjectManager bom = FindObjectOfType<BossObjectManager>();
			if (bom.starting[0] != null)
			{
				Destroy(bom.starting[0]);
			}
			if (bom.starting[1] != null)
			{
				Destroy(bom.starting[1]);
			}
			if (bom.starting[2] != null)
			{
				Destroy(bom.starting[2]);
			}
			player.GetComponent<PlayerScript>().holdingObject = false;
			baldiAudio.clip = preBoss[1];
			FindObjectOfType<SubtitleManager>().AddChained3DSubtitle(thing, thingie, thingest, transform);
			baldiAudio.Play();
			gc.camScript.follow = transform;
			gc.camScript.FuckingDead = true;
			yield return new WaitForSeconds(preBoss[1].length - 1);
			speed = (75 - (health * 3.25f));
			canGetHit = true;
			gc.camScript.FuckingDead = false;
			gc.debugMode = false;
			bossMode = true;
			disableWanderOrTarget = true;
		}
    }

    public bool db;

	public GameControllerScript gc;

	public float baseTime;

	public bool disableWanderOrTarget;

	public float speed;

	public float timeToMove;

	public float baldiAnger;

	public float baldiSpeedScale;

	private float currentPriority;

	public bool antiHearing;

	public float antiHearingTime;

	public bool canGetHit;

	public float pauseTime;

	public int health;

	public Transform player;

	public Transform wanderTarget;

	public GameObject bc;

	public AILocationSelectorScript wanderer;

	public BossControllerScript bcs;

	public AudioSource baldiAudio;

	public float anger;

	public AudioClip[] speech;
	public AudioClip[] found;
	public AudioClip[] preBoss;
	public AudioClip ow;

	public float coolDown;

	private Vector3 previous;

	public NavMeshAgent agent;

	public bool bossMode;

	float[] healthTimesThings = new float[]
	{
		0.6f,
		0.62f,
		0.64f,
		0.66f,
		0.68f,
		0.73f,
		0.78f,
		0.83f,
		0.88f,
		0.91f,
		0.94f,
		0.97f,
		1.02f,
		1.15f,
		1.22f,
		1.29f,
		1.44f,
		1.6f,
		0.0001f,
		0.0001f
	};
}
