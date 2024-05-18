using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public UIController uc;

	public Selectable firstButton;

	public GameObject back;

	public void OnEnable()
	{
		uc.firstButton = firstButton;
		uc.SwitchMenu();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel") && back != null)
		{
			back.SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}
	}
}
