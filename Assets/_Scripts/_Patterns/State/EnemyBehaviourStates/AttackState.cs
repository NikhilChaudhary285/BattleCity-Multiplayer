using UnityEngine;

public class AttackState : IEnemyState
{
	public void Execute(EnemyTankController enemy)
	{
		Debug.Log("AttackState Executing...");
		//throw new System.NotImplementedException();
	}
}
