public interface IEnemyState // Create classes like MoveState, AttackState, DeadState.
{
	void Execute(EnemyTankController enemy);
}
