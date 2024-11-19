using UnityEngine;
using UnityEngine.AI;

public class DoorScript : MonoBehaviour
{
	public float openingDistance;

	public Transform player;

	public BaldiScript baldi;
	public MikoScript miko;
	public AlgerScript alger;
	public BaldiPlayerScript baldiPlayer;

	public MeshCollider barrier;
	public NavMeshObstacle locked;

	public MeshCollider trigger;

	public MeshCollider invisibleBarrier;

	public MeshRenderer inside;

	public MeshRenderer outside;

	public AudioClip doorOpen;

	public AudioClip doorClose;

	public Material closed;

	public Material open;

	public Material lockd;

	private bool bDoorOpen;

	private bool bDoorLocked;

	public int silentOpens;

	private float openTime;

	public float lockTime;
	public float detentionTime;

	private AudioSource myAudio;

	public bool johnDoor;

	public bool DoorLocked => bDoorLocked;

	private void Start()
	{
		myAudio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (johnDoor && baldi.gc.notebooks < 17)
        {
			lockTime = 1;
			openTime = 0;
			locked.enabled = true;
			bDoorLocked = true;
		}
		if (lockTime > 0f)
		{
			lockTime -= 1f * Time.deltaTime;
			inside.material = lockd;
			outside.material = lockd;
		}
		else if (bDoorLocked)
		{
			UnlockDoor();
		}
		if (detentionTime > 0)
		{
			detentionTime -= 1f * Time.deltaTime;
		}
		if (openTime > 0f)
		{
			openTime -= 1f * Time.deltaTime;
		}
		if ((openTime <= 0f) & bDoorOpen)
		{
			barrier.enabled = true;
			invisibleBarrier.enabled = true;
			bDoorOpen = false;
			inside.material = closed;
			outside.material = closed;
			if (silentOpens <= 0)
			{
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("*SLAM*", 0.5f, Color.white, transform);
				myAudio.PlayOneShot(doorClose, 1f);
			}
		}
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) && ((hitInfo.collider == trigger) & (Vector3.Distance(player.position, base.transform.position) < openingDistance)))
		{
			if (!bDoorLocked)
            {
				if (baldi.isActiveAndEnabled & (silentOpens <= 0))
				{
					baldi.Hear(base.transform.position, 1f);
				}
				if ((miko.isActiveAndEnabled) & (silentOpens <= 0))
				{
					miko.Hear(base.transform.position, 3f);
				}
				if (alger.isActiveAndEnabled & (silentOpens <= 0))
				{
					alger.Hear(base.transform.position, 1f);
				}
				if (baldiPlayer.isActiveAndEnabled & (silentOpens <= 0))
				{
					baldiPlayer.Hear(base.transform.position);
				}
				OpenDoor();
				if (silentOpens > 0)
				{
					silentOpens--;
				}
			}
            else
            {
				myAudio.PlayOneShot(rattle);
				FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Rattling*", rattle.length, Color.white, transform);

			}
		}
	}

	public void OpenDoor()
	{
		if (silentOpens <= 0 && !bDoorOpen)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Door opens*", 0.6f, Color.white, transform);
		}
		barrier.enabled = false;
		invisibleBarrier.enabled = false;
		bDoorOpen = true;
		inside.material = open;
		outside.material = open;
		openTime = 3f;
	}

	private void OnTriggerStay(Collider other)
	{
		if (!bDoorLocked & other.CompareTag("NPC"))
		{
			OpenDoor();
		}
	}

	public void LockDoor(float time)
	{
		if (!bDoorLocked)
		{
			baldi.gc.audioDevice.PlayOneShot(baldi.gc.aud_Lock);
			FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Lock!*", baldi.gc.aud_Lock.length, Color.white, transform);
			lockTime = time;
			openTime = 0;
			locked.enabled = true;
			bDoorLocked = true;
		}
	}

	public void UnlockDoor()
	{
		bDoorLocked = false;
		lockTime = 0;
		locked.enabled = false;
		outside.material = closed;
		inside.material = closed;
		myAudio.PlayOneShot(baldi.gc.aud_Unlock);
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Unlock!*", baldi.gc.aud_Unlock.length, Color.white, transform);
	}

	public void SilenceDoor()
	{
		silentOpens = 4;
	}

	public AudioClip rattle;
}
