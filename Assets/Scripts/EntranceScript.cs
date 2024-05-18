using UnityEngine;

public class EntranceScript : MonoBehaviour
{
	public GameControllerScript gc;

	public Material map;

	public MeshRenderer wall;

	public void Lower()
	{
		base.transform.position = base.transform.position - new Vector3(0f, 10f, 0f);
		if (gc.finaleMode)
		{
			wall.material = map;
		}
		transform.Find("ExitMap").gameObject.SetActive(false);
	}

	public void Raise()
	{
		base.transform.position = base.transform.position + new Vector3(0f, 10f, 0f);
		transform.Find("ExitMap").gameObject.SetActive(true);
	}
}
