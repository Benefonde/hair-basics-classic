using UnityEngine;
using UnityEngine.AI;

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

	private AudioSource myAudio;

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
			if (clicksLeft >= 1)
            {
				clicksLeft--;
				myAudio.PlayOneShot(doorHit, 0.3f);
            }
            else
            {
				OpenDoor();
				myAudio.PlayOneShot(doorOpen, 1f);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Door breaks open*", 2f, Color.white, transform);
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

	public void OpenDoor()
	{
		invisibleBarrier.gameObject.SetActive(false);
		anim.SetTrigger("open");
		baldi.Hear(transform.position, 6);
		openable = false;
	}
}
