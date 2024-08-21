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
					case 0: defense += 2; health += 5; break;
					case 1: defense += 5; health += 10; break;
					case 2: defense += 3; health += 5; break;
					case 3: defense += 1; break;
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

	public void TakeDamage(int attack)
    {
		if (invTime > 0)
        {
			return;
        }
		gc.audioDevice.PlayOneShot(stabby);
		health -= attack;
		if (ss.swordType.name == "Wooden")
		{
			if (Random.Range(1, 3) == 2)
			{
				ss.durability -= 1;
			}
			invTime = 0.25f;
			disableTime = 0.35f;
			return;
		}
		invTime = 0.5f;
		disableTime = 0.5f;
		if (PlayerPrefs.GetInt("infItem", 0) == 1)
        {
			return;
        }
		ss.durability -= Mathf.RoundToInt(defense / 1.5f) + Random.Range(0, 2); // its actually range 0 to 1
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
