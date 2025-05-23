using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
	[Tooltip("Strategy Pattern for AI Movement")]
	private IEnemyMovementStrategy movementStrategy;

	[Tooltip("Current AI state based on the State Pattern (e.g., Move, Attack, Dead, Patrol, Chase)")]
	private IEnemyState currentState;

	[Tooltip("Rigidbody2D component used for enemy tank movement and physics")]
	public Rigidbody2D rb;

	[Tooltip("ScriptableObject containing stats like speed, health, and damage for the tank")]
	public TankStatsSO Stats;

	public void SetStrategy(IEnemyMovementStrategy strategy)
	{
		movementStrategy = strategy;
	}
	public void SetState(IEnemyState state)
	{
		currentState = state;
	}
	public void Init(TankStatsSO stats)
	{
		this.Stats = stats;

		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		// Reset velocity and position-related settings if needed
		rb.velocity = Vector2.zero;

		// Optional: Reset health, shooting timers, etc.
	}

	void Update()
	{
		movementStrategy?.Move(this);
		currentState?.Execute(this);
	}

}
