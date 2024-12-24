using System;
using UnityEngine;

public class PickupAnimationScript : MonoBehaviour
{
	private Transform itemPosition;

	float thing;

	private void Start()
	{
		itemPosition = GetComponent<Transform>();
	}

	private void Update()
	{
		thing += Time.deltaTime;
		itemPosition.localPosition = new Vector3(0f, (Mathf.Sin((float)(thing) * 20) / 1.75f) + 1f, 0f);
	}
}
