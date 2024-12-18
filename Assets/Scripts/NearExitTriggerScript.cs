using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	public GameControllerScript gc;

	public EntranceScript es;

	public GameObject bossController;
	public BoxCollider blockage;

	public Transform evilPlayer;

	public bool algerExit;

    private void Update()
    {
		if (evilPlayer.gameObject.activeSelf)
		{
			if ((gc.exitsReached < gc.amountOfExit) && gc.finaleMode && Vector3.Distance(transform.position, evilPlayer.position) <= 10.5f && transform.position.y == 3)
			{
				gc.ExitReached();
				es.Lower();
				if (gc.baldiPlayerScript.isActiveAndEnabled)
				{
					gc.baldiPlayerScript.Hear(base.transform.position);
				}
			}
		}
	}

    private void OnTriggerEnter(Collider other)
	{
		if (gc.mode == "panino" && gc.exitsReached == 4)
        {
			gc.baldiPlayerScript.Die();
        }
		if ((gc.exitsReached < gc.amountOfExit) & gc.finaleMode & (other.tag == "Player" || other.name == "Player (EVIL)"))
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
			if (gc.algerScript.isActiveAndEnabled)
			{
				gc.algerScript.Hear(base.transform.position, 8f);
			}
			if (gc.baldiPlayerScript.isActiveAndEnabled)
			{
				gc.baldiPlayerScript.Hear(base.transform.position);
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
