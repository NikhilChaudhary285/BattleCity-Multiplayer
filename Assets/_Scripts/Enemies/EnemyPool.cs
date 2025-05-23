using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	public static EnemyPool Instance;
	[SerializeField] private EnemyTankController enemyPrefab;
	private Queue<EnemyTankController> pool = new();

	private void Awake() => Instance = this;

	public EnemyTankController GetEnemy()
	{
		if (pool.Count == 0)
		{
			var newEnemy = Instantiate(enemyPrefab);
			newEnemy.gameObject.SetActive(false);
			pool.Enqueue(newEnemy);
		}

		var enemy = pool.Dequeue();
		enemy.gameObject.SetActive(true);
		return enemy;
	}

	public void ReturnEnemy(EnemyTankController enemy)
	{
		enemy.gameObject.SetActive(false);
		pool.Enqueue(enemy);
	}
}
