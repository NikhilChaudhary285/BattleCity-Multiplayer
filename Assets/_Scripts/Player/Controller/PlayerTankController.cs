using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
	public PlayerTankModel modelData;
	public PlayerTankView view;

	public PlayerTankModel Model => modelData;

	void Start()
	{
		view.SetController(this);
	}

	void Update()
	{
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		float rotationInput = -Input.GetAxisRaw("Horizontal");

		view.Move(moveInput.normalized);
		view.Rotate(rotationInput);

		if (Input.GetKeyDown(KeyCode.Space))
			view.Shoot();
	}
}
