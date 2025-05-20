using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 10f;
	public float lifetime = 2f;
	private float timer;

	void OnEnable()
	{
		timer = 0f;
	}

	void Update()
	{
		transform.Translate(Vector2.up * speed * Time.deltaTime);
		timer += Time.deltaTime;

		if (timer >= lifetime)
			BulletFactory.Instance.ReturnBullet(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// damage logic here
		BulletFactory.Instance.ReturnBullet(gameObject);
	}
}
