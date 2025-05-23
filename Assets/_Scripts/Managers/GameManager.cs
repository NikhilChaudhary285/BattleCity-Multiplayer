using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public enum GameState { MainMenu, Playing, Paused, GameOver }
	public GameState CurrentState { get; private set; }

	[Header("References")]
	public BaseHealthManager Base; // Reference to the Eagle Base

	public void SetState(GameState newState)
	{
		CurrentState = newState;
		Debug.Log("Game State Changed to: " + newState);
	}

	private void Start()
	{
		SetState(GameState.Playing); // or load menu scene
	}
}
