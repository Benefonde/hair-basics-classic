using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UrkScript : MonoBehaviour
{
	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public AudioSource audioDevice;
	private AudioSource audMusic;

	public float coolDown;
	public float seeCooldown;

	private Vector3 previous;

	public NavMeshAgent agent;

	public GameControllerScript gc;

	bool chase;

	public float urkTimer;
	public float urkTimerMax = 16.85f;
	public Slider urkSTimer;

	bool wandering;

	public AudioClip[] music;
	// 0-6 randoms
	// 7 - chasey
	// 8 - i see you
	// 9 - see loop
	// 10 - must've been the wind
	// 11 - OW

	float speed;

	void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		audMusic = gameObject.AddComponent<AudioSource>();
		audMusic.spatialBlend = 1;
		audMusic.rolloffMode = AudioRolloffMode.Linear;
		audMusic.minDistance = 50;
		audMusic.maxDistance = 250;
		agent = GetComponent<NavMeshAgent>();
		speed = agent.speed;
		Wander();
		urkSTimer.maxValue = urkTimerMax;
	}

	private void Update()
	{
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (wandering && !audMusic.isPlaying && !AudioListener.pause)
        {
			audMusic.clip = music[Random.Range(0, 7)];
			audMusic.Play();
        }
		if (chase && !gc.camScript.FuckingDead)
        {
			gc.tc.urkTime += Time.deltaTime;
        }
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		if (chase)
        {
			TargetPlayer();
			agent.speed = gc.player.runSpeed + 2;
			return;
        }
		if ((Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.name == "Player")) && (seeCooldown == 1.75f || seeCooldown <= 0))
		{
			wandering = false;
			if (seeCooldown <= 0)
            {
				audMusic.Stop();
				audioDevice.PlayOneShot(music[8]);
				audioDevice.loop = true;
				audioDevice.clip = music[9];
				audioDevice.Play();
			}
			seeCooldown = 1.75f;
			agent.speed = 0;
			if (urkTimer < urkTimerMax)
            {
				urkTimer += Time.deltaTime;
				urkSTimer.gameObject.SetActive(true);
				urkSTimer.value = urkTimer;
			}
            else
            {
				StartChase();
            }
		}
		else
		{
			urkSTimer.gameObject.SetActive(false);
			db = false;
			if (seeCooldown == 1.75f)
			{
				audioDevice.Stop();
				audioDevice.PlayOneShot(music[10]);
			}
			if (seeCooldown > 0)
            {
				seeCooldown -= Time.deltaTime;
				return;
            }
			if ((agent.velocity.magnitude <= 1f) & (coolDown <= 0f))
			{
				Wander();
			}
		}
	}

	private void Wander()
	{
		wandering = true;
		agent.speed = speed;
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}

	public void StartChase()
	{
		if (audMusic != null)
		{
			audMusic.Stop();
		}
		wandering = false;
		chase = true;
		agent.speed = gc.player.runSpeed + 2;
		audioDevice.loop = true;
		audioDevice.spatialBlend = 0;
		audioDevice.clip = music[7];
		audioDevice.Play();
		urkSTimer.gameObject.SetActive(true);
	}

	public void StopChase()
	{
		wandering = true;
		chase = false;
		gc.tc.urkTime = 0;
		agent.speed = 15;
		audioDevice.loop = true;
		audioDevice.Stop();
		urkSTimer.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
		{
			if (!chase)
			{
				seeCooldown = 5;
				urkTimer /= 1.5f;
			}
            else
			{
				urkTimer -= 4f;
				urkSTimer.value = urkTimer;
				if (urkTimer <= 0)
                {
					StopChase();
                }
			}
		}
		if (other.tag == "Player")
        {
			if (chase && gc.player.health > 0)
            {
				gc.player.health = 0;
				audioDevice.PlayOneShot(music[11]);
				gc.player.gonnaBeKriller = transform;
            }
        }
		if (other.transform.name == "Yellow Face") // should npc TIE to yellowey?
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}

    private void OnTriggerStay(Collider other)
    {
		if (other.transform.name == "UbrSpray(Clone)" || other.transform.name == "Objection(Clone)")
		{
			if (chase)
			{
				urkTimer -= 0.1f * Time.deltaTime;
				urkSTimer.value = urkTimer;
				if (urkTimer <= 0)
				{
					StopChase();
				}
			}
		}
	}
}
