using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MouseoverScript : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
	public UnityEvent mouseOver;

	public UnityEvent mouseLeave;

	public void OnSelect(BaseEventData eventData)
	{
		mouseOver.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOver.Invoke();
	}

	public void OnDeselect(BaseEventData eventData)
	{
		mouseLeave.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseLeave.Invoke();
	}
}
