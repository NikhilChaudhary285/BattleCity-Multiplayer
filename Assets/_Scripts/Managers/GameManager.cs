using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public enum GameState { MainMenu, Playing, Paused, GameOver }
	public GameState CurrentState { get; private set; }

	[Header("References")]

	[Tooltip("Wave Spawner")][SerializeField] private WaveSpawner waveSpawner; // Reference to the Wave Spawner

	[Tooltip("Eagle Base Health Manager")] public BaseHealthManager EagleBase; // Reference to the Eagle EagleBase

	public void SetState(GameState newState)
	{
		CurrentState = newState;
		Debug.Log("Game State Changed to: " + newState);

		if (newState == GameState.GameOver)
		{
			UIManager.Instance?.ShowGameOver(); // Trigger GameOver UI animation
		}
	}

	private void Start()
	{
		SetState(GameState.Playing); // or load menu scene
		// Start waves of enemy spawing
		waveSpawner.StartWaves();
	}
}
