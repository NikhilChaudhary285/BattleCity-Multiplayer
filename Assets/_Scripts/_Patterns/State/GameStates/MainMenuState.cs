using UnityEngine;
using static GameManager;

public class MainMenuState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: MainMenu");

		// Example: setup menu music, UI, etc.
		//GameManager.Instance.SetState(GameState.Playing); // Transition to Playing state to initialize core gameplay systems like wave spawning and player control
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting MainMenu...");
	}
}

