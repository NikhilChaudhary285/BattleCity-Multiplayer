using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
	public static BulletFactory Instance;

	public GameObject bulletPrefab;
	private Queue<GameObject> pool = new Queue<GameObject>();

	void Awake()
	{
		if (Instance == null) Instance = this;
	}

	public GameObject GetBullet(Vector3 pos, Quaternion rot)
	{
		GameObject bullet = pool.Count > 0 ? pool.Dequeue() : Instantiate(bulletPrefab);
		bullet.transform.position = pos;
		bullet.transform.rotation = rot;
		bullet.SetActive(true);
		return bullet;
	}

	public void ReturnBullet(GameObject bullet)
	{
		bullet.SetActive(false);
		pool.Enqueue(bullet);
	}
}
