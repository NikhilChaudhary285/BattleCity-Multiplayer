using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    private float timer;
    private Rigidbody2D rb;

    [SerializeField] private GameObject explosionPrefab;

    [TagField][SerializeField] private string neglectCollisionTag; // Same-team tag (ignored)
    [TagField][SerializeField] private string acceptCollisionTag;  // Opponent tag (destroyed/pool)

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

        // 2. Bullet hits a valid target (enemy or player)
        if (collision.CompareTag(acceptCollisionTag))
        {
            Debug.Log($"[HIT] Bullet hit: {acceptCollisionTag}");

            if (isEnemyBullet)
            {
                // Enemy bullet hit player: destroy player
                Destroy(collision.gameObject);
            }
            else
            {
                // Player bullet hit enemy: return enemy to pool
                if (collision.TryGetComponent<EnemyTankController>(out var enemy))
                {
                    EnemyPool.Instance.ReturnEnemy(enemy);
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

        // 4. Tilemap destruction (unchanged)
        Tilemap tilemap = collision.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Vector3 hitPos = transform.position;
            Vector3Int cell = tilemap.WorldToCell(hitPos);
            TileBase tile = tilemap.GetTile(cell);

            if (tile is DestructibleTile destructibleTile && destructibleTile.isDestructible)
            {
                tilemap.SetTile(cell, null); // remove the tile                   

                // Spawn explosion at tile's world position
                Vector3 explosionPos = tilemap.GetCellCenterWorld(cell);
                Instantiate(explosionPrefab, explosionPos, Quaternion.identity);

                BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
                return;
            }
        }

        // 5. Default behavior
        BulletFactory.Instance.ReturnBullet(gameObject, isEnemyBullet);
    }
}
