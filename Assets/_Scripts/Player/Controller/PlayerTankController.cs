using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTankController : MonoBehaviour
{
	public PlayerTankModel modelData;
	public PlayerTankView view;

	private PlayerInputActions input;
	private Vector2 moveInput;
	private bool firePressed;

	private float lastFiredTime = -999f;

	public PlayerTankModel Model => modelData;

	public bool CanFire() => Time.time >= lastFiredTime + modelData.fireCooldown;
	public void RecordFireTime() => lastFiredTime = Time.time;

	private void Awake()
	{
		input = new PlayerInputActions();
	}

	private void OnEnable()
	{
		input.Enable();

		input.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
		input.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

		input.Gameplay.Fire.performed += ctx => firePressed = true;
	}

	private void OnDisable()
	{
		input.Disable();
	}

	void Start()
	{
		view.SetController(this);
	}

	void Update()
	{
		float rotationInput = -moveInput.x;
		view.Move(moveInput.normalized);
		view.Rotate(rotationInput);

		if (firePressed)
		{
			view.Shoot();
			firePressed = false;
		}
	}
}
