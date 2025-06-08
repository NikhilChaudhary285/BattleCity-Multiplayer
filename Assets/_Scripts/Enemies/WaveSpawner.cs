using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private TankStatsSO[] enemyTypes;
	[SerializeField] private float spawnDelay = 2f;

	public void SpawnWave(int count)
	{
		StartCoroutine(SpawnCoroutine(count));
	}

	private IEnumerator SpawnCoroutine(int count)
	{
		for (int i = 0; i < count; i++)
		{
			EnemyTankController enemy = EnemyPool.Instance.GetEnemy();
			enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			enemy.Init(enemyTypes[Random.Range(0, enemyTypes.Length)]);

			//enemy.SetStrategy(new TargetBaseStrategy(GameManager.Instance.Base.transform));
            enemy.SetStrategy(new RandomDirectionStrategy());
            enemy.SetState(new MoveState());

			yield return new WaitForSeconds(spawnDelay);
		}
	}
}
