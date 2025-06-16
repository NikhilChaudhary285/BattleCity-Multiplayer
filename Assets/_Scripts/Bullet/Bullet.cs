using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    private float timer;
    private Rigidbody2D rb;
    [SerializeField] private GameObject brickExplosionPrefab;
    [SerializeField] private Transform[] colliderDetectionPoints; // assign in Inspector

    [TagField][SerializeField] private string neglectCollisionTag; // Same-team tag (ignored)

    public bool isEnemyBullet;

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
        {
            BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Ignore hitting same team (enemy hits enemy or player hits player)
        if (collision.CompareTag(neglectCollisionTag))
        {
            Debug.Log($"[IGNORED] Bullet hit same team: {neglectCollisionTag}");
            return;
        }

        // 2. Bullet hits a valid target (enemy or player or eagleBase)
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log($"[HIT] Bullet hit: {collision.gameObject.tag}");

            // Enemy bullet hit player: destroy player
            if (isEnemyBullet && collision.TryGetComponent<PlayerTankController>(out var player))
            {
                player.health.TakeDamage();
            }
            // Enemy bullet hit eagleBase: eagleBase will take damage
            else if (isEnemyBullet && collision.TryGetComponent<BaseHealthManager>(out var eagleBase))
            {
                eagleBase.TakeDamage();
            }
            // Player bullet hit enemy: enemy will take damage
            else
            {
                if (collision.TryGetComponent<EnemyTankController>(out var enemy))
                {
                    enemy.health.TakeDamage();
                }
                else
                {
                    Debug.LogWarning("[ERROR] Hit an object tagged as Enemy but missing EnemyTankController.");
                }
            }

            BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
            return;
        }

        // 3. Bullet hits another bullet
        if (collision.TryGetComponent<Bullet>(out Bullet otherBullet))
        {
            Debug.Log("[HIT] Bullet collided with another bullet. Destroying both.");
            BulletFactory.Instance.ReturnBullet(otherBullet.gameObject, otherBullet.isEnemyBullet);
            BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
            return;
        }

		// 4. Tilemap Destruction Handling
		Tilemap tilemap = collision.GetComponent<Tilemap>();

		if (tilemap != null)
		{
			// Loop through each detection point to check for tile overlap
			foreach (Transform detector in colliderDetectionPoints)
			{
				// Get the cell corresponding to the detector's world position
				Vector3 hitPosition = detector.position;
				Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);

				// Get the tile at the calculated cell
				TileBase tile = tilemap.GetTile(cellPosition);

				// Check if the tile is destructible
				if (tile is DestructibleTile destructibleTile && destructibleTile.isDestructible)
				{
					// Destroy the tile by setting it to null
					tilemap.SetTile(cellPosition, null);

					// Spawn an explosion effect at the tile's center position
					Vector3 explosionPosition = tilemap.GetCellCenterWorld(cellPosition);
					Instantiate(brickExplosionPrefab, explosionPosition, Quaternion.identity);

					// Return the bullet to the pool (based on its source: enemy/player)
					BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);

					// Optional: Uncomment below if you want to destroy only one tile per bullet hit or not want to destroy both if bullet hit at center of them (attached tiles)
					return;
				}
			}
		}

		// 5. Default behavior
		BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
    }



}
