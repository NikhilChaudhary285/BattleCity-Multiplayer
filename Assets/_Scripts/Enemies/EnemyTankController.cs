using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    [Tooltip("Strategy Pattern for AI Movement")]
    private IEnemyMovementStrategy movementStrategy;

    [Tooltip("Current AI state based on the State Pattern (e.g., Move, Attack, Dead, Patrol, Chase)")]
    private IEnemyState currentState;

    [Tooltip("Rigidbody2D component used for enemy tank movement and physics")]
    public Rigidbody2D rb;

    [Tooltip("ScriptableObject containing stats like speed, health, and damage for the tank")]
    public TankStatsSO Stats;

    [Tooltip("Firing point (position and direction of bullet)")]
    public Transform enemyFirePoint;

    [Tooltip("LayerMask used for detecting obstacles (optional for future use)")]
    public LayerMask obstacleMask;

    private float lastFireTime;

    public void SetStrategy(IEnemyMovementStrategy strategy)
    {
        movementStrategy = strategy;
    }

    public void SetState(IEnemyState state)
    {
        currentState = state;
    }

    public void Init(TankStatsSO stats)
    {
        Stats = stats;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        lastFireTime = -Stats.fireRate;
    }

    void Update()
    {
        movementStrategy?.Move(this);
        currentState?.Execute(this);
    }

    public void ContinuousFire()
    {
        if (Time.time - lastFireTime >= Stats.fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    private void Fire()
    {
        // Convert tank rotation to direction vector 
        float angle = rb.rotation * Mathf.Deg2Rad;
        Vector2 enemyShootDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        // Spawn bullet with no rotation
        #region Don't Need this bullet type spawn inside this region logic without pooling bullet system until we want unique bullet prefab which have unique abilities except normal bullet
        //GameObject bullet = Instantiate(Stats.bulletPrefab, firePoint.position, Quaternion.identity, firePoint);
        #endregion
        GameObject bullet = BulletFactory.Instance.GetBullet(enemyFirePoint.position, Quaternion.identity, enemyFirePoint, true);

        // Set bullet velocity using enemyShootDirection
        bullet.GetComponent<Bullet>().Fire(enemyShootDirection);
    }
}
