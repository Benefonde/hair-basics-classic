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

	public float pipeCoolDown;

	private Vector3 previous;

	private NavMeshAgent agent;

	public GameControllerScript gc;

	private int pipeDucks;
	public TMP_Text pipeText;

	public GameObject devinCanvas;

	public bool minigaming;

	bool noticed;

	private Animator anim;

	public RectTransform pipe;
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
        else if (pipeCoolDown <= 0)
        {
			anim.SetBool("oh", false);
		}
		minigaming = gc.player.pipeGame;
		if (pipeTime > 0 && minigaming)
		{
			pipeTime -= Time.deltaTime;
			if (pipeTime <= 0.05)
            {
				DuckedPipe();
				pipeTime = 0.8f;
			}
			pipe.anchoredPosition = new Vector2(pipe.anchoredPosition.x, (pipeTime / 4) * 800);
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
			noticed = false;
			if (coolDown <= 0)
			{
				Wander();
			}
		}
	}

	private void Wander()
	{
		int rng = Random.Range(0, wander.Length);
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		if (Random.Range(1, 12) == 5 && !audioDevice.isPlaying && pipeCoolDown <= 0)
        {
			anim.SetBool("oh", false);
			audioDevice.PlayOneShot(wander[rng]);
			switch (rng)
            {
				case 1: FindObjectOfType<SubtitleManager>().Add3DSubtitle("Where'd you go, Bob?", wander[rng].length, new Color(255, 165, 0), transform); break;
				case 2: FindObjectOfType<SubtitleManager>().Add3DSubtitle("Do do do do...", wander[rng].length, new Color(255, 165, 0), transform); break;
			}
        }
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		if (!audioDevice.isPlaying && !noticed)
		{
			audioDevice.PlayOneShot(notice);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Oh hey, uh, do this for me please.", notice.length, new Color(255, 165, 0), transform);
		}
		noticed = true;
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
		devinCanvas.SetActive(true);
		pipeTime = ready.length + 0.5f;
		pipeDucks = 0;
		pipeText.text = pipeDucks.ToString();
		gc.player.pipeGame = true;
		agent.speed = 0;
		audioDevice.PlayOneShot(ready);
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("Duck under this pole 5 times. Ready? Go!", ready.length, new Color(255, 165, 0), transform);
		agent.Warp(new Vector3(player.position.x, transform.position.y, player.position.z));
		agent.Move(Vector3.back * 7.5f);
	}

	void DuckedPipe()
    {
		if (gc.player.camscript.pipeGameHeight <= -0.3f)
		{
			gc.player.camscript.pipeGameHeight = 0;
			audioDevice.PlayOneShot(numbers[pipeDucks]);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle($"{pipeDucks + 1}!", numbers[pipeDucks].length, new Color(255, 165, 0), transform);
			pipeDucks++;
			pipeText.text = pipeDucks.ToString();
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
		devinCanvas.SetActive(false);
		gc.player.pipeGame = false;
		gc.player.pipeGameGravity += 8 * Time.deltaTime;
		pipeCoolDown = 45;
		agent.speed = 20;
		if (pipeDucks == 5)
        {
			pipeDucks = 0;
			audioDevice.PlayOneShot(outcome[0]);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Good job, here's a thing I guess.", outcome[0].length, new Color(255, 165, 0), transform);
			if (gc.HasItemInInventory(0))
			{
				gc.CollectItem(gc.CollectItemExcluding(3, 8, 13, 14, 15, 16, 21, 24));
			}
            else
            {
				gc.player.health += 20;
				gc.player.stamina += 50;
			}
		}
        else
        {
			gc.player.health -= 30;
			gc.camScript.ShakeNow(new Vector3(1, 0.5f, 1), 5);
			if (gc.player.health <= 0)
            {
				gc.camScript.follow = transform;
            }
            else
            {
				gc.tc.devinPipeHit++;
            }
			audioDevice.PlayOneShot(outcome[1]);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("Oops, you got hit, see you later.", outcome[1].length, new Color(255, 165, 0), transform);
			audioDevice.PlayOneShot(pipeHit);
			anim.SetBool("oh", true);
        }
    }

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && pipeCoolDown <= 0 && !minigaming)
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
