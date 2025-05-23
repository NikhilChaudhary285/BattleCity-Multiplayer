using UnityEngine;

public class DeadState : IEnemyState
{
	public void Execute(EnemyTankController enemy)
	{
		Debug.Log("DeadState Executing...");
		//throw new System.NotImplementedException();
	}
}
