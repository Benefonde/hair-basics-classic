using TMPro;
using UnityEngine;

public class DetentionTextScript : MonoBehaviour
{
	public DoorScript door;

	private TMP_Text text;

	private void Start()
	{
		text = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (door.detentionTime > 0f)
		{
			text.text = $"You have depression! \n{Mathf.CeilToInt(door.detentionTime)} t i m e remains!";
		}
		else
		{
			text.text = string.Empty;
		}
	}
}
