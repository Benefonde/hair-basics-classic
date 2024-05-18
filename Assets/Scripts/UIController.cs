using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public CursorControllerScript cc;

	private bool joystickEnabled;

	public bool controlMouse;

	public bool unlockOnStart;

	public bool uiControlEnabled;

	public Selectable firstButton;

	public string buttonTag;

	public bool usingJoystick => false;

	private void Start()
	{
		if (unlockOnStart & !joystickEnabled)
		{
			cc.UnlockCursor();
		}
	}

	private void OnEnable()
	{
		UpdateControllerType();
	}

	private void Update()
	{
		UpdateControllerType();
	}

	public void SwitchMenu()
	{
		UpdateControllerType();
	}

	private void UpdateControllerType()
	{
		if (!joystickEnabled & usingJoystick)
		{
			joystickEnabled = true;
			if (controlMouse)
			{
				cc.LockCursor();
			}
		}
		else if (joystickEnabled & !usingJoystick)
		{
			joystickEnabled = false;
			if (controlMouse)
			{
				cc.UnlockCursor();
			}
		}
		UIUpdate();
	}

	private void UIUpdate()
	{
		if (!uiControlEnabled)
		{
			return;
		}
		if (joystickEnabled)
		{
			if ((EventSystem.current.currentSelectedGameObject.tag != buttonTag) & (firstButton != null))
			{
				firstButton.Select();
				firstButton.OnSelect(null);
			}
		}
	}

	public void EnableControl()
	{
		uiControlEnabled = true;
	}

	public void DisableControl()
	{
		uiControlEnabled = false;
	}
}
