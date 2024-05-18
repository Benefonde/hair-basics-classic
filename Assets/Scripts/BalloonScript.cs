using UnityEngine;

public class BalloonScript : MonoBehaviour
{
	private void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), GameObject.Find("Player").GetComponent<CharacterController>());
		transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1, 1, 1, 1, 1, 1);
		this.ChangeDirection();
	}

	private void Update()
	{
		this.directionTime -= Time.deltaTime;
		if (this.directionTime <= 0f)
		{
			this.ChangeDirection();
			this.directionTime = 10f;
		}
		this.rb.velocity = this.direction * this.speed;
	}

	private void ChangeDirection()
	{
		this.direction.x = UnityEngine.Random.Range(-1f, 1f);
		this.direction.z = UnityEngine.Random.Range(-1f, 1f);
		this.direction = this.direction.normalized;
	}

	[SerializeField]
	private Rigidbody rb;

	private float directionTime = 10f;

	private Vector3 direction;

	public float speed = 10f;
}