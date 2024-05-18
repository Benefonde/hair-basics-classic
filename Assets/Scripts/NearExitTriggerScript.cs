using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public EntranceScript es;

	public GameObject bossController;
	public BoxCollider blockage;

	public bool algerExit;

	private void OnTriggerEnter(Collider other)
	{
		if ((gc.exitsReached < gc.amountOfExit) & gc.finaleMode & (other.tag == "Player"))
		{
			gc.ExitReached();
			es.Lower();
			if (gc.baldiScrpt.isActiveAndEnabled)
			{
				gc.baldiScrpt.Hear(base.transform.position, 6f);
			}
			if (gc.mikoScript.isActiveAndEnabled)
            {
				gc.mikoScript.Hear(base.transform.position, 8f);
			}
		}
		if (algerExit & (other.tag == "Player"))
        {
			if (FindObjectOfType<GameControllerScript>().mode == "alger")
			{
				bossController.SetActive(true);
				blockage.enabled = true;
			}
        }
	}
}
