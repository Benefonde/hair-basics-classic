using UnityEngine;

public class Minimap : MonoBehaviour
{
	public Transform player;
	public Transform theOtherPlayer;
	public int upRotation;

    private void Start()
    {
        if (FindObjectOfType<GameControllerScript>().mode == "panino")
        {
			player = theOtherPlayer;
        }
    }

    private void LateUpdate()
	{
		Vector3 position = player.position;
		position.y = base.transform.position.y;
		base.transform.position = position;
		base.transform.rotation = Quaternion.Euler(upRotation, player.eulerAngles.y, 0f);
	}
}
