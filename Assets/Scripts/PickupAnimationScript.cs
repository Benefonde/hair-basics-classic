using System;
using UnityEngine;

public class PickupAnimationScript : MonoBehaviour
{
	private Transform itemPosition;

	private void Start()
	{
		itemPosition = GetComponent<Transform>();
	}

	private void Update()
	{
		itemPosition.localPosition = new Vector3(0f, (Mathf.Sin((float)(Time.frameCount) / 5) / 1.75f) + 1f, 0f);
	}


}
