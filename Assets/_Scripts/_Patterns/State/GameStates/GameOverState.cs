using UnityEngine;

public class GameOverState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: GameOver");

		UIManager.Instance?.ShowGameOver();
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting GameOver...");
	}
}
