using UnityEngine;
using System.Collections;

public class EnemyTankController : MonoBehaviour
{
    [Tooltip("Main body SpriteRenderer used to display the enemy's tank base sprite")]
    [SerializeField] private SpriteRenderer bodyRenderer;

    [Tooltip("Upper part SpriteRenderer used to render the rotating tank cannon or turret")]
    [SerializeField] private SpriteRenderer upperPartBodyRenderer;

    [Tooltip("Rigidbody2D component used for enemy tank movement and physics")]
    public Rigidbody2D rb;

    [Tooltip("Firing point (position and direction of bullet)")]
    public Transform enemyFirePoint;

    [Tooltip("ScriptableObject containing stats like speed, health, and damage for the tank")]
    public TankStatsSO Stats;

    [Tooltip("Enemy health handler for managing hit points and death logic")]
    public EnemyHealth health;

    [Tooltip("Enemy OnDeath Action handler for updating activeEnemies in WaveSpawner")]
	public System.Action OnDeath;

	[Tooltip("Strategy Pattern for AI Movement")]
    private IEnemyMovementStrategy movementStrategy;

    [Tooltip("Current AI state based on the State Pattern (e.g., Move, Attack, Dead, Patrol, Chase)")]
    private IEnemyState currentState;

    [Tooltip("EnemyExplosionPrefab (For Explosion Effect whenever enemy die)")]
    [HideInInspector] public GameObject enemyExplosionPrefab;

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

        health.Init(this, Stats.maxHealth);

        if (bodyRenderer != null && Stats.tankSprite != null)
        {
            bodyRenderer.sprite = Stats.tankSprite;
            upperPartBodyRenderer.sprite = Stats.upperTankSprite;
        }

        if (bodyRenderer != null && Stats.tankSpriteColor != null)
        {
            bodyRenderer.color = Stats.tankSpriteColor;
            upperPartBodyRenderer.color = Stats.tankSpriteColor;            
        }

        enemyExplosionPrefab = Stats.enemyExplosionPrefab;
    }

    void Update()
    {
        movementStrategy?.Move(this);
        currentState?.Execute(this);
    }

    public void ContinuousFire()
    {
        if (!Stats.isBurstFire)
        {
            if (Time.time - lastFireTime >= Stats.fireRate)
            {
                Fire();
                lastFireTime = Time.time;
            }
        }
        else
        {
            // If burst fire, start burst sequence if cooldown has passed
            if (Time.time - lastFireTime >= Stats.fireRate)
            {
                lastFireTime = Time.time;
                StartCoroutine(BurstFireCoroutine());
            }
        }
    }

    private IEnumerator BurstFireCoroutine()
    {
        for (int i = 0; i < Stats.burstCount; i++)
        {
            Fire();
            yield return new WaitForSeconds(Stats.burstDelay); // Small delay between each burst shot
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

        // Create rotation based on direction vector
        Quaternion bulletRotation = Quaternion.LookRotation(Vector3.forward, enemyShootDirection);

        GameObject bullet = BulletFactory.Instance.GetBullet(enemyFirePoint.position, bulletRotation, enemyFirePoint, true);

        // Set bullet velocity using enemyShootDirection
        bullet.GetComponent<Bullet>().Fire(enemyShootDirection);
    }

}
