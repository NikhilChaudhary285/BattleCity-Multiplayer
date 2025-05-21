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
		#region ---- OF ----

		/*if (!controller.Model.CanFire()) return;

		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, firePoint.rotation);
		Debug.Log($"Fired Bullet: {bullet.gameObject.GetInstanceID()}");
		controller.Model.RecordFireTime();*/
		#endregion ---- OF ----

		#region ---- NF ----

		if (!controller.CanFire()) return;

		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, firePoint.rotation);
		Debug.Log($"Fired Bullet: {bullet.gameObject.GetInstanceID()}");
		controller.RecordFireTime();
		#endregion ---- NF ----
	}
}
