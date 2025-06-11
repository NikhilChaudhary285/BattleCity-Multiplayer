using UnityEngine;

public class GameOverAnimationWatcher : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private string stateName = "GameOverFadeIn"; // the name of your animation state
	[SerializeField] private float restartDelay = 2f;

	private bool hasTriggeredReload = false;

	private void Update()
	{
		if (hasTriggeredReload || animator == null) return;

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// Is the GameOverFadeIn animation playing and nearly done?
		if (stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1f)
		{
			hasTriggeredReload = true;
			Debug.Log("GameOver animation completed — reloading Gameplay scene...");
			StartCoroutine(SceneLoader.Instance.ReloadAfterDelay(restartDelay));
		}
	}

	private void OnDisable()
	{
		hasTriggeredReload = false;
	}
}
