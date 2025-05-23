using UnityEngine;

public class TargetBaseStrategy : IEnemyMovementStrategy
{
	private Transform baseTarget;

	public TargetBaseStrategy(Transform baseTransform)
	{
		baseTarget = baseTransform;
	}

	public void Move(EnemyTankController enemy)
	{
		Vector2 dir = (baseTarget.position - enemy.transform.position).normalized;
		enemy.rb.velocity = dir * enemy.Stats.moveSpeed;
	}
}

