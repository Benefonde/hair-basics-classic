using UnityEngine;

public class AlarmClockScript : MonoBehaviour
{
	public float timeLeft = 30;

	private float lifeSpan = 35;

	private bool rang;

	public BaldiScript baldi;
	public MikoScript miko;
	public AlgerScript alger;
	public GameObject yellowFace;

	public MikoScript yellow;

	public AudioClip ring;

	public AudioSource audioDevice;

	private void Start()
	{
		timeLeft = 18f;
		lifeSpan = 21f;
		yellow = yellowFace.GetComponent<MikoScript>();
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("*Ticking*", timeLeft, Color.white, transform);
	}

	private void Update()
	{
		if (timeLeft >= -5f)
		{
			timeLeft -= Time.deltaTime;
			lifeSpan -= Time.deltaTime;
		}
		if (!rang && timeLeft <= 0)
		{
			Alarm();
		}
		if (lifeSpan <= 0)
		{
			Object.Destroy(gameObject);
		}
	}

	private void Alarm()
	{
		print("alrm");
		rang = true;
		if (baldi.isActiveAndEnabled && baldi != null)
		{
			baldi.Hear(base.transform.position, 8f);
		}
		if (miko.isActiveAndEnabled && miko != null)
		{
			miko.Hear(base.transform.position, 8f);
		}
		if (alger.isActiveAndEnabled && alger != null)
        {
			alger.Hear(transform.position, 9f);
        }
		if (yellow.isActiveAndEnabled && yellow != null)
		{
			yellow.Hear(base.transform.position, 8f);
			yellow.speed *= 4;
		}
		audioDevice.clip = null;
		audioDevice.Stop();
		audioDevice.PlayOneShot(ring);
		baldi.gc.camScript.ShakeNow(new Vector3(0.1f, 0.1f, 0.1f), 70);
		FindObjectOfType<SubtitleManager>().Add3DSubtitle("*CAR HORN ALARM!!!*", 5, Color.white, transform);
	}
}
