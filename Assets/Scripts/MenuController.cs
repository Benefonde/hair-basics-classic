using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public UIController uc;

	public Selectable firstButton;

	public GameObject back;

	public GameObject EVILtitle;

	public bool EVILapplicable;

    public void OnEnable()
	{
		uc.firstButton = firstButton;
		uc.SwitchMenu();
		if (EVILapplicable && (System.DateTime.Now.Month == 10 && System.DateTime.Now.Day >= 20) || (System.DateTime.Now.Month == 11 && System.DateTime.Now.Day <= 7))
		{
			EVILtitle.SetActive(true);
			gameObject.SetActive(false);
		}
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
