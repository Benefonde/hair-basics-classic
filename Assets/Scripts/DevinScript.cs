using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class DevinScript : MonoBehaviour
{
	public bool db;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	private AudioSource audioDevice;

	public AudioClip[] wander;
	public AudioClip notice;
	public AudioClip ready;
	public AudioClip[] numbers;
	public AudioClip[] outcome;
	public AudioClip pipeHit;

	public float coolDown;

	private float pipeCoolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	private int pipeDucks;
	public TMP_Text pipeText;

	public bool minigaming;

	private Animator anim;

	private Animator pipe;
	private float pipeTime;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		Wander();
		pipeCoolDown = 25;
		anim.SetBool("oh", false);
		agent.speed = 20;
	}

	private void Update()
	{
		Move();
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (pipeCoolDown > 0f)
		{
			pipeCoolDown -= 1f * Time.deltaTime;
		}
        else
        {
			anim.SetBool("oh", false);
		}
		minigaming = gc.player.pipeGame;
		if (pipeTime > 0 && minigaming)
        {
			pipeTime -= Time.deltaTime;
			if (pipeTime <= 0.1)
            {
				DuckedPipe();
				pipeTime = 0.8f;
            }
        }
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.name == "Player"))
		{
			db = true;
			if (pipeCoolDown <= 0f)
			{
				TargetPlayer();
			}
		}
		else
		{
			db = false;
			Wander();
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		if (Random.Range(1, 10) == 5 && !audioDevice.isPlaying)
        {
			audioDevice.PlayOneShot(wander[Random.Range(0, wander.Length)]);
        }
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		if (!audioDevice.isPlaying)
        {
			audioDevice.PlayOneShot(notice);
        }
	}

	private void Move()
	{
		if (gc.isActiveAndEnabled)
		{
			if ((base.transform.position == previous) & (coolDown < 0f))
			{
				Wander();
			}
		}
		previous = base.transform.position;
	}

	void StartPipeMinigame()
    {
		pipeTime = ready.length + 0.5f;
		pipeDucks = 0;
		gc.player.pipeGame = true;
		agent.Warp(new Vector3(player.position.x, transform.position.y, player.position.z));
		agent.Move(Vector3.back * 7.5f);
		agent.speed = 0;
		audioDevice.PlayOneShot(ready);
    }

	void DuckedPipe()
    {
		if (gc.player.height <= 3.3f)
		{
			audioDevice.PlayOneShot(numbers[pipeDucks]);
			pipeDucks++;
			if (pipeDucks == 5)
            {
				Invoke(nameof(EndGame), numbers[4].length - 0.5f);
            }
		}
        else
        {
			EndGame();
			pipeDucks = 0;
        }
    }

	void EndGame()
    {
		gc.player.pipeGame = false;
		gc.player.height = 4;
		pipeCoolDown = 45;
		agent.speed = 20;
		if (pipeDucks == 5)
        {
			pipeDucks = 0;
			audioDevice.PlayOneShot(outcome[0]);
        }
        else
        {
			gc.player.health -= 30;
			if (gc.player.health <= 0)
            {
				gc.camScript.character = gameObject;
            }
			audioDevice.PlayOneShot(outcome[1]);
			audioDevice.PlayOneShot(pipeHit);
			anim.SetBool("oh", true);
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && pipeCoolDown <= 0)
		{
			StartPipeMinigame();
		}
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}
}
