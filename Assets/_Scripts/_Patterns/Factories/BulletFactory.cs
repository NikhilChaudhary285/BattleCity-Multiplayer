using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

    [Header("BULLET PREFABS")]
    public GameObject playerBulletPrefab;
    public GameObject enemyBulletPrefab;

    private Queue<GameObject> playerBulletPool = new Queue<GameObject>();
    private Queue<GameObject> enemyBulletPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetBullet(Vector3 pos, Quaternion rot, Transform parent, bool isEnemy)
    {
        GameObject bullet;
        if (isEnemy)
        {
            bullet = enemyBulletPool.Count > 0 ? enemyBulletPool.Dequeue() : Instantiate(enemyBulletPrefab);
        }
        else
        {
            bullet = playerBulletPool.Count > 0 ? playerBulletPool.Dequeue() : Instantiate(playerBulletPrefab);
        }

        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
        bullet.transform.localScale = new Vector3(2f, 1.5f, 0f);
        bullet.transform.SetParent(parent);
        bullet.SetActive(true);

        // Store type info inside bullet for return use
        bullet.GetComponent<Bullet>().isEnemyBullet = isEnemy;

        return bullet;
    }

    public void ReturnBullet(GameObject bullet, bool isEnemy)
    {
        bullet.SetActive(false);

        if (isEnemy)
            enemyBulletPool.Enqueue(bullet);
        else
            playerBulletPool.Enqueue(bullet);
    }
}
