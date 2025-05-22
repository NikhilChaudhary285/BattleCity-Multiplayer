using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 10f;
	public float lifetime = 2f;
	private float timer;
	private Rigidbody2D rb;
	[TagField]
	[SerializeField] private string neglectCollisionTag;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void Fire(Vector2 direction)
	{
		rb.velocity = direction.normalized * speed;
		timer = 0f;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= lifetime)
			BulletFactory.Instance.ReturnBullet(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// damage logic here
		//if (!collision.CompareTag(neglectCollisionTag))
		BulletFactory.Instance.ReturnBullet(gameObject);
	}
}
