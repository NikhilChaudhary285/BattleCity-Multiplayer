using UnityEngine;

public class BaseHealthManager : MonoBehaviour, IDamageable
{
	public int maxHealth = 1;
	private int currentHealth;
	public GameObject eagleBaseExplosionPrefab;

	private void Awake()
	{
		GameManager.Instance.RegisterEagleBase(this); // Registering EagleBase To GameManager
		currentHealth = maxHealth;
	}

	public void TakeDamage()
	{
		currentHealth--;
		if (currentHealth <= 0)
		{
			// Storing Spawn explosion Position While hitted from enemy bullet
			Vector3 explosionPos = transform.position;
			// Enemy bullet hit eagleBase: destroy eagleBase
			Destroy(gameObject);
			Debug.Log("Eagle EagleBase Destroyed!");
			// Spawn explosion at eagleBase world position
			if (eagleBaseExplosionPrefab != null)
			{
				Instantiate(eagleBaseExplosionPrefab, explosionPos, Quaternion.identity);
			}
			// Setting State To GameOver State
			GameManager.Instance.SetState(GameManager.GameState.GameOver);
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision == null) return;
		// 2. EagleBase collides with a valid target (enemy)
		if (collision.gameObject.GetComponent<EnemyTankController>() != null)
		{
			// Storing Spawn explosion Position While hitted from enemy bullet
			Vector3 explosionPos = transform.position;
			// Enemy bullet hit eagleBase: destroy eagleBase
			Destroy(gameObject);
			Debug.Log("Eagle EagleBase Destroyed!");
			// Spawn explosion at eagleBase world position
			if (eagleBaseExplosionPrefab != null)
			{
				Instantiate(eagleBaseExplosionPrefab, explosionPos, Quaternion.identity);
			}
			// Setting State To GameOver State
			GameManager.Instance.SetState(GameManager.GameState.GameOver);
		}
	}
}
