using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera m_Camera;

	private void Start()
	{
		m_Camera = Camera.main;
	}

	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.Euler(Vector3.up * this.m_Camera.transform.rotation.eulerAngles.y + Vector3.right * 0);
	}
}
