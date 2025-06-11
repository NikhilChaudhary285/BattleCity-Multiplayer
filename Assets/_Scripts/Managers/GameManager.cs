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

        if (newState == GameState.GameOver)
        {
            UIManager.Instance?.ShowGameOver(); // Trigger GameOver UI animation

            // Automatically restart after delay
            //StartCoroutine(SceneLoader.Instance.ReloadAfterDelay(2f)); // 2 second delay
        }
    }

	private void Start()
	{
		SetState(GameState.Playing); // or load menu scene
	}
}
