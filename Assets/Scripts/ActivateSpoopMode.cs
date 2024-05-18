using UnityEngine;

public class ActivateSpoopMode : MonoBehaviour
{
	public GameControllerScript gc;
	public void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "Player" && !this.gc.spoopMode)
		{
			gc.ActivateSpoopMode();
			Object.Destroy(base.gameObject);
		}
	}
}
