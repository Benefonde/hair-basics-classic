using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    private void Start()
	{
		aud = GetComponent<AudioSource>();
		zombieSprite = transform.Find("ZombieSprite").GetComponent<SpriteRenderer>();
		agent = GetComponent<NavMeshAgent>();
		wanderer.GetNewTargetHallway();
		agent.Warp(wanderTarget.position);
		aud.PlayOneShot(idle[Random.Range(0, idle.Length)]);
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Zombie groans*", 1.5f, Color.green, transform);
		zombieSpeed = Random.Range(5, 20);
		for (int i = 0; i < 4; i++)
		{
			if (Random.Range(0, 24 / gc.notebooks + 1) <= 0.2f)
			{
				armor[i].SetActive(true);
				switch (i)
                {
					case 0: defense += 4; health += 5; break;
					case 1: defense += 7; health += 5; break;
					case 2: defense += 5; health += 5; break;
					case 3: defense += 2; break;
				}
			}
		}
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

		zombieSprite.sprite = zombieNormal;
		if (invTime > 0)
        {
			zombieSprite.sprite = zombieHurt;
			invTime -= Time.deltaTime;
        }

		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (PlayerPrefs.GetInt("slowerKrillers", 0) == 1)
		{
			speed = zombieSpeed * 0.7f;
		}
		else
		{
			speed = zombieSpeed * 1f;
		}

		if (health <= 0)
        {
			Destroy(gameObject);
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
		if (!gc.finaleMode)
		{
			if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
			{
				db = true;
				TargetPlayer();
				coolDown = 1;
			}
			else
			{
				if (coolDown <= 0)
				{
					db = false;
					Wander();
					coolDown = 1;
				}
			}
		}
        else
        {
			TargetPlayer();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
		if (Random.Range(0, 24) == 4 && !aud.isPlaying)
        {
			aud.PlayOneShot(idle[Random.Range(0, idle.Length)]);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Zombie groans*", 1.5f, Color.green, transform);
		}
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		currentPriority = 1f;
	}

	private void Move()
	{
		if ((transform.position == previous) & (coolDown < 0f))
		{
			Wander();
		}
		previous = transform.position;
	}

	public void TakeDamage(int attack, bool yellow = false)
    {
		if (invTime > 0)
        {
			return;
        }
		if (!yellow)
		{
			gc.audioDevice.PlayOneShot(stabby);
		}
        else
        {
			aud.PlayOneShot(stabby);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Zombie gets hurt*", stabby.length, Color.green, transform);
		}
		health -= attack - Mathf.RoundToInt(defense / 1.5f);
		invTime = 0.35f;
		disableTime = 0.36f;
		if (yellow)
        {
			disableTime += 0.11f;
			invTime += 0.1f;
        }
		if (PlayerPrefs.GetInt("infItem", 0) == 1 || yellow)
        {
			return;
        }
		ss.durability -= 1 + Mathf.RoundToInt(defense / 2.25f);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Yellow Face")
        {
			TakeDamage(10, true);
        }
    }

    public bool db;

	public GameControllerScript gc;

	public SwordScript ss;

	public float speed;

	public GameObject[] armor = new GameObject[4];

	float zombieSpeed;

	SpriteRenderer zombieSprite;
	public Sprite zombieHurt;
	public Sprite zombieNormal;

	private float currentPriority;

	public float defense = 1;

	int health = 35;

	float invTime;

	public Transform player;

	public AudioClip stabby;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float coolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public float disableTime;

	AudioSource aud;
	public AudioClip[] idle;
}
