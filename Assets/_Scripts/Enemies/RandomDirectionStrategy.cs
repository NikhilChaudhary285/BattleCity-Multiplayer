using UnityEngine;

public class RandomDirectionStrategy : IEnemyMovementStrategy
{
    private Vector2 currentDirection;
    private float directionChangeTimer;

    public void Move(EnemyTankController enemy)
    {
        directionChangeTimer -= Time.deltaTime;

        if (directionChangeTimer <= 0f)
        {
            currentDirection = GetRandomDirection();
            directionChangeTimer = Random.Range(2f, 4f); // time to next change

            RotateTankToDirection(enemy, currentDirection);
        }

        enemy.rb.velocity = currentDirection * enemy.Stats.moveSpeed;
    }

    private Vector2 GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4);
        return randomIndex switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.down
        };
    }

    private void RotateTankToDirection(EnemyTankController enemy, Vector2 dir)
    {
        float angle = 0f;

        if (dir == Vector2.up) angle = 90f;
        else if (dir == Vector2.down) angle = -90f;
        else if (dir == Vector2.left) angle = 180f;
        else if (dir == Vector2.right) angle = 0f;

        enemy.rb.rotation = angle;
    }
}
