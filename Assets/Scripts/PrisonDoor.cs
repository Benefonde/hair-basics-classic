using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PrisonDoor : MonoBehaviour
{
	public float openingDistance;

	public Transform player;

	public BaldiScript baldi;

	public MeshCollider trigger;

	public MeshCollider invisibleBarrier; 

	public AudioClip doorOpen;

	public AudioClip doorHit;

	[SerializeField]
	int clicksLeft = 128;

	public TMP_Text clicksText;

	private AudioSource myAudio;

	public AudioSource theirAudio;

	public bool playerJailed;

	Animator anim;

	bool opened;
	public bool openable;

	public PrisonItemScript[] items;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		openable = false;
		opened = false;
	}

	private void Update()
	{
		if (!openable)
        {
			gameObject.tag = "Untagged";
        }
        else
        {
			gameObject.tag = "Door";
		}

		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider == trigger) & (Vector3.Distance(player.position, base.transform.position) < openingDistance)))
		{
			if (!openable || opened)
            {
				return;
            }
			if (clicksLeft > 1)
            {
				clicksLeft--;
				clicksText.text = clicksLeft.ToString();
				myAudio.PlayOneShot(doorHit, 0.3f);
				if (Random.Range(1, 500) == 20)
                {
					StartCoroutine(ThisPrisonToHoldMe());
                }
            }
            else
            {
				OpenDoor();
				clicksText.text = string.Empty;
			}
		}
	}

	public void ItemsAreNowGoingToJail()
    {
		openable = true;
		for (int i = 0; i < 4; i++)
        {
			items[i].UpdateItemID(baldi.gc.item[i]);
			baldi.gc.LoseItem(i);
        }
    }

	public void OpenDoor(bool itemsa = false)
	{
		invisibleBarrier.gameObject.SetActive(false);
		if (!itemsa)
		{
			anim.SetTrigger("open");
		}
		else
		{
			anim.SetTrigger("itemsOpen");
		}
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Door breaks open*", 2f, Color.white, transform);
		openable = false;
		myAudio.PlayOneShot(doorOpen, 1f);
		if (baldi.isActiveAndEnabled)
		{
			baldi.Hear(transform.position, 6);
		}
	}

	IEnumerator ThisPrisonToHoldMe()
    {
		if (playerJailed)
        {
			yield break;
        }
		openable = false;
		theirAudio.Play();
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("This prison... to hold... ME?", theirAudio.clip.length, Color.white, theirAudio.transform);
		yield return new WaitForSeconds(theirAudio.clip.length);
		for (int i = 0; i < 4; i++)
        {
			items[i].transform.localPosition = new Vector3(-40, 4, items[i].transform.localPosition.z);
		}
		OpenDoor(true);
	}
}
