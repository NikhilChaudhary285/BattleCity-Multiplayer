using UnityEngine;

public class GameOverState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: GameOver");

		UIManager.Instance?.ShowGameOver();
		Time.timeScale = 0f;
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting GameOver...");
	}
}
