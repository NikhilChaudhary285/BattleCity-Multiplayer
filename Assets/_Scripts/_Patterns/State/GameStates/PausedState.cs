using UnityEngine;

public class PausedState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: Paused");

		Time.timeScale = 0f;
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting Paused...");

		Time.timeScale = 1f;
	}
}
