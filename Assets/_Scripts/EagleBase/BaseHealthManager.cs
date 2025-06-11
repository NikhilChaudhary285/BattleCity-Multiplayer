using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BaseHealthManager : MonoBehaviour, IDamageable
{
	public int maxHealth = 1;
	private int currentHealth;
	public GameObject eagleBaseExplosionPrefab;

    private void Awake()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage()
	{
		currentHealth --;
		if (currentHealth <= 0)
		{
            // Storing Spawn explosion Position While hitted from enemy bullet
            Vector3 explosionPos = transform.position;
            // Enemy bullet hit player: destroy player
            Destroy(gameObject);
            // Spawn explosion at player world position
            if (eagleBaseExplosionPrefab != null)
            {
                Instantiate(eagleBaseExplosionPrefab, explosionPos, Quaternion.identity);
            }
			Debug.Log("Eagle Base Destroyed!");
            GameManager.Instance.SetState(GameManager.GameState.GameOver);
		}
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        // 2. EagleBase collides with a valid target (enemy)
        if (collision.gameObject.GetComponent<EnemyTankController>() != null)
        {
            // Spawn explosion at eagleBase world position
            Instantiate(eagleBaseExplosionPrefab, transform.position, Quaternion.identity);
            // Eagle Base Destroyed
            Debug.Log("Eagle Base Destroyed!");
            GameManager.Instance.SetState(GameManager.GameState.GameOver);
            // Enemy hit eagleBase: destroy eagleBase
            Destroy(gameObject);
        }
    }
}
