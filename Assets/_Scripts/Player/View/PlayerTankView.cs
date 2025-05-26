using UnityEngine;

public class PlayerTankView : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform firePoint;

    private PlayerTankController controller;

    public void SetController(PlayerTankController ctrl) => controller = ctrl;

    public void Move(Vector2 moveDir)
    {
        rb.velocity = moveDir * controller.Model.moveSpeed;

        // Instantly rotate tank to match 90-degree movement direction
        if (moveDir != Vector2.zero)
        {
            float angle = 0f;

            if (moveDir == Vector2.up) angle = 90f;
            else if (moveDir == Vector2.down) angle = -90f;
            else if (moveDir == Vector2.left) angle = 180f;
            else if (moveDir == Vector2.right) angle = 0f;

            rb.rotation = angle;
        }
    }

    public void Rotate(float rotationInput)
    {
        // No longer used with 90-degree snap rotation — left here for flexibility
    }

    public void Shoot()
    {
        if (!controller.CanFire()) return;

        // Convert tank rotation to direction vector
        float angle = rb.rotation * Mathf.Deg2Rad;
        Vector2 shootDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        // Spawn bullet with no rotation
        GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, Quaternion.identity, firePoint, false);
        bullet.GetComponent<Bullet>().Fire(shootDirection);

        controller.RecordFireTime();
    }
}

