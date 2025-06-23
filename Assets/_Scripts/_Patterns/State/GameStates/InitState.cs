using UnityEngine;
using static GameManager;

public class InitState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: Init");

		// Example: setup managers: Audio, Game, UI etc.
		SceneLoader.Instance.LoadMainMenu();
		GameManager.Instance.SetState(GameState.MainMenu);  // Transition to MainMenu state: To Start menu logic or preload (if needed) 
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting MainMenu...");
	}
}

