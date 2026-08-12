using UnityEngine;

public class MoveState : IEnemyState
{
    public void Execute(EnemyTankController enemy)
    {
        // Movement already handled by strategy
        //Debug.Log("MoveState Executing...");
        enemy.ContinuousFire();
    }
}
