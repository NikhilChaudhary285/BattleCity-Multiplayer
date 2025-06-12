#region ---- Old One ----
/*using System.Collections;
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

			//enemy.SetStrategy(new TargetBaseStrategy(GameManager.Instance.EagleBase.transform));
            enemy.SetStrategy(new RandomDirectionStrategy());
            enemy.SetState(new MoveState());

			yield return new WaitForSeconds(spawnDelay);
		}
	}
}
*/
#endregion ---- Old One ----

using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private TankStatsSO[] enemyTypes;
	[SerializeField] private float spawnDelay = 2f;

	[Header("Wave Progression Settings")]
	[SerializeField] private int totalWaves = 5;
	private int currentWave = 0;

	[Header("Wave Enemies Settings")]
	private int activeEnemies = 0;

	[Tooltip("Prevents Coroutine Memory Leak")] 
	private Coroutine enemySpawnWave;

	private void Awake()
	{
		GameManager.Instance.RegisterWaveSpawner(this); // Registering EagleBase To GameManager
	}

	public void StartWaves()
	{
		currentWave = 1;

		UIManager.Instance.SetWaveText(currentWave); // notify UI
		UIManager.Instance.SetEnemyCount(0); // start at 0, gets incremented in spawn loop

		SpawnWave(GetEnemyCountForWave(currentWave));
	}
	public void OnWaveCleared()
	{
		if (currentWave < totalWaves)
		{
			currentWave++;

			UIManager.Instance.SetWaveText(currentWave); // update UI
			UIManager.Instance.SetEnemyCount(0); // start at 0, gets incremented in spawn loop

			SpawnWave(GetEnemyCountForWave(currentWave));
		}
	}
	public void SpawnWave(int count)
	{
		if (enemySpawnWave != null) // Prevents Coroutine Memory Leak
			StopCoroutine(enemySpawnWave);
		else
			enemySpawnWave = StartCoroutine(SpawnCoroutine(count));
	}
	private IEnumerator SpawnCoroutine(int count)
	{
		for (int i = 0; i < count; i++)
		{
			EnemyTankController enemy = EnemyPool.Instance.GetEnemy();
			enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			enemy.Init(enemyTypes[Random.Range(0, enemyTypes.Length)]);

			// Use basic AI pattern
			enemy.SetStrategy(new RandomDirectionStrategy());
			enemy.SetState(new MoveState());

			// Track enemy count and subscribe to death event
			enemy.OnDeath = HandleEnemyDeath;

			activeEnemies++;
			UIManager.Instance.SetEnemyCount(activeEnemies);

			yield return new WaitForSeconds(spawnDelay);
		}
	}
	private int GetEnemyCountForWave(int wave)
	{
		return 2 + wave * 2; // Example: scale difficulty per wave
	}

	private void HandleEnemyDeath()
	{
		activeEnemies--;
		UIManager.Instance.SetEnemyCount(activeEnemies);
		if (activeEnemies <= 0)
		{
			Debug.Log("Wave cleared!");
			OnWaveCleared();
		}
	}
}
