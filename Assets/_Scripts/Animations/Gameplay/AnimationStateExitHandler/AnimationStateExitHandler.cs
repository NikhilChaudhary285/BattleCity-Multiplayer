using UnityEngine;
using System;

public class AnimationStateExitHandler : MonoBehaviour
{
	[Tooltip("Animator whose state you want to track")]
	[SerializeField] private Animator animator;

	[Tooltip("State name to listen for")]
	[SerializeField] private string stateName;

	[Tooltip("Delay after animation exits before callback")]
	[SerializeField] private float delayAfterExit = 0f;

	[Tooltip("Should reload the gameplay scene after this animation ends?")]
	[SerializeField] private bool reloadGameplayScene = false;

	[Tooltip("Optional: UnityEvent, delegate, or callback can also be added here later")]
	public Action OnAnimationComplete;

	private bool hasTriggered = false;

	private void Reset()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (hasTriggered || animator == null) return;

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		if (stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1f)
		{
			hasTriggered = true;
			Debug.Log($"[ANIMATION COMPLETE] '{stateName}' finished on {gameObject.name}");

			if (reloadGameplayScene)
				SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.ReloadSceneAfterDelay(2f, Scene.GamePlay));

			if (OnAnimationComplete != null)
				OnAnimationComplete.Invoke();
		}
	}

	private void OnDisable()
	{
		hasTriggered = false;
	}
}
