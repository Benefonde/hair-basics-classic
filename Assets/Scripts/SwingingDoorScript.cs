using UnityEngine;

public class SwingingDoorScript : MonoBehaviour
{
	public bool heardDoor;

	public GameControllerScript gc;

	public BaldiScript baldi;
	public MikoScript miko;
	public AlgerScript alger;

	public MeshCollider barrier;

	public GameObject obstacle;

	public MeshCollider trigger;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public Material closed;

	public Material open;

	public Material locked;

	public AudioClip doorOpen;

	public AudioClip baldiDoor;

	[SerializeField]
	private float openTime;

	private float lockTime;

	public bool bDoorOpen;

	public bool bDoorLocked;

	public bool checkIfObstacle;

	private AudioSource myAudio;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
		UnlockDoor();
	}

	private void Update()
	{
		if (openTime > 0f)
		{
			openTime -= 1f * Time.deltaTime;
		}
		if (lockTime > 0f)
		{
			lockTime -= Time.deltaTime;
		}
		else if (bDoorLocked)
		{
			UnlockDoor();
		}
		if ((openTime <= 0f) & bDoorOpen & !bDoorLocked)
		{
			heardDoor = false;
			bDoorOpen = false;
			inside.material = closed;
			outside.material = closed;
		}
		if (gc.mode == "classic" && gc.notebooks < 2)
		{
			checkIfObstacle = true;
		}
		if (checkIfObstacle)
        {
			if (gc.notebooks == 2)
			{
				obstacle.SetActive(false);
				checkIfObstacle = false;
			}
            else
            {
				obstacle.SetActive(true);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if ((other.tag == "Player" || (other.tag == "NPC" && other.isTrigger)) && !bDoorLocked)
		{
			if (other.tag == "Player")
			{
				heardDoor = true;
				bDoorOpen = true;
				inside.material = open;
				outside.material = open;
				openTime = 2f;
			}
			else if (other.isTrigger)
			{
				heardDoor = true;
				bDoorOpen = true;
				inside.material = open;
				outside.material = open;
				openTime = 2f;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (gc.mode == "classic" && gc.notebooks < 2)
        {
			myAudio.PlayOneShot(baldiDoor);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("No dwaynes?", baldiDoor.length, Color.cyan, transform);
			LockDoor(5);
			return;
        }
		if (other.tag == "Player" || other.tag == "NPC" && !heardDoor && !bDoorLocked)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Swinging door opens*", doorOpen.length, Color.white, transform);
			if (other.tag == "Player")
			{
				if (miko.isActiveAndEnabled)
                {
					miko.Hear(base.transform.position, 4f);
				}
				if (baldi.isActiveAndEnabled)
                {
					baldi.Hear(base.transform.position, 1f);
				}
				if (alger.isActiveAndEnabled)
				{
					alger.Hear(base.transform.position, 2f);
				}
			}
		}
	}

	public void LockDoor(float time)
	{
		barrier.enabled = true;
		obstacle.SetActive(value: true);
		bDoorLocked = true;
		lockTime = time;
		inside.material = locked;
		outside.material = locked;
	}

	private void UnlockDoor()
	{
		barrier.enabled = false;
		obstacle.SetActive(value: false);
		bDoorLocked = false;
		inside.material = closed;
		outside.material = closed;
	}
}
