using UnityEngine;

public class OldNerd : MonoBehaviour
{
	public GameControllerScript gc;

	public Transform player;

	public int ItemId;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out var hitInfo) || Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0f && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo))
		{
			if ((hitInfo.transform.name == "Pickup_EnergyFlavoredZestyBar") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(1);
			}
			else if ((hitInfo.transform.name == "Pickup_YellowDoorLock") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(2);
			}
			else if ((hitInfo.transform.name == "Pickup_Key") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(3);
			}
			else if ((hitInfo.transform.name == "Pickup_BSODA") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(4);
			}
			else if ((hitInfo.transform.name == "Pickup_Quarter") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(5);
			}
			else if ((hitInfo.transform.name == "Pickup_Tape") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(6);
			}
			else if ((hitInfo.transform.name == "Pickup_AlarmClock") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(7);
			}
			else if ((hitInfo.transform.name == "Pickup_WD-3D") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(8);
			}
			else if ((hitInfo.transform.name == "Pickup_SafetyScissors") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(9);
			}
			else if ((hitInfo.transform.name == "Pickup_BigBoots") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(10);
			}
			else if ((hitInfo.transform.name == "Pickup_CoolItem") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(11);
			}
			else if ((hitInfo.transform.name == "Pickup_Teleporter") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(12);
			}
			else if ((hitInfo.transform.name == "Pickup_AlgerRay") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(13);
			}
			else if ((hitInfo.transform.name == "Pickup_AlgerWhistle") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(14);
			}
			else if ((hitInfo.transform.name == "Pickup_AppleProduct") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(value: false);
				gc.CollectItem(17);
			}
			else if ((hitInfo.transform.name == "Pickup_TrollItem") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(false);
				gc.CollectItem(18);
			}
			else if ((hitInfo.transform.name == "Pickup_Objection") & (Vector3.Distance(player.position, base.transform.position) < 10f))
			{
				hitInfo.transform.gameObject.SetActive(false);
				gc.CollectItem(19);
			}
		}
	}
}
