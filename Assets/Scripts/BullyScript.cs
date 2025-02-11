using UnityEngine;
using System.Collections;

public class BullyScript : MonoBehaviour
{
	public Transform player;

	public GameControllerScript gc;

	public Renderer bullyRenderer;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	public float waitTime;

	public float activeTime;

	public float guilt;

	public bool active;

	public bool spoken;

	private AudioSource audioDevice;

	public AudioClip[] aud_Taunts = new AudioClip[1];

	public AudioClip aud_Denied;

	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
		waitTime = Random.Range(10f, 120f);
	}

	private void Update()
	{
		if (waitTime > 0f)
		{
			waitTime -= Time.deltaTime;
		}
		else if (!active)
		{
			Activate();
		}
		if (active)
		{
			activeTime += Time.deltaTime;
			if ((activeTime >= 180f) & ((base.transform.position - player.position).magnitude >= 120f))
			{
				Reset();
			}
		}
		if (guilt > 0f)
		{
			guilt -= Time.deltaTime;
		}
	}

	private void FixedUpdate()
	{
		Vector3 direction = player.position - base.transform.position;
		Physics.Raycast(base.transform.position + new Vector3(0f, 2f, 0f), direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore);
		if (hitInfo.transform == null)
        {
			return;
        }
		if ((hitInfo.transform.tag == "Player") & ((base.transform.position - player.position).magnitude <= 30f) & active)
		{
			if (!spoken)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 0f));
				audioDevice.PlayOneShot(aud_Taunts[num]);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hello this is your former president Barack Obama", aud_Taunts[0].length, new Color(0.62f, 0.32f, 0.17f, 1f), transform);
				spoken = true;
			}
			guilt = 20f;
		}
	}

	private void Activate()
	{
		wanderer.GetNewTargetHallway();
		base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);
		while ((base.transform.position - player.position).magnitude < 20f)
		{
			wanderer.GetNewTargetHallway();
			base.transform.position = wanderTarget.position + new Vector3(0f, 5f, 0f);
		}
		active = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "UbrSpray(Clone)")
		{
			Reset();
		}
		if ((other.transform.tag != "Player"))
		{
			if (other.transform.name == "Yellow Face")
			{
				gc.SomeoneTied(gameObject);
				gameObject.SetActive(false);
			}
			return;
		}
		if (gc.item[gc.itemSelected] == 25)
		{
			gc.UndoCurse();
			gc.tc.GetTrophy(25);
			StartCoroutine(CantWaitToDye());
		}
		if (gc.item[gc.itemSelected] == 0)
		{
			audioDevice.PlayOneShot(aud_Denied);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("I don't have patience for cynicism right now.", aud_Denied.length, new Color(0.62f, 0.32f, 0.17f, 1f), transform);
			gc.player.health -= 10;
			return;
		}
		gc.LoseItem(gc.itemSelected);
		Reset();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((other.transform.name == "Principal of the Thing") & (guilt > 0f))
		{
			Reset();
		}
	}

	private void Reset()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 20f, 0f);
		waitTime = Random.Range(60f, 120f);
		active = false;
		activeTime = 0f;
		spoken = false;
	}

	IEnumerator CantWaitToDye()
    {
		yield return new WaitForSeconds(50);
		gc.SomeoneTied(gameObject, false);
		gameObject.SetActive(false);
	}
}
