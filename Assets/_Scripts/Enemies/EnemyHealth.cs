using UnityEngine;

public class EnemyHealth     : MonoBehaviour
{
    [Tooltip("CurrentHealth used for enemy health")]
    private int currentHealth;

    [Tooltip("Controller used for enemy tank ctrl")]
    private EnemyTankController controller;

    public void Init(EnemyTankController ctrl, int maxHealth)
    {
        controller = ctrl;
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            // Player bullet hit enemy: return enemy to pool
            EnemyPool.Instance.ReturnEnemy(controller);
            // Storing Spawn explosion Position While hitted from player bullet
            Vector3 explosionPos = transform.position;
            // Spawn explosion at enemy world position
            if (controller.enemyExplosionPrefab != null)
            {
                Instantiate(controller.enemyExplosionPrefab, explosionPos, Quaternion.identity);
            }
        }
    }
}
