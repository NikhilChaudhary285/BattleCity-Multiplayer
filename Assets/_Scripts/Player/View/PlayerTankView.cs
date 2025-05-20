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
	}

	public void Rotate(float rotationInput)
	{
		rb.MoveRotation(rb.rotation + rotationInput * controller.Model.rotationSpeed * Time.fixedDeltaTime);
	}

	public void Shoot()
	{
		if (!controller.Model.CanFire()) return;

		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, firePoint.rotation);
		controller.Model.RecordFireTime();
	}
}
