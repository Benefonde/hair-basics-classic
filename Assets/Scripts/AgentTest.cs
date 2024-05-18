using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
	public bool db;

	public bool touchingPlayer;

	public GameObject player;
	public PlayerScript playerScript;

	public GameObject baldi;

	public float coolDown;

	private NavMeshAgent agent;

	public Animator animator;

	public int clickCount;

	public GameControllerScript gc;

	public float disableTime = 0f;

	public int clickery;

	public float normalSpeed;
	public float speedupAmount;

	public AILocationSelectorScript wanderer;

	public Transform wanderTarget;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		TargetPlayer();
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("balls", 2147483647, Color.blue, transform);
	}

	private void Update()
	{
		animator.speed = agent.speed / 56;
		if (disableTime > 0)
		{
			disableTime -= 1f * Time.deltaTime;
			base.transform.position = new Vector3(base.transform.position.x, -70, base.transform.position.z);
			return;
		}
		if (disableTime < 0.01 && base.transform.position.y == -70)
        {
			base.transform.position = new Vector3(base.transform.position.x, 1.65f, base.transform.position.z);
		}
		if (touchingPlayer && clickCount == 0)
		{
			int cooldownNOW = Mathf.RoundToInt(Random.Range(20, 30));
			disableTime = cooldownNOW;
		}
		else if (touchingPlayer && clickCount > 0)
		{
			GoToBald();
			gc.ClickOutNow();
			playerScript.health -= 0.7f * (8 * Time.deltaTime);
			return;
		}
		TargetPlayer();
	}

	private void FixedUpdate()
	{
		Vector3 direction = this.player.transform.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.name == "Player"))
		{
			db = true;
		}
		else
		{
			db = false;
		}
	}

	private void TargetPlayer()
	{
		if (touchingPlayer && agent.destination == player.transform.position && clickCount > 0)
        {
			GoToBald();
			gc.ClickOutNow();
			return;
        }
		if (playerScript.TouchingNavMesh(player.transform.position, 5))
		{
			agent.SetDestination(player.transform.position);
		}
        else
        {
			Wander();
        }
		coolDown = 1f;
		if (db && !gc.gamePaused)
		{
			agent.speed += speedupAmount * Time.deltaTime;
		}
		else if (!db)
        {
			agent.speed = normalSpeed;
        }
	}

	private void Wander()
	{
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
	}

	private void GoToBald()
    {
		if (gc.mode != "free")
        {
			agent.SetDestination(baldi.transform.position);
		}
        else
        {
			agent.SetDestination(gc.tutorBaldi.transform.position);
		}
		agent.speed = 30;
		player.transform.position = new Vector3(base.transform.position.x, 6, base.transform.position.z);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "Player")
        {
			if (!touchingPlayer  || !gc.playerScript.bootsActive)
			{
				clickery = Mathf.RoundToInt(Vector3.Distance(player.transform.position, baldi.transform.position) / 5.5f);
				clickCount = clickery;
				touchingPlayer = true;
			}
			else if (gc.playerScript.bootsActive)
            {
				touchingPlayer = false;	
				disableTime = 10;
				clickCount = 0;
            }
        }
		if (other.transform.name == "Yellow Face")
		{
			gc.SomeoneTied(gameObject);
			gameObject.SetActive(false);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Player" && touchingPlayer)
		{
			touchingPlayer = false;
		}
	}
}
