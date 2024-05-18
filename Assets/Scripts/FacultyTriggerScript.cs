using UnityEngine;

public class FacultyTriggerScript : MonoBehaviour
{
	public PlayerScript ps;

	private BoxCollider hitBox;

	public bool gerome;

	private void Start()
	{
		hitBox = GetComponent<BoxCollider>();
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			ps.ResetGuilt("faculty", 1f);
			if (gerome)
            {
				ps.inSecret = true;
            }
		}
	}

    private void OnTriggerExit(Collider other)
    {
        if (gerome && other.gameObject.CompareTag("Player"))
        {
			ps.inSecret = false;
        }
    }
}
