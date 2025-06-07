using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("CurrentHealth used for player health")]
    private int currentHealth;

    [Tooltip("Controller used for player tank ctrl")]
    private PlayerTankController controller;

    public void Init(PlayerTankController ctrl, int maxHealth)
    {
        controller = ctrl;
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            // Storing Spawn explosion Position While hitted from enemy bullet
            Vector3 explosionPos = transform.position;
            // Enemy bullet hit player: destroy player
            Destroy(gameObject);
            // Spawn explosion at player world position
            if (controller.Model.playerExplosionPrefab != null)
            {
                Instantiate(controller.Model.playerExplosionPrefab, explosionPos, Quaternion.identity);
            }
        }
    }
}
