using UnityEngine;
using UnityEngine.AI;

public class TargetBaseStrategy : IEnemyMovementStrategy
{
    private Transform target;

    public TargetBaseStrategy(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void Move(EnemyTankController enemy)
    {
        Vector2 dir = (target.position - enemy.transform.position).normalized;
        enemy.rb.velocity = dir * enemy.Stats.moveSpeed;
    }
}