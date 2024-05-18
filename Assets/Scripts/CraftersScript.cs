using UnityEngine;
using UnityEngine.AI;

public class CraftersScript : MonoBehaviour
{
	public bool db;

	public bool angry;

	public bool gettingAngry;

	public float anger;

	private float forceShowTime;

	public Transform player;

	public CharacterController cc;

	public Transform playerCamera;

	public Transform baldi;

	public NavMeshAgent baldiAgent;

	public GameObject sprite;

	public GameControllerScript gc;

	[SerializeField]
	private NavMeshAgent agent;

	public Renderer craftersRenderer;

	public SpriteRenderer spriteImage;

	public Sprite angrySprite;

	public Sprite normalSprite;

	private AudioSource audioDevice;

	public AudioClip aud_Intro;

	public AudioClip aud_Loop;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		if (!TestingMode)
		{
			sprite.SetActive(value: false);
		}
	}

    private void Update()
	{
		if (forceShowTime > 0f & !TestingMode)
		{
			forceShowTime -= Time.deltaTime;
		}
		if (gettingAngry)
		{
			anger += Time.deltaTime;
			if (!TestingMode)
			{
				if ((anger >= 1f) & !angry)
				{
					angry = true;
					audioDevice.PlayOneShot(aud_Intro);
					spriteImage.sprite = angrySprite;
				}
			}
		}
		else if (anger > 0f)
		{
			anger -= Time.deltaTime / 2;
		}
		if (!angry)
		{
			if ((((base.transform.position - agent.destination).magnitude <= 20f) & ((base.transform.position - player.position).magnitude >= 60f)) || forceShowTime > 0f)
			{
				sprite.SetActive(value: true);
			}
			else
			{
				sprite.SetActive(value: false);
			}
			return;
		}
		agent.speed += 2 * Time.deltaTime;
		TargetPlayer();
		audioDevice.pitch += Time.deltaTime / 180;
		if (!audioDevice.isPlaying)
		{
			audioDevice.PlayOneShot(aud_Loop);
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player") & craftersRenderer.isVisible & sprite.activeSelf & ((base.transform.position - player.position).magnitude <= distanceForAnger))
		{
			gettingAngry = true;
		}
		else
		{
			gettingAngry = false;
		}
	}

	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!angry && agent.isActiveAndEnabled)
		{
			agent.SetDestination(location);
			if (flee)
			{
				forceShowTime = 3f;
			}
		}
	}

	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") & angry)
		{
			cc.enabled = false;
			player.position = new Vector3(5f, player.position.y, 45f);
			baldiAgent.Warp(new Vector3(5f, baldi.position.y, 165f));
			if (Time.timeScale > 1)
            {
				Time.timeScale = 1;
            }
			if (TestingMode)
            {
				forceShowTime = 2763;
            }
			player.LookAt(new Vector3(baldi.position.x, player.position.y, baldi.position.z));
			if (gc.mode == "endless")
            {
				gc.LoseNotebooks(7, 1.08f);
            }
			angry = false;
			agent.speed = 20;
			audioDevice.Stop();
			anger = 0;
			spriteImage.sprite = normalSprite;
			audioDevice.pitch = 1;
			cc.enabled = true;
			gc.playerScript.health -= 60;
			gc.DespawnCrafters();
		}
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}

	public bool TestingMode;

	public int distanceForAnger;
}
