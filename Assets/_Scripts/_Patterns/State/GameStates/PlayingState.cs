using UnityEngine;

public class PlayingState : IGameState
{
	public void EnterState(GameManager gameManager)
	{
		Debug.Log("State: Playing");

		SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.ReloadSceneAfterDelay(2f, Scene.GamePlay));
		Time.timeScale = 1f;
	}

	public void ExitState(GameManager gameManager)
	{
		Debug.Log("Exiting Playing...");
		// Pause cleanup if needed
	}
}
