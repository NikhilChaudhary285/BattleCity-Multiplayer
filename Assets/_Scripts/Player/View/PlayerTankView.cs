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
		#region ---- OF ----

		/*if (!controller.Model.CanFire()) return;

		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, firePoint.rotation);
		Debug.Log($"Fired Bullet: {bullet.gameObject.GetInstanceID()}");
		controller.Model.RecordFireTime();*/
		#endregion ---- OF ----
		#region ---- OF ----

		/*if (!controller.CanFire()) return;

		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, new Quaternion(0, 0, )*//*firePoint.rotation*//*);
		Debug.Log($"Fired Bullet: {bullet.gameObject.GetInstanceID()}");
		controller.RecordFireTime();*/
		#endregion ---- OF ----
		#region ---- OF ----

		/*if (!controller.CanFire()) return;

		// Get direction cannon is pointing
		Vector2 shootDirection = firePoint.up;

		// Spawn bullet at position with no rotation
		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, Quaternion.identity);

		// Fire bullet manually in correct direction
		bullet.GetComponent<Bullet>().Fire(shootDirection);

		controller.RecordFireTime();*/
		#endregion ---- OF ----

		#region ---- NF ----

		if (!controller.CanFire()) return;

		// Convert tank rotation to direction vector
		float angle = rb.rotation * Mathf.Deg2Rad;
		Vector2 shootDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

		// Spawn bullet with no rotation
		GameObject bullet = BulletFactory.Instance.GetBullet(firePoint.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().Fire(shootDirection);

		controller.RecordFireTime();
		#endregion ---- NF ----
	}
}

