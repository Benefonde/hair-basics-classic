using UnityEngine;

public class MinimapShow : MonoBehaviour
{
	public GameObject Minimap;

	public bool mapUp;

	private void Update()
	{
		if (Input.GetKey(KeyCode.Tab))
		{
			Minimap.SetActive(value: true);
			mapUp = true;
		}
		else
		{
			Minimap.SetActive(value: false);
			mapUp = false;
		}
	}
}
