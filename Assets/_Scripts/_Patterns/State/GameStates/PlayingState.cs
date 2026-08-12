using UnityEngine;

public class PlayingState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: Playing");
		PhotonManager.Instance.ReloadSceneAfterDelay(2f, Scene.Gameplay);

		Time.timeScale = 1f;
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting Playing...");
		// Pause cleanup if needed
	}
}
