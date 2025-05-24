using UnityEngine;
using UnityEngine.Tilemaps;

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
        // Ignore collision with specified tag (like "Enemy")
        if (collision.CompareTag(neglectCollisionTag))
            return;

        // If it hits the player, destroy the player GameObject
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            BulletFactory.Instance.ReturnBullet(gameObject);
            return;
        }

        // If it hits a tilemap (like the destructible brick layer)
        Tilemap tilemap = collision.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Vector3 hitPos = transform.position;
            Vector3Int cell = tilemap.WorldToCell(hitPos);
            TileBase tile = tilemap.GetTile(cell);

            if (tile is DestructibleTile destructibleTile && destructibleTile.isDestructible)
            {
                tilemap.SetTile(cell, null); // destroy the brick tile
                BulletFactory.Instance.ReturnBullet(gameObject);
                return;
            }
        }

        // If none of the above, return bullet to pool
        BulletFactory.Instance.ReturnBullet(gameObject);
    }
}
