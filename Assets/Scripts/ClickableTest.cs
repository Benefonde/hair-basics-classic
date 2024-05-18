using UnityEngine;

public class ClickableTest : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo) && hitInfo.transform.name == "MathNotebook")
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
